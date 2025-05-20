using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoPPAI.Clases
{
    public class SerieTemporal
    {
        // ========================            Atributos            ========================
        private DateTime fechaHoraInicioRegistroMuestras;
        private DateTime fechaHoraRegistro;
        private string condicionAlarma;
        private double frecuenciaMuestreo;

        // Relación de agregación 1 a muchos con MuestraSismica
        private List<MuestraSismica> muestrasSismicas = new List<MuestraSismica>();

        // Relación de muchos a uno con EventoSismico
        private EventoSismico eventoSismico;

        // Relación 1 a 1 con Sismografo
        private Sismografo sismografo;

        // ========================           Constructores              ========================
        public SerieTemporal(string condicionAlarma, DateTime fechaInicio, DateTime fechaRegistro, double frecuencia, Sismografo sismografo)
        {
            this.condicionAlarma = condicionAlarma;
            this.fechaHoraInicioRegistroMuestras = fechaInicio;
            this.fechaHoraRegistro = fechaRegistro;
            this.frecuenciaMuestreo = frecuencia;
            this.sismografo = sismografo;
        }

        public SerieTemporal()
        {
            // Constructor vacío para flexibilidad
        }

        // ========================     Métodos de acceso (getters y setters)      ========================
        #region Getters y Setters

        public string GetCondicionAlarma()
        {
            return condicionAlarma;
        }

        public DateTime GetFechaHoraInicioRegistroMuestras()
        {
            return fechaHoraInicioRegistroMuestras;
        }

        public DateTime GetFechaHoraRegistro()
        {
            return fechaHoraRegistro;
        }

        public double GetFrecuenciaMuestreo()
        {
            return frecuenciaMuestreo;
        }

        public List<MuestraSismica> GetMuestrasSismicas()
        {
            return muestrasSismicas;
        }

        public EventoSismico GetEventoSismico()
        {
            return eventoSismico;
        }

        public Sismografo GetSismografo()
        {
            return sismografo;
        }

        public void SetCondicionAlarma(string nuevaCondicion)
        {
            condicionAlarma = nuevaCondicion;
        }

        public void SetFechaHoraInicioRegistroMuestras(DateTime nuevaFechaInicio)
        {
            fechaHoraInicioRegistroMuestras = nuevaFechaInicio;
        }

        public void SetFechaHoraRegistro(DateTime nuevaFechaRegistro)
        {
            fechaHoraRegistro = nuevaFechaRegistro;
        }

        public void SetFrecuenciaMuestreo(double nuevaFrecuencia)
        {
            frecuenciaMuestreo = nuevaFrecuencia;
        }

        public void SetEventoSismico(EventoSismico nuevoEvento)
        {
            eventoSismico = nuevoEvento;
        }

        public void SetSismografo(Sismografo nuevoSismografo)
        {
            sismografo = nuevoSismografo;
        }

        #endregion

        // ========================       Métodos adicionales        ========================

        // Agrega una muestra a la lista si no existe ya
        public void AgregarMuestra(MuestraSismica muestra)
        {
            if (muestra != null && !muestrasSismicas.Contains(muestra))
            {
                muestrasSismicas.Add(muestra);
            }
        }

        // Devuelve una lista de diccionarios con los datos de cada muestra
        public List<Dictionary<string, string>> GetValoresMuestras()
        {
            var lista = new List<Dictionary<string, string>>();

            foreach (var muestra in muestrasSismicas)
            {
                var datosDeMuestra = muestra.GetDatos();
                lista.AddRange(datosDeMuestra);
            }

            return lista;
        }

        // Obtiene el nombre de la estación sismológica asociada al sismógrafo
        public string ObtenerNombreEstacionSismilogica()
        {
            return sismografo.GetNombreEstacionSismologica();
        }
    }
}
