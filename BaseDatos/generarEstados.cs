using System;
using System.Collections.Generic;
using ProyectoPPAI;
using ProyectoPPAI.Clases;

namespace ProyectoPPAI.BaseDatos
{
    public class GenerarEstados
    {
        public Estado autoDetectado = new Estado("autoDetectado", "El evento fue registrado automáticamente");
        public Estado bloqueado = new Estado("derivado", "El evento fue derivado manualmente");
        public Estado rechazado = new Estado("rechazado", "El evento fue registrado manualmente");
        public Estado pendienteRevision = new Estado("PendienteRevision", "El evento fue registrado manualmente y está pendiente de revisión");
        public Estado confirmado = new Estado("confirmado", "El evento fue registrado manualmente y ha sido confirmado por un operador");
        public Estado bloqueadoEnRevision = new Estado("bloqueadoEnRevision", "El evento está siendo revisado");
        
        // ?? Lista para guardar todos los estados
        public List<Estado> listaEstados;

        public GenerarEstados()
        {
                listaEstados = new List<Estado>
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