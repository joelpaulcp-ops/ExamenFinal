using BLLCasierraJoel;
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
    public partial class frmDigitoVerificador : Form
    {
        public frmDigitoVerificador()
        {
            InitializeComponent();
            ConfigurarFormulario();
        }
        private void ConfigurarFormulario()
        {
            // Cédula: solo números, máximo 10
            txtCedula.MaxLength = 10;
            txtCedula.KeyPress += SoloNumeros_KeyPress;

            // Apellidos y nombres: solo letras
            txtApellidos.KeyPress += SoloLetras_KeyPress;
            txtNombres.KeyPress += SoloLetras_KeyPress;

            // Género
            cmbGenero.DropDownStyle = ComboBoxStyle.DropDownList;
            if (cmbGenero.Items.Count == 0)
            {
                cmbGenero.Items.Add("Masculino");
                cmbGenero.Items.Add("Femenino");
            }

            // Fecha: no permitir fechas futuras
            dtpFecha.Format = DateTimePickerFormat.Short;
            dtpFecha.MaxDate = DateTime.Today;

            // Tabla
            dgvDatos.AllowUserToAddRows = false;
            dgvDatos.ReadOnly = true;
            dgvDatos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            if (dgvDatos.Columns.Count == 0)
            {
                dgvDatos.Columns.Add("Cedula", "Cedula");
                dgvDatos.Columns.Add("Apellidos", "Apellidos");
                dgvDatos.Columns.Add("Nombres", "Nombres");
                dgvDatos.Columns.Add("FechaNacim", "Fecha Nacim");
                dgvDatos.Columns.Add("Genero", "Genero");
            }

            // Botones
            btnIngresar.Click += btnIngresar_Click;
            btnLimpiar.Click += btnLimpiar_Click;
            btnSalir.Click += btnSalir_Click;
        }

        // ---------- Restricciones de teclado ----------
        private void SoloNumeros_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void SoloLetras_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && e.KeyChar != ' ')
                e.Handled = true;
        }

        // ---------- Botón Ingresar ----------
        private void btnIngresar_Click(object sender, EventArgs e)
        {
            string cedula = txtCedula.Text.Trim();
            string apellidos = txtApellidos.Text.Trim();
            string nombres = txtNombres.Text.Trim();

            // 1. Validar cédula (dígito verificador)
            if (!ClsDigitoVerificador.ValidarCedula(cedula, out string mensaje))
            {
                MessageBox.Show(mensaje, "Cédula inválida",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCedula.Focus();
                txtCedula.SelectAll();
                return;
            }

            // 2. Validar apellidos
            if (apellidos == "")
            {
                MessageBox.Show("Ingrese los apellidos.", "Dato requerido",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtApellidos.Focus();
                return;
            }

            // 3. Validar nombres
            if (nombres == "")
            {
                MessageBox.Show("Ingrese los nombres.", "Dato requerido",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombres.Focus();
                return;
            }

            // 4. Validar fecha de nacimiento (mayor de 18 años)
            DateTime fecha = dtpFecha.Value.Date;
            int edad = DateTime.Today.Year - fecha.Year;
            if (fecha > DateTime.Today.AddYears(-edad)) edad--;

            if (edad < 18)
            {
                MessageBox.Show("La persona debe ser mayor de 18 años.", "Fecha inválida",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpFecha.Focus();
                return;
            }

            // 5. Validar género
            if (cmbGenero.SelectedIndex < 0)
            {
                MessageBox.Show("Seleccione el género.", "Dato requerido",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbGenero.Focus();
                return;
            }

            // 6. Evitar cédulas repetidas en la tabla
            foreach (DataGridViewRow fila in dgvDatos.Rows)
            {
                if (fila.Cells[0].Value != null && fila.Cells[0].Value.ToString() == cedula)
                {
                    MessageBox.Show("Esta cédula ya fue ingresada.", "Duplicado",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtCedula.Focus();
                    txtCedula.SelectAll();
                    return;
                }
            }

            // 7. Agregar a la tabla
            dgvDatos.Rows.Add(cedula,
                              apellidos.ToUpper(),
                              nombres.ToUpper(),
                              fecha.ToString("dd/MM/yyyy"),
                              cmbGenero.SelectedItem.ToString());

            MessageBox.Show("Cédula válida. Datos ingresados correctamente.", "Correcto",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

            LimpiarCampos();
        }

        // ---------- Botón Limpiar ----------
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void LimpiarCampos()
        {
            txtCedula.Clear();
            txtApellidos.Clear();
            txtNombres.Clear();
            dtpFecha.Value = DateTime.Today;
            cmbGenero.SelectedIndex = -1;
            txtCedula.Focus();
        }

        // ---------- Botón Salir ----------
        private void btnSalir_Click(object sender, EventArgs e)
        {
            DialogResult r = MessageBox.Show("¿Desea salir del formulario?", "Salir",
                                             MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r == DialogResult.Yes) this.Close();
        }

 
        private void frmDigitoVerificador_Load(object sender, EventArgs e)
        {

        }
    }
}
