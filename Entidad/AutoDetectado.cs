using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProyectoPPAI.Clases;

namespace ProyectoPPAI
{
    public class AutoDetectado : IEstado
    {
        public string GetNombre()
        {
            return "autoDetectado";
        }

        public bool sosAutoDetectado()
        {
            return true;
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
            return false;
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
            IEstado nuevoEstado = crearEstado();
            
            // Crear nuevo cambio de estado
            var nuevoCambio = crearCambioEstado(fechaHora, nuevoEstado, null);
            
            // Agregar el nuevo cambio al evento sísmico
            eventoSismico.AgregarCambioEstado(nuevoCambio);
            eventoSismico.SetEstadoActual(nuevoEstado);
        }

        public void Rechazar(CambioEstado[] es, EventoSismico eventoSismico, DateTime fechaHora, String responsable)
        {
            // El estado AutoDetectado no puede ser rechazado directamente
            throw new InvalidOperationException("Un evento en estado AutoDetectado no puede ser rechazado sin ser revisado primero.");
        }

        public CambioEstado buscarCambioEstadoActual(CambioEstado[] cambios)
        {
            return cambios.FirstOrDefault(c => c.SosActual());
        }

        public IEstado crearEstado()
        {
            return new BloqueadoEnRevision();
        }

        public CambioEstado crearCambioEstado(DateTime fechaHora, IEstado estado, String responsable)
        {
            return new CambioEstado(fechaHora, estado, responsable);
        }
    }
}