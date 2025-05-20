using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoPPAI.Clases
{
    public class TipoDato
    {
        // ========================            Atributos            ========================
        private string nombreUnidadMedida;
        private string denominacion;
        private string valorUmbral;

        // ========================           Constructor              ========================
        public TipoDato(string nombre, string denominacion)
        {
            this.nombreUnidadMedida = nombre;
            this.denominacion = denominacion;
            this.valorUmbral = "0"; // Valor por defecto
        }

        // ========================     Métodos de acceso (getters y setters)      ========================

        #region Getters y Setters
        // Métodos Get
        public string GetNombreUnidadmedida()
        {
            return nombreUnidadMedida;
        }

        public string GetDenominacion()
        {
            return denominacion;
        }
        public string GetValorUmbral()
        {
            return valorUmbral;
        }

        // Métodos Set
        public void SetNombre(string nuevoNombre)
        {
            nombreUnidadMedida = nuevoNombre;
        }
        public void SetDenominacion(string nuevaDenominacion)
        {
            denominacion = nuevaDenominacion;
        }
        public void SetValorUmbral(string nuevoUmbral)
        {
            valorUmbral = nuevoUmbral;
        }
        #endregion

        // ========================       Métodos adicionales        ========================
    }
}