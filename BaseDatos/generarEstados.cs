using System;
using System.Collections.Generic;
using ProyectoPPAI;
using ProyectoPPAI.Clases;

namespace ProyectoPPAI.BaseDatos
{
    public class GenerarEstados
    {
        public IEstado autoDetectado = new AutoDetectado();
        public IEstado bloqueado = new Derivado();
        public IEstado rechazado = new Rechazado();
                public IEstado pendienteRevision = new PendienteRevision();
        public IEstado confirmado = new Confirmado();
        public IEstado bloqueadoEnRevision = new BloqueadoEnRevision();
        
        // Lista para guardar todos los estados
        public List<IEstado> listaEstados;

        public GenerarEstados()
        {
                listaEstados = new List<IEstado>
            {
                autoDetectado,
                bloqueado,
                rechazado,
                pendienteRevision,
                confirmado,
                bloqueadoEnRevision
            };
        }
    }
}