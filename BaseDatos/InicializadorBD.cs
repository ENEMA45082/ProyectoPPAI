using ProyectoPPAI.BaseDatos;
using System;
using System.IO;

namespace ProyectoPPAI
{
    public class InicializadorBD
    {
        public static async Task<bool> InicializarBaseDatos()
        {
            try
            {
                // Crear directorio si no existe
                var directorioBase = Path.Combine(Directory.GetCurrentDirectory(), "BaseDatos");
                if (!Directory.Exists(directorioBase))
                {
                    Directory.CreateDirectory(directorioBase);
                }

                // Crear repositorio e inicializar BD
                using var repository = new EventoSismicoRepository();
                await repository.InicializarBaseDatosConEventos();

                Console.WriteLine("? Base de datos inicializada exitosamente con 100 eventos sísmicos");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"? Error al inicializar base de datos: {ex.Message}");
                return false;
            }
        }

        public static async Task<int> ObtenerCantidadEventosAutodetectados()
        {
            try
            {
                using var repository = new EventoSismicoRepository();
                var eventos = await repository.ObtenerEventosAutodetectados();
                return eventos.Count;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"? Error al consultar eventos: {ex.Message}");
                return -1;
            }
        }
    }
}