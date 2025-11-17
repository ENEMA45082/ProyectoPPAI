using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProyectoPPAI.Clases;

namespace ProyectoPPAI
{
    public class Derivado : IEstado
    {
        public string GetNombre()
        {
            return "derivado";
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
            return false;
        }

        public bool sosConfirmado()
        {
            return false;
        }

        public bool sosDerivado()
        {
            return true;
        }

        public bool sosPendienteRevision()
        {
            return false;
        }

        public void Revisar(CambioEstado[] es, EventoSismico eventoSismico, DateTime fechaHora)
        {
            // El estado Derivado es final, no permite más revisiones del sistema
            throw new InvalidOperationException("Un evento derivado a experto no puede ser revisado por el sistema.");
        }

        public void Rechazar(CambioEstado[] es, EventoSismico eventoSismico, DateTime fechaHora, Usuario responsable)
        {
            // El estado Derivado es final, no puede ser rechazado por el sistema
            throw new InvalidOperationException("Un evento derivado a experto no puede ser rechazado por el sistema.");
        }

        public CambioEstado buscarCambioEstadoActual(CambioEstado[] cambios)
        {
            return cambios.FirstOrDefault(c => c.SosActual());
        }

        public IEstado crearEstado()
        {
            return this; // Se mantiene derivado
        }

        public CambioEstado crearCambioEstado(DateTime fechaHora, IEstado estado, Usuario responsable)
        {
            return new CambioEstado(fechaHora, estado, responsable);
        }
    }
}