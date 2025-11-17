using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProyectoPPAI.Clases;

namespace ProyectoPPAI
{
    public class BloqueadoEnRevision : IEstado
    {
        public string GetNombre()
        {
            return "bloqueadoEnRevision";
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
            return true;
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
            // El estado BloqueadoEnRevision no cambia de estado durante la revisión
            // Solo valida que está en proceso de revisión
        }


        public void Rechazar(CambioEstado[] cambios, EventoSismico eventoSismico, DateTime fechaHora, String analista)
        {
            CambioEstado cambioActual = buscarCambioEstadoActual(cambios);
            cambioActual.SetFechaHoraFin(fechaHora);

            IEstado estadoRechazado = crearEstado();

            CambioEstado nuevoCambio = crearCambioEstado(fechaHora, estadoRechazado, analista);

            eventoSismico.AgregarCambioEstado(nuevoCambio);
            eventoSismico.SetEstadoActual(estadoRechazado);
        }

        public CambioEstado buscarCambioEstadoActual(CambioEstado[] cambios)
        {
            return cambios.FirstOrDefault(c => c.SosActual());
        }

        public IEstado crearEstado()
        {
            return new Rechazado();
        }

        public CambioEstado crearCambioEstado(DateTime fechaHora, IEstado estado, String analista)
        {
            return new CambioEstado(fechaHora, estado, analista);
        }
    }
}