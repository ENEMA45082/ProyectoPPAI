using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoPPAI.Pantalla
{
    public partial class PantallaSismograma : Form
    {
        public PantallaSismograma(string rutaImagen)
        {
            InitializeComponent();
            pictureBox1.Image = Image.FromFile(rutaImagen);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom; // Para que escale bien
        }

        private void PantallaSismograma_Load(object sender, EventArgs e)
        {

        }
    }
}
