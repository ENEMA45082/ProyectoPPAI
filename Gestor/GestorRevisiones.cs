using ProyectoPPAI.BaseDatos;
using ProyectoPPAI.Clases;
using ProyectoPPAI.Pantalla;
using System.Diagnostics;
using System.Linq;
using System.Numerics;

namespace ProyectoPPAI
{
    public class GestorRevisiones
    {
        // ========================            Atributos            ========================
        private List<EventoSismico> listaEventosOriginal = new();   // Todos los eventos generados
        private List<EventoSismico> listaEventosFiltrados = new();  // Solo los eventos autodetectados
        public EventoSismico? eventoSeleccionado;                   // Evento actualmente seleccionado
        private DateTime fechaHoraActual;                           // Fecha y hora actual

        public PantallaRevisiones? pantallaRevisiones;              // Referencia a la pantalla de revisión

        public Sesion? sesion;                                      // Sesión actual activa
        private string? usuarioLogueado;                            // Usuario actualmente logueado

        // Filtros seleccionados
        public string? alcanceSeleccionado;
        public string? clasificacionSeleccionado;
        public string? origenGeneracionSeleccionado;

        // Datos de series y muestras
        public List<List<Dictionary<string, string>>> infoMuestrasSismicas = new(); // Muestras por serie
        public List<string> nombresEstaciones = new();                              // Estaciones asociadas a las muestras

        // Atributo para controlar la visualización del mapa
        private bool mostrarMapa;
        // Atributo para controlar si el usuario quiere modificaciones
        private bool permitirModificaciones;

        // Repositorio para manejar la base de datos
        private readonly EventoSismicoRepository _repository;

        // ========================           Constructor              ========================
        public GestorRevisiones()
        {
            _repository = new EventoSismicoRepository();
        }


        // ========================     Métodos principales     ========================
        #region Metodos DE SESION
        // Crea una nueva sesión con el usuario
        public void CrearNuevaSesion(string nombreUsuario, string contraseña)
        {
            Usuario user = CrearNuevoUsuario(nombreUsuario, contraseña);
            sesion = new Sesion(user);

        }

        // Crea un nuevo usuario con nombre y contraseña
        public Usuario CrearNuevoUsuario(string nombre, string contraseña)
        {
            return new Usuario(nombre, contraseña);
        }
        #endregion

        // Carga todos los eventos desde la BD y filtra autodetectados
        public async Task crearNuevaRevision()
        {
            // Asegurar que la BD esté inicializada con datos
            await _repository.InicializarBaseDatosConEventos();
            
            // Cargar TODOS los eventos desde la BD (no solo autodetectados)
            listaEventosOriginal = await _repository.ObtenerTodosLosEventos();
            
            // Ahora filtrar los autodetectados de esa lista completa
            await buscarEventosAutodetectados();
        }

        // Filtra solo los eventos autodetectados desde la listaEventosOriginal
        public async Task buscarEventosAutodetectados()
        {
            var eventosConDatos = new List<(EventoSismico evento, object datos)>();
            
            foreach (var evento in listaEventosOriginal)
            {
                var datosEvento = evento.SosAutoDetectado();
                if (datosEvento != null)
                {
                    eventosConDatos.Add((evento, datosEvento));
                }
            }

            ordenarPorFechaHoraConcurrencia(eventosConDatos);
        }

        public void ordenarPorFechaHoraConcurrencia(List<(EventoSismico evento, object datos)> eventosConDatos)
        {
            // Ordenar la lista completa por fecha de ocurrencia
            var eventosOrdenados = eventosConDatos
                .OrderBy(item => item.evento.GetFechaHoraOcurrencia())
                .ToList();

            // Separar las listas manteniendo el mismo orden
            listaEventosFiltrados = eventosOrdenados.Select(item => item.evento).ToList();// Extrae el EVENTO
            var listaEventosConDatos = eventosOrdenados.Select(item => item.datos).ToList();// Extrae los DATOS

            //•	Lógica de negocio(gestor) → Trabaja con objetos EventoSismico completos
            pantallaRevisiones.SetListaEventosOrdenados(listaEventosFiltrados);

            // Los datos ya están procesados y ordenados, solo los pasamos a la pantalla
            // •	Interfaz de usuario (pantalla) → Recibe datos ya formateados y listos para mostrar
            pantallaRevisiones.mostrarDatosOrdenados(listaEventosConDatos);
        }

