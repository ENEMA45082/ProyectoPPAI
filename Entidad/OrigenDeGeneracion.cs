using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProyectoPPAI.Clases
{
    public class OrigenDeGeneracion
    {
        // ========================            Atributos            ========================
        private string nombre;
        private string descripcion;

        // ========================           Constructor           ========================

        public OrigenDeGeneracion(string nombre, string descripcion)
        {
            this.nombre = nombre;
            this.descripcion = descripcion;
        }

        // ========================     Métodos de acceso (getters y setters)     ========================
        #region Getters y Setters

        // Métodos Get
        public string GetNombre()
        {
            return nombre;
        }

        public string GetDescripcion()
        {
            return descripcion;
        }

        // Métodos Set
        public void SetNombre(string nuevoNombre)
        {
            nombre = nuevoNombre;
        }

        public void SetDescripcion(string nuevaDescripcion)
        {
            descripcion = nuevaDescripcion;
        }

        #endregion

        // ========================       Métodos adicionales        ========================

    }
}