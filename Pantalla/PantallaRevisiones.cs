using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProyectoPPAI.BaseDatos;
using ProyectoPPAI.Clases;
using System.Drawing;
using System.Drawing.Drawing2D;


namespace ProyectoPPAI.Pantalla
{
    public partial class PantallaRevisiones : Form
    {
        // Atributos
        private GestorRevisiones gestorRevisiones;
        public List<EventoSismico> listaEventosOrdenados;
        private EventoSismico eventoSeleccionado;


        // Constructor
        public PantallaRevisiones()
        {
            InitializeComponent();
            gestorRevisiones = new GestorRevisiones();
            gestorRevisiones.pantallaRevisiones = this; // Asignamos la referencia para callbacks
        }

        // Mostrar la ventana
        public void habilitarPantalla()
        {
            this.Show();
        }

        // Método para iniciar la carga y generación de eventos
        public async void OpcionMostrarEventos()
        {
            habilitarPantalla();
            await gestorRevisiones.crearNuevaRevision();
        }
        // Doble click en un evento para seleccionarlo y actualizar su estado
        private async void TomarEventoSeleccionado(object sender, DataGridViewCellEventArgs e)
        {
            dataGridViewSeleccionado.Visible = true;
            labelTitulo.Visible = false;
            labelVisualizarMapa.Visible = true;
            checkBoxMostrarDatos.Visible = true;
            button1.Visible = true;
            comboBox1.Visible = true;
            label1.Visible = true;
            dataGridViewSerie.Visible = true;
            dataGridViewEventos.Visible = false;
            btnDerivarAExperto.Visible = true;
            button2.Visible = true;
            RedondearDataGridView(dataGridViewSeleccionado, 8); // Radio de 20 píxeles
            RedondearDataGridView(dataGridViewSerie, 8); // Radio de 20 píxeles
            if (e.RowIndex >= 0) // Evitar clic en encabezado
            {
                eventoSeleccionado = listaEventosOrdenados[e.RowIndex];
                await gestorRevisiones.tomarEventoSismicoSeleccionado(eventoSeleccionado);
                mostrarSeriesYMuestras(gestorRevisiones.infoMuestras, gestorRevisiones.nombresEstaciones); // muestra las series en el nuevo DataGridView

                MessageBox.Show(
                    $"✅ El estado del sismo fue cambiado con éxito.\n\n🌍 Estado actual: {eventoSeleccionado.GetEstadoActual().GetNombre()}",
                    "✅ Estado Actual",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            // Ruta absoluta de la imagen
            string rutaImagen = @"E:\Apps y Recursos\Nueva carpeta\ProyectoPPAI-v2.0\ProyectoPPAI-v1\FotoSismograma.jpg"; // Cambiala por tu ruta real

            if (System.IO.File.Exists(rutaImagen))
            {
                PantallaSismograma ventana = new PantallaSismograma(rutaImagen);
                ventana.Show(); // o ShowDialog() si querés que sea modal
            }
            else
            {
                MessageBox.Show("No se encontró la imagen en la ruta especificada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Mostrar lista de eventos en el DataGridView principal
        public void mostrarDatosOrdenados(object listaEventos)
        {
            if (listaEventos == null) return;

            dataGridViewEventos.DataSource = listaEventos;
            dataGridViewEventos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewEventos.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string usuario = gestorRevisiones.TomarRechazarEvento();
            MessageBox.Show(
                $"El evento sísmico fue rechazado con éxito.\nEstado actual: {eventoSeleccionado.GetEstadoActual().GetNombre()}\n Modificado por: {usuario}",
                $"Estado Actual\n Rechazado por: {usuario}",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }






        // Mostrar datos detallados del evento seleccionado en la grilla secundaria
        public void mostrarDatosEventoSismico(Dictionary<string, string> diccionario)
        {
            DataTable tabla = new DataTable();

            // Agregar columnas
            foreach (var clave in diccionario.Keys)
            {
                tabla.Columns.Add(clave);
            }
            // Agregar fila con valores
            var fila = tabla.NewRow();
            foreach (var kvp in diccionario)
            {
                fila[kvp.Key] = kvp.Value;
            }
            tabla.Rows.Add(fila);

            dataGridViewSeleccionado.DataSource = tabla;
            dataGridViewSeleccionado.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewSeleccionado.Refresh();
        }
        public void mostrarSeriesYMuestras( List<List<Dictionary<string, string>>> infoMuestras, List<string> nombresEstaciones)
        {
            if (infoMuestras == null || infoMuestras.Count == 0)
            {
                MessageBox.Show("No hay series temporales o muestras para mostrar.", "Atención");
                return;
            }

            DataTable tabla = new DataTable();

            // Agregar columnas fijas
            tabla.Columns.Add("Estación");

            // Recolectar claves únicas excepto FechaHoraMuestra (ya la agregamos)
            HashSet<string> clavesUnicas = new HashSet<string>();
            foreach (var serie in infoMuestras)
            {
                foreach (var muestra in serie)
                {
                    foreach (var kvp in muestra)
                    {
                        if (kvp.Key != "FechaHoraMuestra")
                            clavesUnicas.Add(kvp.Key);
                    }
                }
            }

            // Agregar las columnas restantes
            foreach (var clave in clavesUnicas)
            {
                tabla.Columns.Add(clave);
            }

            // Asociar estaciones y ordenar alfabéticamente
            var estacionesYMuestras = infoMuestras
                .Select((serie, index) => new { Estacion = nombresEstaciones[index], Serie = serie })
                .OrderBy(e => e.Estacion)
                .ToList();

            foreach (var item in estacionesYMuestras)
            {
                var serie = item.Serie;
                string nombreEstacion = item.Estacion;

                foreach (var muestra in serie)
                {
                    var fila = tabla.NewRow();
                    fila["Estación"] = nombreEstacion;

                    foreach (var kvp in muestra)
                    {
                        if (kvp.Key == "FechaHoraMuestra")
                        {
                            fila["FechaHoraMuestra"] = kvp.Value; // Asignamos explícitamente
                        }
                        else if (tabla.Columns.Contains(kvp.Key))
                        {
                            fila[kvp.Key] = kvp.Value;
                        }
                    }

                    tabla.Rows.Add(fila);
                }
            }

            dataGridViewSerie.DataSource = tabla;
            dataGridViewSerie.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewSerie.Refresh();
        }




        #region PARA FLUJO ALTERNATIVO

        private void button2_Click(object sender, EventArgs e)
        {
            gestorRevisiones.ConfirmarEventoSismico();
            string usuario = gestorRevisiones.buscarUsuario();
            MessageBox.Show(
                $"El evento sísmico fue modificado con éxito.\nEstado actual: {eventoSeleccionado.GetEstadoActual().GetNombre()}\n Modificado por: {usuario}",
                $"Estado Actual\n Modificado por: {usuario}",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void btnDerivarAExperto_Click(object sender, EventArgs e)
        {
            gestorRevisiones.DerivarEventoSismico();
            string usuario = gestorRevisiones.buscarUsuario();
            MessageBox.Show(
                $"El evento sísmico fue modificado con éxito.\nEstado actual: {eventoSeleccionado.GetEstadoActual().GetNombre()}\n Modificado por: {usuario}",
                $"Estado Actual\n Modificado por: {usuario}",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }
        #endregion

        private void checkBoxMostrarDatos_Click(object sender, EventArgs e){ }

        private void labelVisualizarMapa_Click(object sender, EventArgs e){ }

        private void finCU(object sender, EventArgs e)
        {
            Application.Exit();
        }

        #region Metodos adicionales
        public void CrearNuevaSesion(string nombreUsuario, string contraseñaIngresada)
        {             // Crear una nueva sesión
            gestorRevisiones.CrearNuevaSesion(nombreUsuario, contraseñaIngresada);
            // Aquí puedes agregar lógica para manejar la nueva sesión
        }
        #endregion

        #region diseño de la ventana
        private void RedondearDataGridView(DataGridView dgv, int radio)
        {
            GraphicsPath path = new GraphicsPath();
            Rectangle bounds = new Rectangle(0, 0, dgv.Width, dgv.Height);
            int d = radio * 2;

            path.StartFigure();
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();

            dgv.Region = new Region(path);
        }
        private void PantallaRevisiones_Load(object sender, EventArgs e)
        {
            panelFondo.SendToBack();
            dataGridViewSeleccionado.Visible = false;
            label1.Visible = false;
            dataGridViewSerie.Visible = false;
            labelVisualizarMapa.Visible = false;
            comboBox1.Visible = false;
            checkBoxMostrarDatos.Visible = false;
            button1.Visible = false;
            btnDerivarAExperto.Visible = false;
            button2.Visible = false;
            RedondearDataGridView(dataGridViewEventos, 8); // Radio de 20 píxeles
            dataGridViewEventos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewSeleccionado.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            this.BackColor = Color.FromArgb(30, 30, 47); // Fondo general

            // Estilo para botones
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Button btn)
                {
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.BackColor = Color.FromArgb(78, 154, 241); // Azul fachero
                    btn.ForeColor = Color.White;
                    btn.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                    btn.FlatAppearance.BorderColor = Color.FromArgb(30, 30, 47);
                    btn.FlatAppearance.BorderSize = 1;
                    btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(100, 180, 255);
                }

                if (ctrl is Label lbl)
                {
                    lbl.ForeColor = Color.LightGray;
                    lbl.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                }

                if (ctrl is ComboBox combo)
                {
                    combo.BackColor = Color.White;
                    combo.ForeColor = Color.Black;
                    combo.Font = new Font("Segoe UI", 10, FontStyle.Regular);
                }

                if (ctrl is CheckBox check)
                {
                    check.ForeColor = Color.LightGray;
                    check.Font = new Font("Segoe UI", 10, FontStyle.Regular);
                }
            }

            // Estilizar todos los DataGridView
            EstilizarDataGridView(dataGridViewEventos);
            EstilizarDataGridView(dataGridViewSerie);
            EstilizarDataGridView(dataGridViewSeleccionado);
        }

        private void EstilizarDataGridView(DataGridView dgv)
        {
            dgv.BackgroundColor = Color.FromArgb(45, 45, 58);
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.GridColor = Color.FromArgb(70, 70, 90);

            dgv.DefaultCellStyle.BackColor = Color.FromArgb(59, 59, 74);
            dgv.DefaultCellStyle.ForeColor = Color.White;
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(78, 154, 241);
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 10);

            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(50, 50, 65);

            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(35, 35, 50);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
        }
        private void dataGridViewSerie_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void dataGridViewSeleccionado_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void panel5_Paint(object sender, PaintEventArgs e) { }
        private void panel3_Paint_1(object sender, PaintEventArgs e) { }
        #endregion
    }
}