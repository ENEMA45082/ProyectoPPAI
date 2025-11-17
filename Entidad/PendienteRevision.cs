using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProyectoPPAI.Clases;

namespace ProyectoPPAI
{
    public class PendienteRevision : IEstado
    {
        public string GetNombre()
        {
            return "PendienteRevision";
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
            return false;
        }

        public bool sosPendienteRevision()
        {
            return true;
        }

        public void Revisar(CambioEstado[] es, EventoSismico eventoSismico, DateTime fechaHora)
        {
            // Buscar el cambio de estado actual
            var cambioActual = buscarCambioEstadoActual(es);
            if (cambioActual != null)
            {
                // Establecer fecha hora fin del cambio actual
                cambioActual.SetFechaHoraFin(fechaHora);
            }
            
            // Crear nuevo estado (BloqueadoEnRevision)
            var nuevoEstado = crearEstado();
            
            // Crear nuevo cambio de estado
            var nuevoCambio = crearCambioEstado(fechaHora, nuevoEstado, null);
            
            // Agregar el nuevo cambio al evento sísmico
            eventoSismico.AgregarCambioEstado(nuevoCambio);
        }

        public void Rechazar(CambioEstado[] es, EventoSismico eventoSismico, DateTime fechaHora, Usuario responsable)
        {
            // El estado PendienteRevision no puede ser rechazado directamente
            throw new InvalidOperationException("Un evento en estado PendienteRevision debe ser revisado antes de ser rechazado.");
        }

        public CambioEstado buscarCambioEstadoActual(CambioEstado[] cambios)
        {
            return cambios.FirstOrDefault(c => c.SosActual());
        }

        public IEstado crearEstado()
        {
            return new BloqueadoEnRevision();
        }

        public CambioEstado crearCambioEstado(DateTime fechaHora, IEstado estado, Usuario responsable)
        {
            return new CambioEstado(fechaHora, estado, responsable);
        }
    }
}