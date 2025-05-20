using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoPPAI
{
    public class Estado
    {
        // ========================            Atributos            ========================
        private string nombre;
        private string descripcion;

        // ========================           Constructores         ========================
        public Estado(string nombre, string descripcion)
        {
            this.nombre = nombre;
            this.descripcion = descripcion;
        }

        // ========================     Métodos de acceso (getters y setters)     ========================
        #region Getters y Setters

        public string GetNombre()
        {
            return nombre;
        }

        public void SetNombre(string nuevoNombre)
        {
            nombre = nuevoNombre;
        }

        public string GetDescripcion()
        {
            return descripcion;
        }

        public void SetDescripcion(string nuevaDescripcion)
        {
            descripcion = nuevaDescripcion;
        }

        #endregion

        // ========================       Métodos adicionales        ========================

        public bool SosAutoDetectado()
        {
            return nombre == "autoDetectado";
        }

        public bool SosBloqueadoEnRevision()
        {
            return nombre == "bloqueadoEnRevision";
        }

        public bool SosRechazado()
        {
            return nombre == "rechazado";
        }

        public bool SosConfirmado()
        {
            return nombre == "confirmado";
        }

        public bool SosDerivado()
        {
            return nombre == "derivado";
        }
    }
}