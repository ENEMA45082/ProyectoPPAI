using Microsoft.EntityFrameworkCore;
using ProyectoPPAI.Clases;

namespace ProyectoPPAI.BaseDatos
{
    public class EventoSismicoRepository
    {
        private readonly SismicoContext _context;

        public EventoSismicoRepository()
        {
            _context = new SismicoContext();
            // Asegurar que la base de datos existe
            _context.Database.EnsureCreated();
        }

        // Método para cargar 100 eventos en la BD (reemplaza al GenerarEventosAleatorios)
        public async Task InicializarBaseDatosConEventos()
        {
            // Verificar si ya hay datos
            if (await _context.EventosSismicos.AnyAsync())
            {
                return; // Ya hay datos, no generar más
            }

            // Generar eventos usando la lógica existente
            var eventosGenerados = Generar.GenerarEventosAleatorios(100);

            // Convertir a entidades de BD
            foreach (var evento in eventosGenerados)
            {
                var eventoBD = ConvertirEventoABD(evento);
                _context.EventosSismicos.Add(eventoBD);
            }

            await _context.SaveChangesAsync();
        }

        // Obtener TODOS los eventos desde la BD
        public async Task<List<EventoSismico>> ObtenerTodosLosEventos()
        {
            var eventosBD = await _context.EventosSismicos
                .Include(e => e.SeriesTemporales)
                    .ThenInclude(s => s.Sismografo)
                        .ThenInclude(sm => sm.Estacion)
                .Include(e => e.SeriesTemporales)
                    .ThenInclude(s => s.Muestras)
                        .ThenInclude(m => m.Detalles)
                .ToListAsync(); // SIN filtro, todos los eventos

            // Convertir de BD a entidades del dominio
            var eventos = new List<EventoSismico>();
            foreach (var eventoBD in eventosBD)
            {
                var evento = ConvertirBDaEvento(eventoBD);
                eventos.Add(evento);
            }

            return eventos;
        }

        // Obtener solo eventos autodetectados desde la BD
        public async Task<List<EventoSismico>> ObtenerEventosAutodetectados()
        {
            var eventosBD = await _context.EventosSismicos
                .Include(e => e.SeriesTemporales)
                    .ThenInclude(s => s.Sismografo)
                        .ThenInclude(sm => sm.Estacion)
                .Include(e => e.SeriesTemporales)
                    .ThenInclude(s => s.Muestras)
                        .ThenInclude(m => m.Detalles)
                .Where(e => e.EstadoActual == "autoDetectado")
                .ToListAsync();

            // Convertir de BD a entidades del dominio
            var eventos = new List<EventoSismico>();
            foreach (var eventoBD in eventosBD)
            {
                var evento = ConvertirBDaEvento(eventoBD);
                eventos.Add(evento);
            }

            return eventos;
        }

        // Actualizar estado de un evento en la BD usando datos únicos
        public async Task ActualizarEstadoEventoPorDatos(DateTime fechaOcurrencia, double latitudEpicentro, double longitudEpicentro, string nuevoEstado)
        {
            var evento = await _context.EventosSismicos
                .FirstOrDefaultAsync(e => 
                    e.FechaHoraOcurrencia == fechaOcurrencia &&
                    Math.Abs(e.LatitudEpicentro - latitudEpicentro) < 0.0001 &&
                    Math.Abs(e.LongitudEpicentro - longitudEpicentro) < 0.0001);
                    
            if (evento != null)
            {
                evento.EstadoActual = nuevoEstado;
                await _context.SaveChangesAsync();
            }
        }

        // Actualizar estado de un evento en la BD
        public async Task ActualizarEstadoEvento(int eventoId, string nuevoEstado)
        {
            var evento = await _context.EventosSismicos.FindAsync(eventoId);
            if (evento != null)
            {
                evento.EstadoActual = nuevoEstado;
                await _context.SaveChangesAsync();
            }
        }

