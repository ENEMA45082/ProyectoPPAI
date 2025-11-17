using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProyectoPPAI.Clases;

namespace ProyectoPPAI.Clases
{
    public class EventoSismico
    {
        // ========================            Atributos            ========================
        private int id; // Campo para el ID de la base de datos
        private DateTime fechaHoraFin;
        private DateTime fechaHoraOcurrencia;
        private double latitudEpicentro;
        private double latitudHipocentro;
        private double longitudEpicentro;
        private double longitudHipocentro;
        private double valorMagnitud;
        

        // ========================            Relaciones 1:1            ========================
        private IEstado estadoActual;
        private ClasificacionSismo clasificacion;
        private OrigenDeGeneracion origen;
        private AlcanceSismo alcance;
        private MagnitudRichter magnitudRichter;

        // ========================           Relaciones 1:N            ========================
        private List<SerieTemporal> seriesTemporales = new List<SerieTemporal>();
        private List<CambioEstado> cambiosDeEstado = new List<CambioEstado>();

        // ========================           Constructores           ========================
        public EventoSismico() { }

        public EventoSismico(DateTime fechaHoraFin, DateTime fechaHoraOcurrencia,
                             double latEpic, double latHipo,
                             double lonEpic, double lonHipo,
                             double valorMagnitud,
                             IEstado estadoActual,
                             ClasificacionSismo clasificacion,
                             OrigenDeGeneracion origen,
                             AlcanceSismo alcance,
                             MagnitudRichter magnitudRichter)
        {
            this.fechaHoraFin = fechaHoraFin;
            this.fechaHoraOcurrencia = fechaHoraOcurrencia;
            this.latitudEpicentro = latEpic;
            this.latitudHipocentro = latHipo;
            this.longitudEpicentro = lonEpic;
            this.longitudHipocentro = lonHipo;
            this.valorMagnitud = valorMagnitud;
            this.estadoActual = estadoActual;
            this.clasificacion = clasificacion;
            this.origen = origen;
            this.alcance = alcance;
            this.magnitudRichter = magnitudRichter;
        }

        // ========================     Métodos de acceso (getters y setters)      ========================
        #region Getters y Setters

        // Métodos para el ID
        public int GetId() => id;
        public void SetId(int nuevoId) => id = nuevoId;

        public DateTime GetFechaHoraFin() => fechaHoraFin;
        public DateTime GetFechaHoraOcurrencia() => fechaHoraOcurrencia;
        public double GetLatitudEpicentro() => latitudEpicentro;
        public double GetLatitudHipocentro() => latitudHipocentro;
        public double GetLongitudEpicentro() => longitudEpicentro;
        public double GetLongitudHipocentro() => longitudHipocentro;
        public double GetValorMagnitud() => valorMagnitud;

        public IEstado GetEstadoActual() => estadoActual;
        public void SetEstadoActual(IEstado estado) => estadoActual = estado;
        public MagnitudRichter GetMagnitudRichter() => magnitudRichter;

        public string GetClasificacion() => clasificacion?.GetNombre();
        public string GetOrigenGeneracion() => origen?.GetNombre();
        public string GetAlcance() => alcance?.GetNombre();

        // Métodos para obtener los objetos completos
        public OrigenDeGeneracion GetOrigenDeGeneracionCompleto() => origen;
        public AlcanceSismo GetAlcanceCompleto() => alcance;

        public List<SerieTemporal> GetSeriesTemporales() => seriesTemporales;
        public List<CambioEstado> GetCambiosDeEstado() => cambiosDeEstado;

        public void SetCambiosDeEstado(List<CambioEstado> cambios)
        {
            cambiosDeEstado = cambios;
            if (cambios != null && cambios.Count > 0)
                estadoActual = cambios.Last().GetEstado();
        }

        #endregion

        // ========================       Métodos adicionales        ========================
        #region Métodos Públicos

        public void AgregarSerieTemporal(SerieTemporal serie)
        {
            if (serie != null && !seriesTemporales.Contains(serie))
            {
                seriesTemporales.Add(serie);
            }
        }

        public void AgregarCambioEstado(CambioEstado cambio)
        {
            cambiosDeEstado.Add(cambio);
            estadoActual = cambio.GetEstado();
        }
        // aca empieza
        public object SosAutoDetectado()
        {
            if (estadoActual != null && estadoActual.sosAutoDetectado())
            {
                return new
                {
                    FechaHoraOcurrencia = GetFechaHoraOcurrencia().ToString("dd/MM/yyyy HH:mm"),
                    LatitudHipocentro = GetLatitudHipocentro(),
                    LongitudHipocentro = GetLongitudHipocentro(),
                    LatitudEpicentro = GetLatitudEpicentro(),
                    LongitudEpicentro = GetLongitudEpicentro(),
                    MagnitudRichter = GetValorMagnitud(),
                    Evento = this
                };
            }
            return null;
        }

        public (List<List<Dictionary<string, string>>>, List<string>) TomarInfoSeriesYMuestras()
        {
            List<List<Dictionary<string, string>>> muestras = new();
            List<string> nombresEstaciones = new();

            foreach (var serie in seriesTemporales)
            {
                muestras.Add(serie.GetValoresMuestras());
                nombresEstaciones.Add(serie.ObtenerNombreEstacionSismilogica());
            }

            return (muestras, nombresEstaciones);
        }

        public void Rechazar(DateTime fechaHora,Usuario analista)
        {
            CambioEstado[] cambiosArray = cambiosDeEstado.ToArray();
            estadoActual.Rechazar(cambiosArray, this, fechaHora, analista);
        }

        private void CambiarEstadoActual(IEstado nuevoEstado)
        {
            foreach (var cambio in cambiosDeEstado)
            {
                if (cambio.SosActual())
                {
                    //cambio.SetFechaHoraFin();
                    var nuevoCambio = new CambioEstado(DateTime.Now, nuevoEstado, null);
                    cambiosDeEstado.Add(nuevoCambio);
                    estadoActual = nuevoEstado;
                    break;
                }
            }
        }

        // PARA FLUJOS ALTERNATIVOS

        public void Revisar(DateTime fechaHoraActual)
        {
            CambioEstado[] cambiosArray = cambiosDeEstado.ToArray();
            estadoActual.Revisar(cambiosArray, this, fechaHoraActual);
        }

        public void Confirmar(IEstado nuevoEstado)
        {
            CambiarEstadoActual(nuevoEstado);
        }

        public void Derivar(IEstado nuevoEstado)
        {
            CambiarEstadoActual(nuevoEstado);
        }
        #endregion
    }
}
