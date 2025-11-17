using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProyectoPPAI.Clases;

namespace ProyectoPPAI
{
    public class Rechazado : IEstado
    {
        public string GetNombre()
        {
            return "rechazado";
        }

        public bool sosAutoDetectado()
        {
            return false;
        }

        public bool sosAmbitoEventoSismico()
        {
            return false;
        }

        public bool sosBloqueadoEnRevision()
        {
            return false;
        }

        public bool sosRechazado()
        {
            return true;
        }

        public bool sosConfirmado()
        {
            return false;
        }

        public bool sosDerivado()
        {
            return false;
        }

        public bool sosPendienteRevision()
        {
            return false;
        }

        public void Revisar(CambioEstado[] es, EventoSismico eventoSismico, DateTime fechaHora)
        {
            // El estado Rechazado es final, no permite más revisiones
            throw new InvalidOperationException("Un evento rechazado no puede ser revisado nuevamente.");
        }

        public void Rechazar(CambioEstado[] es, EventoSismico eventoSismico, DateTime fechaHora, Usuario responsable)
        {
            // El estado Rechazado ya está rechazado, no puede ser rechazado nuevamente
            throw new InvalidOperationException("Un evento ya rechazado no puede ser rechazado nuevamente.");
        }

        public CambioEstado buscarCambioEstadoActual(CambioEstado[] cambios)
        {
            return cambios.FirstOrDefault(c => c.SosActual());
        }

        public IEstado crearEstado()
        {
            return this; // Se mantiene rechazado
        }

        public CambioEstado crearCambioEstado(DateTime fechaHora, IEstado estado, Usuario responsable)
        {
            return new CambioEstado(fechaHora, estado, responsable);
        }
    }
}