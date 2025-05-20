using System.Linq;
using ProyectoPPAI.BaseDatos;
using ProyectoPPAI.Pantalla;
using ProyectoPPAI.Clases;

namespace ProyectoPPAI
{
    public class GestorRevisiones
    {
        // ========================            Atributos            ========================
        private List<EventoSismico> listaEventosOriginal = new(); // Todos los eventos generados
        private List<EventoSismico> listaEventosFiltrados = new(); // Solo los eventos autodetectados
        public EventoSismico? eventoSeleccionado; // Evento actualmente seleccionado
        public PantallaRevisiones? pantallaRevisiones; // Referencia a la pantalla de revisión
        public List<Estado> listaEstados = new(); // Lista de estados posibles
        public Sesion? sesion; // Sesión actual activa
        private GenerarEstados generadorEstados; // Generador de estados

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

        // Filtra solo los eventos autodetectados
        public void buscarEventosAutodetectados()
        {
            foreach (var evento in listaEventosOriginal)
            {
                if (evento.SosAutoDetectado())
                    listaEventosFiltrados.Add(evento);
            }

            ordenarPorFechaHoraConcurrencia();
        }
        public void ordenarPorFechaHoraConcurrencia()
        {
            listaEventosFiltrados = listaEventosFiltrados
                .OrderBy(ev => ev.GetFechaHoraOcurrencia())
                .ToList();

            pantallaRevisiones.listaEventosOrdenados = listaEventosFiltrados; // PASA LA LISTA ORIGINAL
            var listaAnonima = listaEventosFiltrados.Select(ev => new
            {
                FechaHoraOcurrencia = ev.GetFechaHoraOcurrencia().ToString("dd/MM/yyyy HH:mm"),
                LatitudHipocentro = ev.GetLatitudHipocentro(),
                LongitudHipocentro = ev.GetLongitudHipocentro(),
                LatitudEpicentro = ev.GetLatitudEpicentro(),
                LongitudEpicentro = ev.GetLongitudEpicentro(),
                MagnitudRichter = ev.GetValorMagnitud(),
            }).ToList();

            pantallaRevisiones.mostrarDatosOrdenados(listaAnonima);
        }

        // Selecciona un evento, lo bloquea y carga info asociada
        public async Task tomarEventoSismicoSeleccionado(EventoSismico evento)
        {
            eventoSeleccionado = evento;
            var estadoBloqueado = buscarBloqueadoEnRevision();
            bloquearEventoSismico(eventoSeleccionado, estadoBloqueado);
            BuscarDatos();
            tomarInfoSeriesYMuestras();

        }

        // ========================     Métodos de estado     ========================

        // Devuelve el estado Bloqueado en Revisión
        public Estado buscarBloqueadoEnRevision()
        {
            if (listaEstados == null || listaEstados.Count == 0)
                return null;

            return listaEstados.FirstOrDefault(e => e.SosBloqueadoEnRevision());
        }

        // Cambia el estado del evento a Bloqueado en Revisión
        public void bloquearEventoSismico(EventoSismico evento, Estado estado)
        {
            evento.Revisar(estado);
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
        public Estado BuscarEstadoRechazado()
        {
            if (listaEstados == null || listaEstados.Count == 0)
                return null;

            return listaEstados.FirstOrDefault(e => e.SosRechazado());
        }

        // Devuelve el nombre del usuario actual
        public string buscarUsuario()
        {
            return sesion.GetUsuario();
        }



        #region Flujo Alternativo

        //                                                  PARA EL FLUJO ALTERNATIVO
        // Devuelve el estado Confirmado
        public Estado buscarConfirmado()
        {
            if (listaEstados == null || listaEstados.Count == 0)
                return null;

            return listaEstados.FirstOrDefault(e => e.SosConfirmado());
        }

        // Confirma el evento sismico seleccionado
        public void ConfirmarEventoSismico()
        {
            Estado estado = buscarConfirmado();
            eventoSeleccionado.Confirmar(estado);
        }

        // Devuelve el estado Derivado
        public Estado buscarDerivado()
        {
            if (listaEstados == null || listaEstados.Count == 0)
                return null;

            return listaEstados.FirstOrDefault(e => e.SosDerivado());
        }

        // Cambia el estado del evento a Derivado
        public void DerivarEventoSismico()
        {
            Estado estado = buscarDerivado();
            eventoSeleccionado.Derivar(estado);
        }

        // Rechaza un evento si cumple con los requisitos
        public String TomarRechazarEvento()
        {
            // ACA VALIDAMOS QUE EXISTAN DATOS PARA RECHAZAR EL EVENTO
            Estado estado = BuscarEstadoRechazado();
            if (eventoSeleccionado.GetMagnitudRichter() != null
                && eventoSeleccionado.GetAlcance() != null
                && eventoSeleccionado.GetOrigenGeneracion() != null)
            {
                eventoSeleccionado.Rechazar(estado);
            }
            return sesion.GetUsuario();
        }

        #endregion

    }
}