        // Selecciona un evento, lo bloquea y carga info asociada
        public async Task tomarEventoSismicoSeleccionado(EventoSismico evento)
        {
            eventoSeleccionado = evento;
            //var estadoBloqueado = buscarBloqueadoEnRevision();

            bloquearEventoSismico(eventoSeleccionado);
            BuscarDatos();
            tomarInfoSeriesYMuestras();

        }

        // ========================     Métodos de estado     ========================

        // Cambia el estado del evento a Bloqueado en Revisión
        public void getFechaHoraActual()
        {
            fechaHoraActual = DateTime.Now;
        }

        public void bloquearEventoSismico(EventoSismico evento)
        {
            getFechaHoraActual(); // Llama al método para establecer la fecha y hora actual
            evento.Revisar(fechaHoraActual); // Usa la fecha y hora actual establecida
            
            // Actualizar estado en la base de datos usando datos únicos del evento
            _ = Task.Run(async () => {
                await _repository.ActualizarEstadoEventoPorDatos(
                    evento.GetFechaHoraOcurrencia(),
                    evento.GetLatitudEpicentro(),
                    evento.GetLongitudEpicentro(),
                    "bloqueadoEnRevision");
            });
        }


        // ========================     Métodos de selección     ========================

        // Guarda el alcance seleccionado del evento
        public string tomarAlcance()
        {
            alcanceSeleccionado = eventoSeleccionado.GetAlcance();
            return alcanceSeleccionado;
        }

        // Guarda la clasificación seleccionada del evento
        public string tomarClasificacion()
        {
            clasificacionSeleccionado = eventoSeleccionado.GetClasificacion();
            return clasificacionSeleccionado;
        }

        // Guarda el origen de generación seleccionado del evento
        public string tomarOrigenDeGeneracion()
        {
            origenGeneracionSeleccionado = eventoSeleccionado.GetOrigenGeneracion();
            return origenGeneracionSeleccionado;
        }

        // Obtiene info de las series temporales y muestras del evento
        public void tomarInfoSeriesYMuestras()
        {
            (infoMuestrasSismicas, nombresEstaciones) = eventoSeleccionado.TomarInfoSeriesYMuestras();
            OrdenarPorEstacionSismologica();
        }
        // =========================================================================================================================================================
        public void OrdenarPorEstacionSismologica() 
        {
            
        }

        // Busca y muestra datos principales del evento
        public void BuscarDatos()
        {
            if (eventoSeleccionado == null) return;

            var datos = new Dictionary<string, string>
            {
                { "Fecha y Hora de Ocurrencia", eventoSeleccionado.GetFechaHoraOcurrencia().ToString("dd/MM/yyyy HH:mm") },
                { "Fecha y Hora de Fin", eventoSeleccionado.GetFechaHoraFin().ToString("dd/MM/yyyy HH:mm") },
                { "Latitud Epicentro", eventoSeleccionado.GetLatitudEpicentro().ToString() },
                { "Longitud Epicentro", eventoSeleccionado.GetLongitudEpicentro().ToString() },
                { "Latitud Hipocentro", eventoSeleccionado.GetLatitudHipocentro().ToString() },
                { "Longitud Hipocentro", eventoSeleccionado.GetLongitudHipocentro().ToString() },
                { "Valor Magnitud", eventoSeleccionado.GetValorMagnitud().ToString() },
                { "Alcance", tomarAlcance() },
                { "Clasificación", tomarClasificacion() },
                { "Origen de Generación", tomarOrigenDeGeneracion() },
                { "Estado Actual", eventoSeleccionado.GetEstadoActual()?.GetNombre() ?? "Sin estado" }
            };

            pantallaRevisiones.mostrarDatosEventoSismico(datos);
        }


        // Devuelve el estado Rechazado
        //public IEstado BuscarEstadoRechazado()
        //{
        //    if (listaEstados == null || listaEstados.Count == 0)
        //        return null;

        //    return listaEstados.FirstOrDefault(e => e.sosRechazado());
        //}

        // Devuelve el nombre del usuario actual
        public string buscarUsuario()
        {
            return sesion.GetUsuario();
        }



        #region Flujo Alternativo