        // Convertir EventoSismico a EventoSismicoBD
        private EventoSismicoBD ConvertirEventoABD(EventoSismico evento)
        {
            var eventoBD = new EventoSismicoBD
            {
                FechaHoraOcurrencia = evento.GetFechaHoraOcurrencia(),
                FechaHoraFin = evento.GetFechaHoraFin(),
                LatitudEpicentro = evento.GetLatitudEpicentro(),
                LongitudEpicentro = evento.GetLongitudEpicentro(),
                LatitudHipocentro = evento.GetLatitudHipocentro(),
                LongitudHipocentro = evento.GetLongitudHipocentro(),
                ValorMagnitud = evento.GetValorMagnitud(),
                DescripcionMagnitud = evento.GetMagnitudRichter()?.GetDescripcion() ?? "",
                EstadoActual = evento.GetEstadoActual()?.GetNombre() ?? "",
                Clasificacion = evento.GetClasificacion(),
                OrigenGeneracion = evento.GetOrigenGeneracion(),
                DescripcionOrigen = evento.GetOrigenDeGeneracion()?.GetDescripcion() ?? "",
                Alcance = evento.GetAlcance(),
                DescripcionAlcance = evento.GetAlcanceSismo()?.GetDescripcion() ?? ""
            };

            // Convertir series temporales
            foreach (var serie in evento.GetSeriesTemporales())
            {
                var serieBD = ConvertirSerieABD(serie, eventoBD.Id);
                eventoBD.SeriesTemporales.Add(serieBD);
            }

            return eventoBD;
        }

        private SerieTemporalBD ConvertirSerieABD(SerieTemporal serie, int eventoId)
        {
            var serieBD = new SerieTemporalBD
            {
                CondicionAlarma = serie.GetCondicionAlarma(),
                FechaHoraInicioRegistroMuestras = serie.GetFechaHoraInicioRegistroMuestras(),
                FechaHoraRegistro = serie.GetFechaHoraRegistro(),
                FrecuenciaMuestreo = serie.GetFrecuenciaMuestreo(),
                EventoSismicoId = eventoId
            };

            // Manejar sismografo si existe
            var sismografo = serie.GetSismografo();
            if (sismografo != null)
            {
                var sismografoBD = ConvertirSismografoABD(sismografo);
                serieBD.Sismografo = sismografoBD;
            }

            // Convertir muestras
            foreach (var muestra in serie.GetMuestrasSismicas())
            {
                var muestraBD = ConvertirMuestraABD(muestra, serieBD.Id);
                serieBD.Muestras.Add(muestraBD);
            }

            return serieBD;
        }

        private SismografoBD ConvertirSismografoABD(Sismografo sismografo)
        {
            var estacion = sismografo.GetEstacion();
            var estacionBD = new EstacionSismologicaBD
            {
                Nombre = estacion.GetNombre(),
                Codigo = estacion.GetCodigo()
            };

            return new SismografoBD
            {
                FechaAdquisicion = sismografo.GetFechaAdquisicion(),
                Identificador = sismografo.GetIdentificador(),
                NumeroSerie = sismografo.GetNumeroSerie(),
                Estacion = estacionBD
            };
        }

        private MuestraSismicaBD ConvertirMuestraABD(MuestraSismica muestra, int serieId)
        {
            var muestraBD = new MuestraSismicaBD
            {
                FechaHoraMuestra = muestra.GetFechaHoraMuestra(),
                DetalleMuestra = muestra.GetDetalleDeMuestra(),
                SerieTemporalId = serieId
            };

            // Convertir detalles
            foreach (var detalle in muestra.GetDetalleDeMuestras())
            {
                var detalleBD = new DetalleMuestraSismicaBD
                {
                    Valor = detalle.GetValor(),
                    TipoDato = detalle.GetTipoDato().GetNombre(),
                    DescripcionTipoDato = detalle.GetTipoDato().GetDescripcion(),
                    MuestraId = muestraBD.Id
                };
                muestraBD.Detalles.Add(detalleBD);
            }

            return muestraBD;
        }

