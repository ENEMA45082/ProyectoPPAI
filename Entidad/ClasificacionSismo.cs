using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoPPAI.Clases
{
    public class ClasificacionSismo
    {
        // ========================            Atributos            ========================
        private double kmProfundidadDesde;
        private double kmProfundidadHasta;
        private string nombre;

        // ========================           Constructor           ========================

        public ClasificacionSismo(double desde, double hasta, string nombre)
        {
            this.kmProfundidadDesde = desde;
            this.kmProfundidadHasta = hasta;
            this.nombre = nombre;
        }

        // ========================     Métodos de acceso (getters y setters)     ========================
        #region Getters y Setters

        // Métodos Get
        public double GetKmProfundidadDesde()
        {
            return kmProfundidadDesde;
        }

        public double GetKmProfundidadHasta()
        {
            return kmProfundidadHasta;
        }

        public string GetNombre()
        {
            return nombre;
        }

        // Métodos Set
        public void SetKmProfundidadDesde(double valor)
        {
            kmProfundidadDesde = valor;
        }

        public void SetKmProfundidadHasta(double valor)
        {
            kmProfundidadHasta = valor;
        }

        public void SetNombre(string nuevoNombre)
        {
            nombre = nuevoNombre;
        }

        #endregion

        // ========================       Métodos adicionales        ========================

    }
}