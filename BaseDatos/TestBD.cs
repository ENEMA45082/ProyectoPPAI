using ProyectoPPAI.BaseDatos;
using ProyectoPPAI.Clases;

namespace ProyectoPPAI.BaseDatos
{
    public class TestBD
    {
        public static async Task EjecutarPruebasBasicas()
        {
            Console.WriteLine("?? Iniciando pruebas de la base de datos...\n");

            try
            {
                // Test 1: Inicialización
                Console.WriteLine("1?? Probando inicialización de BD...");
                var resultado = await InicializadorBD.InicializarBaseDatos();
                Console.WriteLine($"   Resultado: {(resultado ? "? EXITOSO" : "? FALLÓ")}\n");

                // Test 2: Consultar eventos autodetectados
                Console.WriteLine("2?? Probando consulta de eventos autodetectados...");
                var cantidad = await InicializadorBD.ObtenerCantidadEventosAutodetectados();
                Console.WriteLine($"   Eventos autodetectados encontrados: {cantidad}\n");

                // Test 3: Usar el repositorio directamente
                Console.WriteLine("3?? Probando repositorio directo...");
                using var repository = new EventoSismicoRepository();
                var eventos = await repository.ObtenerEventosAutodetectados();
                Console.WriteLine($"   Eventos obtenidos del repositorio: {eventos.Count}");
                
                if (eventos.Count > 0)
                {
                    var primerEvento = eventos.First();
                    Console.WriteLine($"   Primer evento - Fecha: {primerEvento.GetFechaHoraOcurrencia()}");
                    Console.WriteLine($"   Primer evento - Magnitud: {primerEvento.GetValorMagnitud()}");
                    Console.WriteLine($"   Primer evento - Estado: {primerEvento.GetEstadoActual()?.GetNombre()}");
                }

                Console.WriteLine("\n? Todas las pruebas completadas exitosamente!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n? Error durante las pruebas: {ex.Message}");
            }
        }
    }
}