        // Convertir EventoSismicoBD a EventoSismico
        private EventoSismico ConvertirBDaEvento(EventoSismicoBD eventoBD)
        {
            // Crear objetos auxiliares
            var magnitudRichter = new MagnitudRichter(eventoBD.ValorMagnitud, eventoBD.DescripcionMagnitud);
            var clasificacion = new ClasificacionSismo(0, 100, eventoBD.Clasificacion); // Valores por defecto
            var origen = new OrigenDeGeneracion(eventoBD.OrigenGeneracion, eventoBD.DescripcionOrigen);
            var alcance = new AlcanceSismo(eventoBD.Alcance, eventoBD.DescripcionAlcance);

            // Crear estado basado en el nombre
            IEstado estado = eventoBD.EstadoActual switch
            {
                "autoDetectado" => new AutoDetectado(),
                "bloqueadoEnRevision" => new BloqueadoEnRevision(),
                "confirmado" => new Confirmado(),
                "rechazado" => new Rechazado(),
                "pendienteRevision" => new PendienteRevision(),
                _ => new AutoDetectado()
            };

            // Crear evento
            var evento = new EventoSismico(
                eventoBD.FechaHoraFin,
                eventoBD.FechaHoraOcurrencia,
                eventoBD.LatitudEpicentro,
                eventoBD.LongitudEpicentro,
                eventoBD.LatitudHipocentro,
                eventoBD.LongitudHipocentro,
                eventoBD.ValorMagnitud,
                estado,
                clasificacion,
                origen,
                alcance,
                magnitudRichter
            );

            // Agregar series temporales
            foreach (var serieBD in eventoBD.SeriesTemporales)
            {
                var serie = ConvertirBDaSerie(serieBD, evento);
                evento.AgregarSerieTemporal(serie);
            }

            return evento;
        }

        private SerieTemporal ConvertirBDaSerie(SerieTemporalBD serieBD, EventoSismico evento)
        {
            var serie = new SerieTemporal();
            serie.SetCondicionAlarma(serieBD.CondicionAlarma);
            serie.SetFechaHoraInicioRegistroMuestras(serieBD.FechaHoraInicioRegistroMuestras);
            serie.SetFechaHoraRegistro(serieBD.FechaHoraRegistro);
            serie.SetFrecuenciaMuestreo(serieBD.FrecuenciaMuestreo);
            serie.SetEventoSismico(evento);

            // Convertir sismografo si existe
            if (serieBD.Sismografo != null)
            {
                var sismografo = ConvertirBDaSismografo(serieBD.Sismografo);
                serie.SetSismografo(sismografo);
            }

            // Convertir muestras
            foreach (var muestraBD in serieBD.Muestras)
            {
                var muestra = ConvertirBDaMuestra(muestraBD);
                serie.AgregarMuestra(muestra);
            }

            return serie;
        }

        private Sismografo ConvertirBDaSismografo(SismografoBD sismografoBD)
        {
            var estacion = new EstacionSismologica(sismografoBD.Estacion.Nombre, sismografoBD.Estacion.Codigo);
            var sismografo = new Sismografo(
                sismografoBD.FechaAdquisicion,
                sismografoBD.Identificador,
                sismografoBD.NumeroSerie
            );
            sismografo.SetEstacion(estacion);
            return sismografo;
        }

        private MuestraSismica ConvertirBDaMuestra(MuestraSismicaBD muestraBD)
        {
            var muestra = new MuestraSismica();
            muestra.SetFechaHoraMuestra(muestraBD.FechaHoraMuestra);
            muestra.SetDetalleDeMuestra(muestraBD.DetalleMuestra);

            // Convertir detalles
            foreach (var detalleBD in muestraBD.Detalles)
            {
                var tipoDato = new TipoDato(detalleBD.TipoDato, detalleBD.DescripcionTipoDato);
                var detalle = new DetalleMuestraSismica(detalleBD.Valor, tipoDato);
                muestra.CrearDetalleMuestra(detalle);
            }

            return muestra;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}