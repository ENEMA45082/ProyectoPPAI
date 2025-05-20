using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProyectoPPAI.Clases;

namespace ProyectoPPAI.Clases
{
    public class Sismografo
    {
        // ========================            Atributos            ========================
        private DateTime fechaAdquisicion;
        private string identificadorSimografo;
        private string numeroSerie;
        private EstacionSismologica estacion;
        private List<SerieTemporal> seriesTemporales;

        // ========================           Constructor              ========================
        public Sismografo(DateTime fechaAdquisicion, string identificadorSimografo, string numeroSerie)
        {
            this.fechaAdquisicion = fechaAdquisicion;
            this.identificadorSimografo = identificadorSimografo;
            this.numeroSerie = numeroSerie;
            this.seriesTemporales = new List<SerieTemporal>();
        }

        // ========================     Métodos de acceso (getters y setters)      ========================
        #region Getters y Setters

        // Fecha de adquisición
        public DateTime GetFechaAdquisicion()
        {
            return fechaAdquisicion;
        }

        public void SetFechaAdquisicion(DateTime nuevaFecha)
        {
            fechaAdquisicion = nuevaFecha;
        }

        // Identificador del simógrafo
        public string GetIdentificadorSimografo()
        {
            return identificadorSimografo;
        }

        public void SetIdentificadorSimografo(string nuevoIdentificador)
        {
            identificadorSimografo = nuevoIdentificador;
        }

        // Número de serie
        public string GetNumeroSerie()
        {
            return numeroSerie;
        }

        public void SetNumeroSerie(string nuevoNumeroSerie)
        {
            numeroSerie = nuevoNumeroSerie;
        }

        // Estación sismológica (1 a 1)
        public EstacionSismologica GetEstacion()
        {
            return estacion;
        }

        public void SetEstacion(EstacionSismologica nuevaEstacion)
        {
            estacion = nuevaEstacion;
        }

        // Series temporales
        public List<SerieTemporal> GetSeriesTemporales()
        {
            return seriesTemporales;
        }

        public void SetSeriesTemporales(List<SerieTemporal> nuevasSeries)
        {
            seriesTemporales = nuevasSeries;
        }

        #endregion

        // ========================       Métodos adicionales        ========================

        // Asociar una nueva serie temporal
        public void AgregarSerieTemporal(SerieTemporal serie)
        {
            if (serie != null)
            {
                seriesTemporales.Add(serie);
            }
        }

        // Obtener los datos del simógrafo y su estación
        public Dictionary<string, string> GetDatos()
        {
            var datos = new Dictionary<string, string>
            {
                { "Fecha de Adquisición", fechaAdquisicion.ToString("dd/MM/yyyy") },
                { "Identificador", identificadorSimografo },
                { "Número de Serie", numeroSerie }
            };

            if (estacion != null)
            {
                datos.Add("Nombre Estación", estacion.GetNombre());
                datos.Add("Código Estación", estacion.GetCodigo());
            }

            return datos;
        }

        public string GetNombreEstacionSismologica()
        {
            return estacion.GetNombre();
        }
    }
}