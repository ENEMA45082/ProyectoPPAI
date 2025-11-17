using Microsoft.EntityFrameworkCore;
using ProyectoPPAI.Clases;
using System;
using System.Diagnostics;

namespace ProyectoPPAI.BaseDatos
{
    public class EventoSismicoRepository : IDisposable
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
            try
            {
                // Asegurar que la base de datos existe y crear las tablas
                bool created = await _context.Database.EnsureCreatedAsync();
                Console.WriteLine($"Base de datos creada: {created}");

                // Verificar si ya hay datos
                var count = await _context.EventosSismicos.CountAsync();
                Console.WriteLine($"Eventos existentes en BD: {count}");

                if (count > 0)
                {
                    Console.WriteLine("Ya hay datos en la base de datos, no se generarán más eventos.");
                    return; // Ya hay datos, no generar más
                }

                Console.WriteLine("Generando 100 eventos sísmicos...");
                // Generar eventos usando la lógica existente
                var eventosGenerados = Generar.GenerarEventosAleatorios(100);
                Console.WriteLine($"Eventos generados: {eventosGenerados.Count}");

                // Convertir a entidades de BD
                foreach (var evento in eventosGenerados)
                {
                    var eventoBD = ConvertirEventoABD(evento);
                    _context.EventosSismicos.Add(eventoBD);
                }

                Console.WriteLine("Guardando eventos en la base de datos...");
                int saved = await _context.SaveChangesAsync();
                Console.WriteLine($"Eventos guardados exitosamente: {saved}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al inicializar base de datos: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                throw;
            }
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
        // Método completo para cambiar estado con persistencia y tracking por ID
        public async Task CambiarEstadoEventoPorId(int eventoId, string estadoAnterior, string nuevoEstado, Usuario usuario = null)
        {
            var evento = await _context.EventosSismicos
                .Include(e => e.CambiosEstado)
                .FirstOrDefaultAsync(e => e.Id == eventoId);
                    
            if (evento != null)
            {
                Console.WriteLine($"CAMBIO ESTADO: Evento ID {evento.Id}: '{estadoAnterior}' → '{nuevoEstado}'");
                
                // Crear registro de cambio de estado
                var cambioEstado = new CambioEstadoBD
                {
                    FechaHoraCambio = DateTime.Now,
                    EstadoAnterior = estadoAnterior,
                    EstadoNuevo = nuevoEstado,
                    UsuarioModificacion = usuario?.GetNombre() ?? "Sistema", // Corregido para obtener el nombre del usuario
                    EventoSismicoId = evento.Id
                };
                
                // Actualizar estado actual
                evento.EstadoActual = nuevoEstado;
                
                // Agregar cambio al historial
                evento.CambiosEstado.Add(cambioEstado);
                
                var changes = await _context.SaveChangesAsync();
                Console.WriteLine($"PERSISTENCIA: {changes} cambios guardados en BD");
            }
            else
            {
                Console.WriteLine($"ERROR: No se encontró evento con ID {eventoId} para cambiar estado");
            }
        }

        // Método completo para cambiar estado con persistencia y tracking
        public async Task CambiarEstadoEvento(DateTime fechaOcurrencia, double latitudEpicentro, double longitudEpicentro, string nuevoEstado, string? usuario = null)
        {
            var evento = await _context.EventosSismicos
                .Include(e => e.CambiosEstado)
                .FirstOrDefaultAsync(e => 
                    e.FechaHoraOcurrencia == fechaOcurrencia &&
                    Math.Abs(e.LatitudEpicentro - latitudEpicentro) < 0.0001 &&
                    Math.Abs(e.LongitudEpicentro - longitudEpicentro) < 0.0001);
                    
            if (evento != null)
            {
                string estadoAnterior = evento.EstadoActual;
                Console.WriteLine($"CAMBIO ESTADO: Evento ID {evento.Id}: '{estadoAnterior}' → '{nuevoEstado}'");
                
                // Crear registro de cambio de estado
                var cambioEstado = new CambioEstadoBD
                {
                    FechaHoraCambio = DateTime.Now,
                    EstadoAnterior = estadoAnterior,
                    EstadoNuevo = nuevoEstado,
                    UsuarioModificacion = usuario ?? "Sistema",
                    EventoSismicoId = evento.Id
                };
                
                // Actualizar estado actual
                evento.EstadoActual = nuevoEstado;
                
                // Agregar cambio al historial
                evento.CambiosEstado.Add(cambioEstado);
                
                var changes = await _context.SaveChangesAsync();
                Console.WriteLine($"PERSISTENCIA: {changes} cambios guardados en BD");
            }
            else
            {
                Console.WriteLine($"ERROR: No se encontró evento para cambiar estado");
            }
        }

        // Actualizar estado de un evento en la BD usando datos únicos
        public async Task ActualizarEstadoEventoPorDatos(DateTime fechaOcurrencia, double latitudEpicentro, double longitudEpicentro, string nuevoEstado)
        {
            await CambiarEstadoEvento(fechaOcurrencia, latitudEpicentro, longitudEpicentro, nuevoEstado);
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

        // Agregar este método para actualizar por ID
        public async Task ActualizarEstadoEventoPorId(int eventoId, string nuevoEstado)
        {
            var evento = await _context.EventosSismicos.FindAsync(eventoId);
            if (evento != null)
            {
                Debug.WriteLine($"ENCONTRADO - Evento ID {evento.Id}: Estado actual '{evento.EstadoActual}'");
                Debug.WriteLine($"ACTUALIZANDO a: '{nuevoEstado}'");
                
                evento.EstadoActual = nuevoEstado;
                
                try 
                {
                    var changes = await _context.SaveChangesAsync();
                    Debug.WriteLine($"✅ SUCCESS: {changes} cambios guardados en BD");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"❌ ERROR al guardar: {ex.Message}");
                }
            }
            else
            {
                Debug.WriteLine($"❌ No se encontró evento con ID: {eventoId}");
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
                DescripcionMagnitud = evento.GetMagnitudRichter()?.GetDescripcionMagnitud() ?? "",
                EstadoActual = evento.GetEstadoActual()?.GetNombre() ?? "",
                Clasificacion = evento.GetClasificacion(),
                OrigenGeneracion = evento.GetOrigenGeneracion() ?? "",
                DescripcionOrigen = ObtenerDescripcionOrigen(evento),
                Alcance = evento.GetAlcance() ?? "",
                DescripcionAlcance = ObtenerDescripcionAlcance(evento)
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

            // Manejar sismografo si existe - primero guardarlo para obtener el ID
            var sismografo = serie.GetSismografo();
            if (sismografo != null)
            {
                var sismografoBD = ConvertirSismografoABD(sismografo);
                serieBD.Sismografo = sismografoBD;
                // El SismografoId se asignará automáticamente por EF cuando se guarde
            }

            // Convertir muestras
            foreach (var muestra in serie.GetMuestrasSismicas())
            {
                var muestraBD = ConvertirMuestraABD(muestra, 0); // Pasamos 0, se asignará automáticamente
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
                Identificador = sismografo.GetIdentificadorSimografo(),
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
            foreach (var detalle in muestra.GetDetalles())
            {
                var detalleBD = new DetalleMuestraSismicaBD
                {
                    Valor = detalle.GetValorMedido(),
                    TipoDato = detalle.GetTipoDato().GetNombreUnidadmedida(),
                    DescripcionTipoDato = detalle.GetTipoDato().GetDenominacion(),
                    // MuestraId se asignará automáticamente por EF
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
                "PendienteRevision" => new PendienteRevision(),
                "derivado" => new Derivado(),
                _ => throw new ArgumentException($"Estado desconocido en BD: '{eventoBD.EstadoActual}'")
            };

            Debug.WriteLine($"DEBUG: Estado en BD: '{eventoBD.EstadoActual}'");

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

            // *** AGREGAR ESTA LÍNEA ***
            evento.SetId(eventoBD.Id);

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

        // Métodos auxiliares para obtener descripciones
        private string ObtenerDescripcionOrigen(EventoSismico evento)
        {
            return evento.GetOrigenDeGeneracionCompleto()?.GetDescripcion() ?? "";
        }

        private string ObtenerDescripcionAlcance(EventoSismico evento)
        {
            return evento.GetAlcanceCompleto()?.GetDescripcion() ?? "";
        }

        // Método de prueba para verificar conectividad y actualización
        public async Task<bool> PruebaActualizacionBD()
        {
            try
            {
                // Obtener el primer evento autodetectado
                var primerEvento = await _context.EventosSismicos
                    .Where(e => e.EstadoActual == "autoDetectado")
                    .FirstOrDefaultAsync();
                
                if (primerEvento != null)
                {
                    Console.WriteLine($"PRUEBA: Evento encontrado ID {primerEvento.Id}, Estado: {primerEvento.EstadoActual}");
                    
                    // Cambiar temporalmente a "TEST"
                    primerEvento.EstadoActual = "TEST";
                    var changes = await _context.SaveChangesAsync();
                    Console.WriteLine($"PRUEBA: {changes} cambios guardados");
                    
                    // Volver al estado original
                    primerEvento.EstadoActual = "autoDetectado";
                    await _context.SaveChangesAsync();
                    Console.WriteLine($"PRUEBA: Estado restaurado");
                    
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PRUEBA ERROR: {ex.Message}");
                return false;
            }
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}