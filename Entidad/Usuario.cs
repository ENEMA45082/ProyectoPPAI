using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoPPAI.Clases
{
    public class Usuario
    {
        // ========================            Atributos            ========================
        private string nombreUsuario;
        private string contraseña;

        // ========================           Constructor              ========================
        public Usuario(string nombre, string contraseña)
        {
            this.nombreUsuario = nombre;
            this.contraseña = contraseña;
        }

        // ========================     Métodos de acceso (getters y setters)      ========================

        #region Getters y Setters
        // Métodos Get
        public string GetNombre()
        {
            return nombreUsuario;
        }

        public string GetContraseña()
        {
            return contraseña;
        }

        // Métodos Set
        public void SetNombre(string nuevoNombre)
        {
            nombreUsuario = nuevoNombre;
        }

        public void SetContraseña(string nuevaContraseña)
        {
            contraseña = nuevaContraseña;
        }
        #endregion

        // ========================       Métodos adicionales        ========================
    }
}
