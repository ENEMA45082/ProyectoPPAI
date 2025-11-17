using ProyectoPPAI.BaseDatos;
using ProyectoPPAI.Clases;
using ProyectoPPAI.Pantalla;
using System.Diagnostics;
using System.Linq;

namespace ProyectoPPAI
{
    public class GestorRevisiones
    {
        // ========================            Atributos            ========================
        private List<EventoSismico> listaEventosOriginal = new(); // Todos los eventos generados
        private List<EventoSismico> listaEventosFiltrados = new(); // Solo los eventos autodetectados
        public EventoSismico? eventoSeleccionado; // Evento actualmente seleccionado
        public PantallaRevisiones? pantallaRevisiones; // Referencia a la pantalla de revisión
        public List<IEstado> listaEstados = new(); // Lista de estados posibles
        public Sesion? sesion; // Sesión actual activa
        private GenerarEstados generadorEstados; // Generador de estados
        private DateTime fechaHoraActual; // Fecha y hora actual para operaciones
        private string? usuarioLogueado; // Usuario actualmente logueado

        // Filtros seleccionados
        public string? alcanceSeleccionado;
        public string? clasificacionSeleccionado;
        public string? origenGeneracionSeleccionado;

        // Datos de series y muestras
        public List<List<Dictionary<string, string>>> infoMuestras = new(); // Muestras por serie
        public List<string> nombresEstaciones = new(); // Estaciones asociadas a las muestras

        // ========================           Constructor              ========================

        // Constructor vacío: inicializa generador de estados
        public GestorRevisiones()
        {
            generadorEstados = new GenerarEstados();
        }

        // ========================     Métodos principales     ========================
        #region Metodos Adicionales
        // Crea una nueva sesión con el usuario
        public void CrearNuevaSesion(string nombreUsuario, string contraseña)
        {
            Usuario user = CrearNuevoUsuario(nombreUsuario, contraseña);
            sesion = new Sesion(user);

            var generar = new GenerarEstados();
            listaEstados = generar.listaEstados;
        }

        // Crea un nuevo usuario con nombre y contraseña
        public Usuario CrearNuevoUsuario(string nombre, string contraseña)
        {
            return new Usuario(nombre, contraseña);
        }
        #endregion

        // Genera 100 eventos aleatorios y filtra autodetectados
        public async Task crearNuevaRevision()
        {
            listaEventosOriginal = Generar.GenerarEventosAleatorios(100);
            buscarEventosAutodetectados();
        }

        // Filtra solo los eventos autodetectados y guarda sus datos
        public void buscarEventosAutodetectados()
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
            listaEventosFiltrados = eventosOrdenados.Select(item => item.evento).ToList();
            var listaEventosConDatos = eventosOrdenados.Select(item => item.datos).ToList();

            pantallaRevisiones.SetListaEventosOrdenados(listaEventosFiltrados);
            
            // Los datos ya están procesados y ordenados, solo los pasamos a la pantalla
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

        // Devuelve el estado Bloqueado en Revisión
        public IEstado buscarBloqueadoEnRevision()
        {
            if (listaEstados == null || listaEstados.Count == 0)
                return null;

            return listaEstados.FirstOrDefault(e => e.sosBloqueadoEnRevision());
        }

        // Cambia el estado del evento a Bloqueado en Revisión
        public void getFechaHoraActual()
        {
            fechaHoraActual = DateTime.Now;
        }

        public void bloquearEventoSismico(EventoSismico evento)
        {
            getFechaHoraActual(); // Llama al método para establecer la fecha y hora actual
            evento.Revisar(fechaHoraActual); // Usa la fecha y hora actual establecida
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
            (infoMuestras, nombresEstaciones) = eventoSeleccionado.TomarInfoSeriesYMuestras();
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
        // Devuelve el estado Confirmado
        public IEstado buscarConfirmado()
        {
            if (listaEstados == null || listaEstados.Count == 0)
                return null;

            return listaEstados.FirstOrDefault(e => e.sosConfirmado());
        }

        // Confirma el evento sismico seleccionado
        public void ConfirmarEventoSismico()
        {
            IEstado estado = buscarConfirmado();
            eventoSeleccionado.Confirmar(estado);
        }

        // Devuelve el estado Derivado
        public IEstado buscarDerivado()
        {
            if (listaEstados == null || listaEstados.Count == 0)
                return null;

            return listaEstados.FirstOrDefault(e => e.sosDerivado());
        }

        // Cambia el estado del evento a Derivador
        public void DerivarEventoSismico()
        {
            IEstado estado = buscarDerivado();
            eventoSeleccionado.Derivar(estado);
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
        }

        #endregion

    }
}