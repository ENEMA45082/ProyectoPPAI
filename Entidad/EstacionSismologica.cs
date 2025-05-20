using System;

namespace ProyectoPPAI.Clases
{
    public class EstacionSismologica
    {
        // ========================            Atributos            ========================
        private string nombre;
        private string codigoEstacion;
        private string latitud;
        private string longitud;
        private string documentoCertificacionAdq;
        private string fechaSolicitudCertificacion;
        private int nroCertificacionAdquisicion;

        // ========================           Constructor           ========================
        // Constructor con solo nombre y código
        public EstacionSismologica(string nombre, string codigo)
        {
            this.nombre = nombre;
            this.codigoEstacion = codigo;

            // Podés dejar los demás atributos con valores por defecto
            this.latitud = "";
            this.longitud = "";
            this.documentoCertificacionAdq = "";
            this.fechaSolicitudCertificacion = "";
            this.nroCertificacionAdquisicion = 0;
        }

        public EstacionSismologica(string nombre, string codigo, string latitud, string longitud, string documentoCertificacionAdq, string fechaSolicitudCertificacion, int nroCertificacionAdquisicion)
        {
            this.nombre = nombre;
            this.codigoEstacion = codigo;
            this.latitud = latitud;
            this.longitud = longitud;
            this.documentoCertificacionAdq = documentoCertificacionAdq;
            this.fechaSolicitudCertificacion = fechaSolicitudCertificacion;
            this.nroCertificacionAdquisicion = nroCertificacionAdquisicion;
        }

        // ========================     Métodos de acceso (getters y setters)     ========================
        #region Getters y Setters

        public string GetNombre()
        {
            return nombre;
        }

        public void SetNombre(string nuevoNombre)
        {
            nombre = nuevoNombre;
        }

        public string GetCodigo()
        {
            return codigoEstacion;
        }

        public void SetCodigo(string nuevoCodigo)
        {
            codigoEstacion = nuevoCodigo;
        }

        public string GetLatitud()
        {
            return latitud;
        }

        public void SetLatitud(string nuevaLatitud)
        {
            latitud = nuevaLatitud;
        }

        public string GetLongitud()
        {
            return longitud;
        }

        public void SetLongitud(string nuevaLongitud)
        {
            longitud = nuevaLongitud;
        }

        public string GetDocumentoCertificacionAdq()
        {
            return documentoCertificacionAdq;
        }

        public void SetDocumentoCertificacionAdq(string nuevoDoc)
        {
            documentoCertificacionAdq = nuevoDoc;
        }

        public string GetFechaSolicitudCertificacion()
        {
            return fechaSolicitudCertificacion;
        }

        public void SetFechaSolicitudCertificacion(string nuevaFecha)
        {
            fechaSolicitudCertificacion = nuevaFecha;
        }

        public int GetNroCertificacionAdquisicion()
        {
            return nroCertificacionAdquisicion;
        }

        public void SetNroCertificacionAdquisicion(int nuevoNro)
        {
            nroCertificacionAdquisicion = nuevoNro;
        }

        #endregion
        // ========================     Métodos Adicionales    ========================

    }
}
