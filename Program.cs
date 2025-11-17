using ProyectoPPAI.Pantalla;

namespace ProyectoPPAI
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        ///  punto de entrada de la aplicacion
        /// </summary>
        [STAThread]
        static async Task Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            
            // Inicializar base de datos antes de mostrar la aplicación
            await InicializadorBD.InicializarBaseDatos();
            
            Application.Run(new PantallaInicio()); // VENTANA INICIAL DEL SISTEMA
        }
        
    }
}           