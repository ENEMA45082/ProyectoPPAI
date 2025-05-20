using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProyectoPPAI.Clases;

namespace ProyectoPPAI.Clases
{
    public class CambioEstado
    {
        // ========================            Atributos            ========================
        private DateTime fechaHoraDesde;
        private DateTime? fechaHoraFin;  // Null porque puede estar sin defeinir

        // Relaciones 1 a 1
        private Estado estado;  

        // ========================           Constructores              ========================
        public CambioEstado() { }

        public CambioEstado(DateTime fechaHoraDesde, Estado estado)
        {
            this.fechaHoraDesde = fechaHoraDesde;
            this.estado = estado;
            this.fechaHoraFin = null; // Por defecto, no tiene fin
        }

        // ========================     Métodos de acceso (getters y setters)      ========================
        #region Getters y Setters

        // Métodos Get
        public DateTime GetFechaHoraDesde()
        {
            return fechaHoraDesde;
        }

        public DateTime? GetFechaHoraFin()
        {
            return fechaHoraFin;
        }

        public Estado GetEstado()
        {
            return estado;
        }

        // Métodos Set
        public void SetFechaHoraDesde()
        {
            fechaHoraDesde = DateTime.Now;
        }

        public void SetFechaHoraFin()
        {
            fechaHoraFin = DateTime.Now;
        }

        public void SetEstado(Estado nuevoEstado)
        {
            estado = nuevoEstado;
        }

        #endregion

        // ========================       Métodos adicionales        ========================
        public bool SosActual()
        {
            return fechaHoraFin == null;
        }
    }
}