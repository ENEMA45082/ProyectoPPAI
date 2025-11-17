using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoPPAI.Clases
{
    public class Sesion
    {
        // ========================            Atributos            ========================
        private Usuario usuario;
        private DateTime fechaHoraInicio;
        private DateTime fechaHoraFin;

        // ========================           Constructores              ========================
        public Sesion(Usuario usuario)
        { 
            this.usuario = usuario;
        }
        public Sesion(Usuario usuario, DateTime fechaHoraInicio, DateTime fechaHoraFin)
        {
            this.usuario = usuario;
            this.fechaHoraInicio = fechaHoraInicio;
            this.fechaHoraFin = fechaHoraFin;
        }

        // Constructor alternativo sin fecha fin (por si se inicia la sesión pero no se cerró aún)
        public Sesion(Usuario usuario, DateTime fechaHoraInicio)
        {
            this.usuario = usuario;
            this.fechaHoraInicio = fechaHoraInicio;
            this.fechaHoraFin = DateTime.MinValue; // Valor por defecto que indica que no finalizó
        }

        // ========================     Métodos de acceso (getters y setters)      ========================
        #region Getters y Setters

        // Métodos Get
        public Usuario GetUsuario()
        {
            return usuario;
        }

        public DateTime GetFechaHoraInicio()
        {
            return fechaHoraInicio;
        }

        public DateTime GetFechaHoraFin()
        {
            return fechaHoraFin;
        }

        // Métodos Set
        public void SetUsuario(Usuario nuevoUsuario)
        {
            usuario = nuevoUsuario;
        }

        public void SetFechaHoraInicio(DateTime nuevaFechaInicio)
        {
            fechaHoraInicio = nuevaFechaInicio;
        }

        public void SetFechaHoraFin(DateTime nuevaFechaFin)
        {
            fechaHoraFin = nuevaFechaFin;
        }

        #endregion

        // ========================       Métodos adicionales        ========================
    }
}
