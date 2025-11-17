using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProyectoPPAI.Clases;

namespace ProyectoPPAI
{
    public interface IEstado
    {
        string GetNombre();
        bool sosAutoDetectado();
        bool sosAmbitoEventoSismico();
        bool sosBloqueadoEnRevision();
        bool sosRechazado();
        bool sosConfirmado();
        bool sosDerivado();
        bool sosPendienteRevision();
        void Revisar(CambioEstado[] es, EventoSismico eventoSismico, DateTime fechaHora);
        void Rechazar(CambioEstado[] es, EventoSismico eventoSismico, DateTime fechaHora, String responsable);
        CambioEstado buscarCambioEstadoActual(CambioEstado[] cambios);
        IEstado crearEstado();
        CambioEstado crearCambioEstado(DateTime fechaHora, IEstado estado, String responsable);
    }
}