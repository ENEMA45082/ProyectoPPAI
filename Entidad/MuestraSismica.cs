using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoPPAI.Clases
{
    public class MuestraSismica
    {
        // ========================            Atributos            ========================
        private DateTime fechaHoraMuestra;
        private string detalleDeMuestra;

        // Relación de agregación 1 a muchas
        private List<DetalleMuestraSismica> detalles;

        // ========================           Constructor              ========================
        public MuestraSismica(DateTime fechaHoraMuestra, string detalleDeMuestra)
        {
            this.fechaHoraMuestra = fechaHoraMuestra;
            this.detalleDeMuestra = detalleDeMuestra;
            this.detalles = new List<DetalleMuestraSismica>();
        }

        public MuestraSismica()
        {
            this.detalles = new List<DetalleMuestraSismica>();
        }

        // ========================     Métodos de acceso (getters y setters)      ========================

        #region Getters y Setters

        public DateTime GetFechaHoraMuestra()
        {
            return fechaHoraMuestra;
        }

        public void SetFechaHoraMuestra(DateTime nuevaFechaHora)
        {
            fechaHoraMuestra = nuevaFechaHora;
        }

        public string GetDetalleDeMuestra()
        {
            return detalleDeMuestra;
        }

        public void SetDetalleDeMuestra(string nuevoDetalle)
        {
            detalleDeMuestra = nuevoDetalle;
        }

        public List<DetalleMuestraSismica> GetDetalles()
        {
            return detalles;
        }

        public void SetDetalles(List<DetalleMuestraSismica> nuevosDetalles)
        {
            detalles = nuevosDetalles ?? new List<DetalleMuestraSismica>();
        }

        #endregion

        // ========================       Métodos adicionales        ========================

        public void CrearDetalleMuestra(DetalleMuestraSismica detalle)
        {
            if (detalle != null && !detalles.Contains(detalle))
                detalles.Add(detalle);
        }


        public List<Dictionary<string, string>> GetDatos()
        {
            var datos = new List<Dictionary<string, string>>();
            foreach (var detalle in detalles)
            {
                var info = detalle.GetDatos();

                // Agregamos la fecha/hora de la muestra (puede formatearse si querés solo la hora, por ejemplo)
                info["fechaHoraMuestra"] = fechaHoraMuestra.ToString("g"); // "g" = fecha y hora corta

                datos.Add(info);
            }
            return datos;
        }
    }
}