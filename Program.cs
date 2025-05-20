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
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new PantallaInicio()); // VENTANA INICIAL DEL SISTEMA
        }
        
    }
}           