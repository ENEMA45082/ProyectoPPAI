using System;

namespace ProyectoPPAI.Pantalla
{
    partial class PantallaRevisiones
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dataGridViewEventos = new DataGridView();
            dataGridViewSeleccionado = new DataGridView();
            label1 = new Label();
            labelTitulo = new Label();
            dataGridViewSerie = new DataGridView();
            labelVisualizarMapa = new Label();
            comboBox1 = new ComboBox();
            button1 = new Button();
            checkBoxMostrarDatos = new CheckBox();
            button2 = new Button();
            btnDerivarAExperto = new Button();
            panel1 = new Panel();
            panel3 = new Panel();
            panel4 = new Panel();
            panel2 = new Panel();
            panelFondo = new Panel();
            buttonFinal = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridViewEventos).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewSeleccionado).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewSerie).BeginInit();
            panel1.SuspendLayout();
            panel3.SuspendLayout();
            panelFondo.SuspendLayout();
            SuspendLayout();
            // 
            // dataGridViewEventos
            // 
            dataGridViewEventos.Anchor = AnchorStyles.Top;
            dataGridViewEventos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewEventos.Location = new Point(64, 98);
            dataGridViewEventos.Margin = new Padding(3, 2, 3, 2);
            dataGridViewEventos.Name = "dataGridViewEventos";
            dataGridViewEventos.RowHeadersWidth = 51;
            dataGridViewEventos.Size = new Size(900, 385);
            dataGridViewEventos.TabIndex = 0;
            dataGridViewEventos.CellContentDoubleClick += TomarEventoSismicoSeleccionado;
            // 
            // dataGridViewSeleccionado
            // 
            dataGridViewSeleccionado.AllowUserToAddRows = false;
            dataGridViewSeleccionado.Anchor = AnchorStyles.Top;
            dataGridViewSeleccionado.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
            dataGridViewSeleccionado.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewSeleccionado.Location = new Point(64, 98);
            dataGridViewSeleccionado.Margin = new Padding(3, 2, 3, 2);
            dataGridViewSeleccionado.Name = "dataGridViewSeleccionado";
            dataGridViewSeleccionado.RowHeadersWidth = 51;
            dataGridViewSeleccionado.Size = new Size(900, 79);
            dataGridViewSeleccionado.TabIndex = 1;
            dataGridViewSeleccionado.CellContentClick += dataGridViewSeleccionado_CellContentClick;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.FromArgb(0, 64, 64);
            label1.Location = new Point(445, 81);
            label1.Name = "label1";
            label1.Size = new Size(116, 15);
            label1.TabIndex = 2;
            label1.Text = "Evento Seleccionado";
            // 
            // labelTitulo
            // 
            labelTitulo.AutoSize = true;
            labelTitulo.BackColor = Color.FromArgb(0, 64, 64);
            labelTitulo.Location = new Point(397, 81);
            labelTitulo.Name = "labelTitulo";
            labelTitulo.Size = new Size(217, 15);
            labelTitulo.TabIndex = 3;
            labelTitulo.Text = "EVENTOS SISMICOS AUTODETECTADOS";
            // 
            // dataGridViewSerie
            // 
            dataGridViewSerie.AllowUserToAddRows = false;
            dataGridViewSerie.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            dataGridViewSerie.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
            dataGridViewSerie.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewSerie.Location = new Point(202, 110);
            dataGridViewSerie.Margin = new Padding(3, 2, 3, 2);
            dataGridViewSerie.Name = "dataGridViewSerie";
            dataGridViewSerie.RowHeadersWidth = 51;
            dataGridViewSerie.Size = new Size(590, 265);
            dataGridViewSerie.TabIndex = 4;
            dataGridViewSerie.CellContentClick += dataGridViewSerie_CellContentClick;
            // 
            // labelVisualizarMapa
            // 
            labelVisualizarMapa.AutoSize = true;
            labelVisualizarMapa.Location = new Point(840, 179);
            labelVisualizarMapa.Name = "labelVisualizarMapa";
            labelVisualizarMapa.Size = new Size(124, 15);
            labelVisualizarMapa.TabIndex = 6;
            labelVisualizarMapa.Text = "Visualizacion de Mapa";
            labelVisualizarMapa.Click += labelVisualizarMapa_Click;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "Visible", "No Visible" });
            comboBox1.Location = new Point(840, 196);
            comboBox1.Margin = new Padding(3, 2, 3, 2);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(137, 23);
            comboBox1.TabIndex = 7;
            comboBox1.Text = "Seleccionar";
            // 
            // button1
            // 
            button1.BackColor = Color.OrangeRed;
            button1.FlatAppearance.BorderSize = 0;
            button1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            button1.ForeColor = SystemColors.ButtonHighlight;
            button1.Location = new Point(251, 379);
            button1.Margin = new Padding(3, 2, 3, 2);
            button1.Name = "button1";
            button1.Size = new Size(162, 35);
            button1.TabIndex = 8;
            button1.Text = "Rechazar Evento Sismico";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // checkBoxMostrarDatos
            // 
            checkBoxMostrarDatos.AutoSize = true;
            checkBoxMostrarDatos.BackColor = Color.FromArgb(0, 64, 64);
            checkBoxMostrarDatos.ForeColor = SystemColors.ButtonFace;
            checkBoxMostrarDatos.Location = new Point(64, 181);
            checkBoxMostrarDatos.Margin = new Padding(3, 2, 3, 2);
            checkBoxMostrarDatos.Name = "checkBoxMostrarDatos";
            checkBoxMostrarDatos.Size = new Size(110, 19);
            checkBoxMostrarDatos.TabIndex = 9;
            checkBoxMostrarDatos.Text = "Modificar Datos";
            checkBoxMostrarDatos.UseVisualStyleBackColor = false;
            checkBoxMostrarDatos.Click += checkBoxMostrarDatos_Click;
            // 
            // button2
            // 
            button2.BackColor = Color.OrangeRed;
            button2.FlatAppearance.BorderSize = 0;
            button2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            button2.ForeColor = SystemColors.ButtonHighlight;
            button2.Location = new Point(586, 379);
            button2.Margin = new Padding(3, 2, 3, 2);
            button2.Name = "button2";
            button2.Size = new Size(162, 35);
            button2.TabIndex = 10;
            button2.Text = "Confirmar Evento Sismico";
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // btnDerivarAExperto
            // 
            btnDerivarAExperto.BackColor = Color.OrangeRed;
            btnDerivarAExperto.FlatAppearance.BorderSize = 0;
            btnDerivarAExperto.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnDerivarAExperto.ForeColor = SystemColors.ButtonHighlight;
            btnDerivarAExperto.Location = new Point(418, 379);
            btnDerivarAExperto.Margin = new Padding(3, 2, 3, 2);
            btnDerivarAExperto.Name = "btnDerivarAExperto";
            btnDerivarAExperto.Size = new Size(162, 35);
            btnDerivarAExperto.TabIndex = 11;
            btnDerivarAExperto.Text = "Derivar a Experto";
            btnDerivarAExperto.UseVisualStyleBackColor = false;
            btnDerivarAExperto.Click += btnDerivarAExperto_Click;
            // 
            // panel1
            // 
            panel1.BackColor = Color.OrangeRed;
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(panel2);
            panel1.Location = new Point(-4, -14);
            panel1.Margin = new Padding(2);
            panel1.Name = "panel1";
            panel1.Size = new Size(1042, 48);
            panel1.TabIndex = 13;
            // 
            // panel3
            // 
            panel3.BackColor = Color.OrangeRed;
            panel3.Controls.Add(panel4);
            panel3.Location = new Point(0, 5);
            panel3.Margin = new Padding(2);
            panel3.Name = "panel3";
            panel3.Size = new Size(1048, 48);
            panel3.TabIndex = 15;
            panel3.Paint += panel3_Paint_1;
            // 
            // panel4
            // 
            panel4.BackColor = Color.OrangeRed;
            panel4.Location = new Point(6, 171);
            panel4.Margin = new Padding(2);
            panel4.Name = "panel4";
            panel4.Size = new Size(1042, 65);
            panel4.TabIndex = 14;
            // 
            // panel2
            // 
            panel2.BackColor = Color.OrangeRed;
            panel2.Location = new Point(6, 171);
            panel2.Margin = new Padding(2);
            panel2.Name = "panel2";
            panel2.Size = new Size(1042, 65);
            panel2.TabIndex = 14;
            // 
            // panelFondo
            // 
            panelFondo.BackColor = Color.FromArgb(0, 18, 38);
            panelFondo.Controls.Add(dataGridViewSerie);
            panelFondo.Controls.Add(btnDerivarAExperto);
            panelFondo.Controls.Add(button1);
            panelFondo.Controls.Add(button2);
            panelFondo.Location = new Point(27, 81);
            panelFondo.Margin = new Padding(2);
            panelFondo.Name = "panelFondo";
            panelFondo.Size = new Size(980, 418);
            panelFondo.TabIndex = 16;
            panelFondo.Paint += panel5_Paint;
            // 
            // buttonFinal
            // 
            buttonFinal.BackColor = Color.Red;
            buttonFinal.FlatAppearance.BorderSize = 0;
            buttonFinal.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            buttonFinal.ForeColor = SystemColors.ButtonHighlight;
            buttonFinal.Location = new Point(279, 511);
            buttonFinal.Margin = new Padding(3, 2, 3, 2);
            buttonFinal.Name = "buttonFinal";
            buttonFinal.Size = new Size(497, 35);
            buttonFinal.TabIndex = 17;
            buttonFinal.Text = "FINALIZAR REVISION";
            buttonFinal.UseVisualStyleBackColor = false;
            buttonFinal.Click += finCU;
            // 
            // PantallaRevisiones
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1035, 556);
            Controls.Add(buttonFinal);
            Controls.Add(panelFondo);
            Controls.Add(panel1);
            Controls.Add(checkBoxMostrarDatos);
            Controls.Add(comboBox1);
            Controls.Add(labelVisualizarMapa);
            Controls.Add(labelTitulo);
            Controls.Add(label1);
            Controls.Add(dataGridViewSeleccionado);
            Controls.Add(dataGridViewEventos);
            Margin = new Padding(3, 2, 3, 2);
            Name = "PantallaRevisiones";
            Text = "Form1";
            Load += PantallaRevisiones_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridViewEventos).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewSeleccionado).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewSerie).EndInit();
            panel1.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panelFondo.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridViewEventos;
        private DataGridView dataGridViewSeleccionado;
        private Label label1;
        private Label labelTitulo;
        private DataGridView dataGridViewSerie;
        private Label labelVisualizarMapa;
        private ComboBox comboBox1;
        private Button button1;
        private CheckBox checkBoxMostrarDatos;
        private Button button2;
        private Button btnDerivarAExperto;
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private Panel panel4;
        private Panel panelFondo;
        private Button buttonFinal;
    }
}