        //                                                  PARA EL FLUJO ALTERNATIVO


        // Confirma el evento sismico seleccionado
        public void ConfirmarEventoSismico()
        {
            
        }



        // Cambia el estado del evento a Derivador
        public void DerivarEventoSismico()
        {
        }

        // Rechaza un evento si cumple con los requisitos
        public String TomarRechazarEvento()
        {
            ValidarExistenDatos();
            ObtenerASLogueado();
            getFechaHoraActual(); // Llama al método para establecer la fecha y hora actual
            RechazarEventoSismico();
            return usuarioLogueado;
        }
        public string ValidarExistenDatos()
        {
            // Validar que existe un evento seleccionado
            if (eventoSeleccionado == null)
            {
                throw new Exception("No hay evento seleccionado para validar.");
            }

            // Validar que exista magnitud
            if (eventoSeleccionado.GetMagnitudRichter() == null)
            {
                throw new Exception("El evento no tiene magnitud Richter asociada.");
            }

            // Validar que exista alcance
            if (string.IsNullOrEmpty(eventoSeleccionado.GetAlcance()))
            {
                throw new Exception("El evento no tiene alcance definido.");
            }

            // Validar que exista origen de generación
            if (string.IsNullOrEmpty(eventoSeleccionado.GetOrigenGeneracion()))
            {
                throw new Exception("El evento no tiene origen de generación definido.");
            }

            // Validar que se haya seleccionado una acción (verificar filtros seleccionados)
            if (string.IsNullOrEmpty(alcanceSeleccionado) && 
                string.IsNullOrEmpty(clasificacionSeleccionado) && 
                string.IsNullOrEmpty(origenGeneracionSeleccionado))
            {
                throw new Exception("Debe seleccionar al menos una acción (alcance, clasificación u origen de generación).");
            }

            // Validar series temporales (código existente)
            if (eventoSeleccionado.GetSeriesTemporales().Count == 0)
            {
                throw new Exception("El evento no tiene series temporales asociadas.");
            }

            foreach (var serie in eventoSeleccionado.GetSeriesTemporales())
            {
                if (serie.GetMuestrasSismicas().Count == 0)
                {
                    throw new Exception("Una de las series temporales no tiene muestras asociadas.");
                }
            }

            return "Datos validados correctamente.";
        }
        public string ObtenerASLogueado()
        {
            usuarioLogueado = sesion.GetUsuario();
            return usuarioLogueado;
        }
        public void RechazarEventoSismico()
        {
            eventoSeleccionado.Rechazar(fechaHoraActual, usuarioLogueado);
            
            // Actualizar estado en la base de datos usando datos únicos del evento
            _ = Task.Run(async () => {
                await _repository.ActualizarEstadoEventoPorDatos(
                    eventoSeleccionado.GetFechaHoraOcurrencia(),
                    eventoSeleccionado.GetLatitudEpicentro(),
                    eventoSeleccionado.GetLongitudEpicentro(),
                    "rechazado");
            });
        }

        #endregion

        // Método para generar y mostrar el sismograma
        public void llamarCuGenerarSismograma()
        {
            // Ruta de la imagen del sismograma (puedes configurar esta ruta según tu proyecto)
            string rutaImagen = @"E:\Apps y Recursos\Nueva carpeta\ProyectoPPAI-v2.0\ProyectoPPAI-v1\FotoSismograma.jpg";
            
            // Validar que existe la imagen
            if (System.IO.File.Exists(rutaImagen))
            {
                // Crear y mostrar la ventana del sismograma
                PantallaSismograma ventanaSismograma = new PantallaSismograma(rutaImagen);
                ventanaSismograma.Show(); // Mostrar la ventana
            }
            else
            {
                // Informar error si no se encuentra la imagen
                MessageBox.Show("No se encontró la imagen del sismograma en la ruta especificada.", 
                               "Error", 
                               MessageBoxButtons.OK, 
                               MessageBoxIcon.Error);
            }
        }

        // Método que recibe el retorno de la pantalla para no mostrar el mapa
        public void tomarNoVerMapa()
        {
            mostrarMapa = false;
        }

        // Método que recibe el retorno de la pantalla para no modificar
        public void tomarOpcionNoModificacion()
        {
            permitirModificaciones = false;
        }
    }
}