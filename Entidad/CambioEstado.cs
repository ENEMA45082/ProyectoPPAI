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
        private Usuario responsable;

        // Relaciones 1 a 1
        private IEstado estado;  

        // ========================           Constructores              ========================
        public CambioEstado() { }

        public CambioEstado(DateTime fechaHoraDesde, IEstado estado, Usuario responsable)
        {
            this.fechaHoraDesde = fechaHoraDesde;
            this.estado = estado;
            this.responsable = responsable;
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

        public IEstado GetEstado()
        {
            return estado;
        }

        // Métodos Set
        public void SetFechaHoraDesde(DateTime fechaHora)
        {
            fechaHoraDesde = fechaHora;
        }

        public void SetFechaHoraFin(DateTime fechaHora)
        {
            fechaHoraFin = fechaHora;
        }

        public void SetEstado(IEstado nuevoEstado)
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