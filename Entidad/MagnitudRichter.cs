using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProyectoPPAI.Clases
{
    public class MagnitudRichter
    {
        // ========================            Atributos            ========================
        private double numero;
        private string descripcionMagnitud;

        // ========================           Constructor           ========================

        public MagnitudRichter(double numero, string descripcion)
        {
            this.numero = numero;
            this.descripcionMagnitud = descripcion;
        }

        // ========================     Métodos de acceso (getters y setters)     ========================
        #region Getters y Setters

        // Métodos Get
        public double GetNumero()
        {
            return numero;
        }

        public string GetDescripcionMagnitud()
        {
            return descripcionMagnitud;
        }

        // Métodos Set
        public void SetNumero(double nuevoNumero)
        {
            numero = nuevoNumero;
        }

        public void SetDescripcionMagnitud(string nuevaDescripcion)
        {
            descripcionMagnitud = nuevaDescripcion;
        }

        #endregion

        // ========================       Métodos adicionales        ========================

    }
}
