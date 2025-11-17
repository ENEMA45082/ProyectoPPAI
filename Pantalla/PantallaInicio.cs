using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProyectoPPAI.Clases;

namespace ProyectoPPAI.Pantalla
{
    public partial class PantallaInicio : Form
    {
        private Label lblBienvenida;
        private Panel panelContenedor;
        private PantallaRevisiones pantallaRevisiones;

        public PantallaInicio()
        {
            InitializeComponent();
        }

        private void btnRegistrar_Click_1(object sender, EventArgs e)
        {

            string textoIngresado = txtUsuario.Text;
            string contraseñaIngresada = txtContraseña.Text;
            pantallaRevisiones = new PantallaRevisiones();
            pantallaRevisiones.TomarOpcionRegistrarResultadoRevManual();
            pantallaRevisiones.CrearNuevaSesion(textoIngresado, contraseñaIngresada);
            // this.Hide();
        }

        private void InitializeComponent()
        {
            btnRegistrar = new Button();
            lblBienvenida = new Label();
            panelContenedor = new Panel();
            button1 = new Button();
            label2 = new Label();
            label1 = new Label();
            txtContraseña = new TextBox();
            txtUsuario = new TextBox();
            panelContenedor.SuspendLayout();
            SuspendLayout();
            // 
            // btnRegistrar
            // 
            btnRegistrar.BackColor = Color.FromArgb(251, 191, 36);
            btnRegistrar.FlatAppearance.BorderSize = 0;
            btnRegistrar.FlatStyle = FlatStyle.Flat;
            btnRegistrar.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            btnRegistrar.ForeColor = Color.Black;
            btnRegistrar.Location = new Point(275, 430);
            btnRegistrar.Name = "btnRegistrar";
            btnRegistrar.Size = new Size(370, 60);             ;
            btnRegistrar.TabIndex = 1;
            btnRegistrar.Text = "Registrar revisión manual";
            btnRegistrar.UseVisualStyleBackColor = false;
            btnRegistrar.Click += btnRegistrar_Click_1;
            // 
            // lblBienvenida
            // 
            lblBienvenida.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblBienvenida.ForeColor = Color.FromArgb(251, 191, 36);
            lblBienvenida.Location = new Point(0, 40);
            lblBienvenida.Name = "lblBienvenida";
            lblBienvenida.Size = new Size(900, 60);
            lblBienvenida.TabIndex = 0;
            lblBienvenida.Text = "Bienvenido al sistema de revisión sísmica";
            lblBienvenida.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panelContenedor
            // 
            panelContenedor.BackColor = Color.FromArgb(30, 41, 59);
            panelContenedor.Controls.Add(button1);
            panelContenedor.Controls.Add(label2);
            panelContenedor.Controls.Add(label1);
            panelContenedor.Controls.Add(txtContraseña);
            panelContenedor.Controls.Add(txtUsuario);
            panelContenedor.Controls.Add(lblBienvenida);
            panelContenedor.Controls.Add(btnRegistrar);
            panelContenedor.Dock = DockStyle.Fill;
            panelContenedor.Location = new Point(0, 0);
            panelContenedor.Name = "panelContenedor";
            panelContenedor.Size = new Size(900, 600);
            panelContenedor.TabIndex = 0;
            panelContenedor.Paint += panelContenedor_Paint;
            // 
            // button1 (Iniciar sesión)
            // 
            button1.Name = "button1";
            button1.Size = new Size(350, 45);
            button1.Location = new Point(275, 340);
            button1.TabIndex = 6;
            button1.Text = "Iniciar Sesión";
            button1.UseVisualStyleBackColor = false;
            button1.BackColor = Color.FromArgb(37, 99, 235);
            button1.FlatStyle = FlatStyle.Flat;
            button1.FlatAppearance.BorderSize = 0;
            button1.ForeColor = Color.White;
            button1.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            button1.Cursor = Cursors.Hand;
            button1.Click += button1_Click;
            // 
            // label2 (Contraseña)
            // 
            label2.AutoSize = true;
            label2.ForeColor = Color.White;
            label2.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label2.Location = new Point(275, 245);
            label2.Name = "label2";
            label2.Size = new Size(98, 23);
            label2.TabIndex = 5;
            label2.Text = "Contraseña";
            // 
            // label1 (Usuario)
            // 
            label1.AutoSize = true;
            label1.ForeColor = Color.White;
            label1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label1.Location = new Point(275, 165);
            label1.Name = "label1";
            label1.Size = new Size(140, 23);
            label1.TabIndex = 4;
            label1.Text = "Nombre usuario";
            // 
            // txtContraseña
            // 
            txtContraseña.Location = new Point(275, 270);
            txtContraseña.Name = "txtContraseña";
            txtContraseña.PlaceholderText = "Ingrese contraseña...";
            txtContraseña.Size = new Size(350, 32);
            txtContraseña.TabIndex = 3;
            txtContraseña.BackColor = Color.FromArgb(51, 65, 85);
            txtContraseña.ForeColor = Color.White;
            txtContraseña.BorderStyle = BorderStyle.FixedSingle;
            txtContraseña.PasswordChar = '●';
            // 
            // txtUsuario
            // 
            txtUsuario.Location = new Point(275, 190);
            txtUsuario.Name = "txtUsuario";
            txtUsuario.PlaceholderText = "Ingrese nombre usuario...";
            txtUsuario.Size = new Size(350, 32);
            txtUsuario.TabIndex = 2;
            txtUsuario.BackColor = Color.FromArgb(51, 65, 85);
            txtUsuario.ForeColor = Color.White;
            txtUsuario.BorderStyle = BorderStyle.FixedSingle;
            // 
            // PantallaInicio
            // 
            BackColor = Color.FromArgb(15, 23, 42);
            ClientSize = new Size(900, 600);
            Controls.Add(panelContenedor);
            Font = new Font("Segoe UI", 11F);
            Name = "PantallaInicio";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Inicio - Proyecto PPAI";
            Load += PantallaInicio_Load_1;
            panelContenedor.ResumeLayout(false);
            panelContenedor.PerformLayout();
            ResumeLayout(false);
        }

        private void PantallaInicio_Load_1(object sender, EventArgs e)
        {
            btnRegistrar.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Ocultar controles del login
            txtContraseña.Visible = false;
            txtUsuario.Visible = false;
            button1.Visible = false;
            label1.Visible = false;
            label2.Visible = false;

            // Mostrar mensaje de bienvenida (opcional)
            lblBienvenida.Text = "Bienvenido al sistema de revisión sísmica";

            // Mostrar y centrar el botón de registro
            btnRegistrar.Visible = true;
            btnRegistrar.Location = new Point(
                (panelContenedor.Width - btnRegistrar.Width) / 2,
                (panelContenedor.Height - btnRegistrar.Height) / 2
            );
        }

        private void panelContenedor_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}


