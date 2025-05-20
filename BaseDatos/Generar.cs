using System;
using System.Collections.Generic;
using ProyectoPPAI;
using ProyectoPPAI.Clases;
using ProyectoPPAI.BaseDatos; // Asegúrate de tener este using

namespace ProyectoPPAI.BaseDatos
{
    public class Generar
    {
        public static List<EventoSismico> GenerarEventosAleatorios(int cantidad)
        {
            var rand = new Random();

            var tiposDatos = new List<TipoDato>
            {
                new TipoDato( "km/seg", "Velocidad de onda"),
                new TipoDato("Hz", "Frecuencia de onda"),
                new TipoDato("km/ciclo", "Longitud")
            };

            var generadorEstados = new GenerarEstados();

            var listaEstados = new List<Estado>
            {
                generadorEstados.autoDetectado,
                generadorEstados.bloqueado,
                generadorEstados.rechazado,
                generadorEstados.pendienteRevision,
                generadorEstados.confirmado
            };

            var clasificaciones = new List<ClasificacionSismo>
            {
                new ClasificacionSismo(0, 70, "Superficial"),
                new ClasificacionSismo(70, 300, "Intermedio"),
                new ClasificacionSismo(300, 700, "Profundo")
            };

            var origenes = new List<OrigenDeGeneracion>
            {
                new OrigenDeGeneracion("Tectónico", "Movimiento de placas tectónicas"),
                new OrigenDeGeneracion("Volcánico", "Actividad volcánica"),
                new OrigenDeGeneracion("Colapso", "Colapso de cavidades subterráneas")
            };

            var alcances = new List<AlcanceSismo>
            {
                new AlcanceSismo("Local", "Percibido solo en la zona del epicentro"),
                new AlcanceSismo("Regional", "Percibido en varias provincias"),
                new AlcanceSismo("Global", "Registrado en estaciones de todo el mundo")
            };

            // Lista de estaciones sismológicas (objetos EstacionSismologica)
            var estaciones = new List<EstacionSismologica>
            {
                new EstacionSismologica("Estación Norte", "N001"),
                new EstacionSismologica("Estación Centro", "C001"),
                new EstacionSismologica("Estación Sur", "S001")
            };

            var listaEventos = new List<EventoSismico>();

            for (int i = 0; i < cantidad; i++)
            {
                var estado = listaEstados[rand.Next(listaEstados.Count)];
                
                double magnitudNum = Math.Round(rand.NextDouble() * 8 + 1, 1);

                if (estado.GetNombre() == "autoDetectado")
                {
                    magnitudNum = Math.Round(rand.NextDouble() * 3 + 1, 1);
                }
                

                var clasificacion = clasificaciones[rand.Next(clasificaciones.Count)];
                var origen = origenes[rand.Next(origenes.Count)];
                var alcance = alcances[rand.Next(alcances.Count)];
                string descripcionMag = magnitudNum switch
                {
                    <= 3.9 => "Menor",
                    <= 5.9 => "Moderado",
                    <= 6.9 => "Fuerte",
                    <= 7.9 => "Mayor",
                    _ => "Extremo"
                };
                var magnitudRichter = new MagnitudRichter(magnitudNum, descripcionMag);

                DateTime fechaOcurrencia = DateTime.Now.AddMinutes(-rand.Next(2000));
                DateTime fechaFin = fechaOcurrencia.AddMinutes(rand.Next(1000));

                var evento = new EventoSismico(
                    fechaFin,
                    fechaOcurrencia,
                    Math.Round(rand.NextDouble() * 180 - 90, 4),
                    Math.Round(rand.NextDouble() * 180 - 90, 4),
                    Math.Round(rand.NextDouble() * 360 - 180, 4),
                    Math.Round(rand.NextDouble() * 360 - 180, 4),
                    magnitudNum,

                    estado,
                    clasificacion,
                    origen,
                    alcance,
                    magnitudRichter
                );

                var cambioEstado = new CambioEstado(fechaOcurrencia, estado);
                evento.SetCambiosDeEstado(new List<CambioEstado> { cambioEstado });

                int cantidadSeries = 4;

                for (int s = 0; s < cantidadSeries; s++)
                {
                    var serieTemporal = new SerieTemporal();
                    serieTemporal.SetCondicionAlarma($"Alarma serie #{s + 1}");
                    serieTemporal.SetFechaHoraInicioRegistroMuestras(DateTime.Now.AddMinutes(-30 * (s + 1)));
                    serieTemporal.SetFechaHoraRegistro(DateTime.Now.AddMinutes(-20 * (s + 1)));
                    serieTemporal.SetFrecuenciaMuestreo(100.0 + s * 10);
                    serieTemporal.SetEventoSismico(evento);

                    int cantidadMuestras = rand.Next(5, 9);

                    for (int j = 0; j < cantidadMuestras; j++)
                    {
                        var muestra = new MuestraSismica();
                        muestra.SetFechaHoraMuestra(serieTemporal.GetFechaHoraInicioRegistroMuestras().AddHours(j));

                        muestra.SetDetalleDeMuestra($"Detalle muestra #{j + 1} (Serie {s + 1})");

                        foreach (var tipo in tiposDatos)
                        {
                            var valor = Math.Round(rand.NextDouble() * 10, 2);
                            var detalle = new DetalleMuestraSismica(valor, tipo);
                            muestra.CrearDetalleMuestra(detalle);
                        }


                        serieTemporal.AgregarMuestra(muestra);
                    }

                    // Crear Sismografo
                    DateTime fechaAdq = DateTime.Now.AddYears(-rand.Next(1, 10)); // fecha adquisición aleatoria en los últimos 10 años
                    string identificador = $"Sismo-{rand.Next(1000, 9999)}";
                    string numeroSerie = $"NS-{rand.Next(10000, 99999)}";

                    var estacionAsignada = estaciones[rand.Next(estaciones.Count)];

                    var sismografo = new Sismografo(fechaAdq, identificador, numeroSerie);
                    sismografo.SetEstacion(estacionAsignada);

                    // Asociar la serie temporal al sismografo y viceversa
                    sismografo.AgregarSerieTemporal(serieTemporal);
                    serieTemporal.SetSismografo(sismografo);

                    evento.AgregarSerieTemporal(serieTemporal);
                }

                listaEventos.Add(evento);
            }

            return listaEventos;
        }
    }
}