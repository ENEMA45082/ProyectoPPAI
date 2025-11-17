using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProyectoPPAI.Clases;

namespace ProyectoPPAI
{
    public class Confirmado : IEstado
    {
        public string GetNombre()
        {
            return "confirmado";
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
            return true;
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
            // El estado Confirmado es final, no permite más revisiones
            throw new InvalidOperationException("Un evento confirmado no puede ser revisado nuevamente.");
        }

        public void Rechazar(CambioEstado[] es, EventoSismico eventoSismico, DateTime fechaHora, String responsable)
        {
            // El estado Confirmado es final, no puede ser rechazado
            throw new InvalidOperationException("Un evento confirmado no puede ser rechazado.");
        }

        public CambioEstado buscarCambioEstadoActual(CambioEstado[] cambios)
        {
            return cambios.FirstOrDefault(c => c.SosActual());
        }

        public IEstado crearEstado()
        {
            return this; // Se mantiene confirmado
        }

        public CambioEstado crearCambioEstado(DateTime fechaHora, IEstado estado, String responsable)
        {
            return new CambioEstado(fechaHora, estado, responsable);
        }
    }
}