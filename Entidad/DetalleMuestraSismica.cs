using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoPPAI.Clases
{

   public class DetalleMuestraSismica
    {
        // ========================            Atributos            ========================
        private double valorMedido;
        // Relaciones 1 a 1
        private TipoDato tipoDato;

        // ========================           Constructor              ========================
        public DetalleMuestraSismica(double valorMedido, TipoDato tipoDato)
        {
            this.valorMedido = valorMedido;
            this.tipoDato = tipoDato;
        }

        // ========================     Métodos de acceso (getters y setters)      ========================

        #region Getters y Setters
        // Métodos Get
        public double GetValorMedido()
        {
            return valorMedido;
        }

        public TipoDato GetTipoDato()
        {
            return tipoDato;
        }

        // Métodos Set
        public void SetValorMedido(double nuevoValor)
        {
            valorMedido = nuevoValor;
        }

        public void SetTipoDato(TipoDato nuevoTipoDato)
        {
            tipoDato = nuevoTipoDato;
        }
        #endregion

        // ========================       Métodos adicionales        ========================

        public Dictionary<string, string> GetDatos()
        {
            return new Dictionary<string, string>
            {
                { "Denominacion", tipoDato.GetDenominacion() },
                { "Valor", valorMedido.ToString() },
                { "Unidad", tipoDato.GetNombreUnidadmedida() }

            };
        }
    }
}