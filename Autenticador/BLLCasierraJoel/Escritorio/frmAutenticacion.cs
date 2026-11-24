using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Escritorio
{
    public partial class frmAutenticacion : Form
    {
        private bool permitirCerrar = false;

        public frmAutenticacion()
        {
            InitializeComponent();

            // Configuración del formulario.
            this.Text = "Autenticación";
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ControlBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new System.Drawing.Size(427, 313);

            // Configuración de la contraseña.
            txtContrasena.UseSystemPasswordChar = true;

            // Evento que controla cualquier intento de cierre.
            this.FormClosing += frmAutenticacion_FormClosing;
        }

        private void frmAutenticacion_FormClosing(
            object sender,
            FormClosingEventArgs e)
        {
        // Bloquear el cierre normal si no se pulsó Salir.
            if (!permitirCerrar &&
                e.CloseReason != CloseReason.WindowsShutDown)
            {
                e.Cancel = true;
            }
        }
    
        // No Borrar
        private void label3_Click(object sender, EventArgs e)
        {

        }

        //Boton Ingresar
        private void btnIngresar_Click_1(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string contrasena = txtContrasena.Text;

            if (usuario == "" || contrasena == "")
            {
                MessageBox.Show(
                    "Ingrese el usuario y la contraseña.",
                    "Campos obligatorios",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

        // Validación de ejemplo
            if (usuario == "JoelCasierra" && contrasena == "1234")
            {
                frmPrincipal principal = new frmPrincipal();
                principal.Show();

                this.Hide();
            }
            else
            {
                MessageBox.Show(
                    "Usuario o contraseña incorrectos.",
                    "Error de autenticación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                txtContrasena.Clear();
                txtContrasena.Focus();
            }
        }

        //Boton Salir
        private void btnSalir_Click_1(object sender, EventArgs e)
        {
            DialogResult respuesta = MessageBox.Show(
        "¿Está seguro de que desea salir del sistema?",
        "Confirmar salida",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Question
    );

            if (respuesta == DialogResult.Yes)
            {
                permitirCerrar = true;
                Application.Exit();
            }
        }
    }

}
