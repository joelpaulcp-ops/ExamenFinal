using BLLCasierraJoel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Escritorio
{
    public partial class frmOperadores : Form
    {
        public double primerNumero;
        public string operacion = "";
        public bool nuevaEntrada = true;

        // Nuevas variables
        public string segundoNumeroTexto = "";
        public string expresion = "";
        public double ultimoResultado = 0;
        public bool resultadoMostrado = false;

        public frmOperadores()
        {
            InitializeComponent();

            txtPantalla.ReadOnly = true;
            txtPantalla.TextAlign = HorizontalAlignment.Right;
            txtPantalla.Text = "0";
        }

        // Agregar un número a la pantalla
        public void AgregarNumero(string numero)
        {
            if (!string.IsNullOrEmpty(operacion))
            {
                if (nuevaEntrada)
                {
                    segundoNumeroTexto = numero;
                    txtPantalla.Text += numero;
                    nuevaEntrada = false;
                }
                else
                {
                    segundoNumeroTexto += numero;
                    txtPantalla.Text += numero;
                }
            }
            else
            {
                if (nuevaEntrada || txtPantalla.Text == "0" || resultadoMostrado)
                {
                    txtPantalla.Text = numero;
                    nuevaEntrada = false;
                    resultadoMostrado = false;
                }
                else
                {
                    txtPantalla.Text += numero;
                }
            }
        }

        public void btn0_Click(object sender, EventArgs e)
        {
            AgregarNumero("0");
        }

        public void btn1_Click(object sender, EventArgs e)
        {
            AgregarNumero("1");
        }

        public void btn2_Click(object sender, EventArgs e)
        {
            AgregarNumero("2");
        }

        public void btn3_Click(object sender, EventArgs e)
        {
            AgregarNumero("3");
        }

        public void btn4_Click(object sender, EventArgs e)
        {
            AgregarNumero("4");
        }

        public void btn5_Click(object sender, EventArgs e)
        {
            AgregarNumero("5");
        }

        public void btn6_Click(object sender, EventArgs e)
        {
            AgregarNumero("6");
        }

        public void btn7_Click(object sender, EventArgs e)
        {
            AgregarNumero("7");
        }

        public void btn8_Click(object sender, EventArgs e)
        {
            AgregarNumero("8");
        }

        public void btn9_Click(object sender, EventArgs e)
        {
            AgregarNumero("9");
        }

        public void btnPunto_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(operacion))
            {
                if (nuevaEntrada)
                {
                    segundoNumeroTexto = "0.";
                    txtPantalla.Text += "0.";
                    nuevaEntrada = false;
                }
                else if (!segundoNumeroTexto.Contains("."))
                {
                    segundoNumeroTexto += ".";
                    txtPantalla.Text += ".";
                }
            }
            else
            {
                if (nuevaEntrada || resultadoMostrado)
                {
                    txtPantalla.Text = "0.";
                    nuevaEntrada = false;
                    resultadoMostrado = false;
                }
                else if (!txtPantalla.Text.Contains("."))
                {
                    txtPantalla.Text += ".";
                }
            }
        }
        public void PrepararOperacion(string signo)
        {
            if (!double.TryParse(
       txtPantalla.Text,
       NumberStyles.Float,
       CultureInfo.InvariantCulture,
       out primerNumero) && !resultadoMostrado)
            {
                MessageBox.Show("Introduce un número válido.");
                return;
            }

            if (resultadoMostrado)
            {
                primerNumero = ultimoResultado;
            }

            operacion = signo;

            expresion = primerNumero.ToString(
                "G15",
                CultureInfo.InvariantCulture) + signo;

            txtPantalla.Text = expresion;

            segundoNumeroTexto = "";
            nuevaEntrada = true;
            resultadoMostrado = false;
        }

        public void btnSumar_Click(object sender, EventArgs e)
        {
            PrepararOperacion("+");
        }

        public void btnRestar_Click(object sender, EventArgs e)
        {
            PrepararOperacion("-");
        }

        public void btnMultiplicar_Click(object sender, EventArgs e)
        {
            PrepararOperacion("*");
        }

        public void btnDividir_Click(object sender, EventArgs e)
        {
            PrepararOperacion("/");
        }

        public void btnIgual_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(operacion))
                return;

            if (string.IsNullOrEmpty(segundoNumeroTexto))
            {
                MessageBox.Show("Introduce el segundo número.");
                return;
            }

            if (!double.TryParse(
                segundoNumeroTexto,
                NumberStyles.Float,
                CultureInfo.InvariantCulture,
                out double segundoNumero))
            {
                MessageBox.Show("Introduce un número válido.");
                return;
            }

            double resultado;

            switch (operacion)
            {
                case "+":
                    resultado = primerNumero + segundoNumero;
                    break;

                case "-":
                    resultado = primerNumero - segundoNumero;
                    break;

                case "*":
                    resultado = primerNumero * segundoNumero;
                    break;

                case "/":

                    if (segundoNumero == 0)
                    {
                        MessageBox.Show(
                            "No se puede dividir entre cero.",
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        txtPantalla.Text = "0";
                        operacion = "";
                        nuevaEntrada = true;
                        segundoNumeroTexto = "";
                        expresion = "";
                        return;
                    }

                    resultado = primerNumero / segundoNumero;
                    break;

                default:
                    return;
            }

            // Construir la expresión completa
            txtPantalla.Text = resultado.ToString("G15",CultureInfo.InvariantCulture);

            ultimoResultado = resultado;

            operacion = "";
            segundoNumeroTexto = "";
            expresion = "";

            nuevaEntrada = true;
            resultadoMostrado = true;

        }

        public void btnLimpiar_Click(object sender, EventArgs e)
        {
            primerNumero = 0;
            operacion = "";
            segundoNumeroTexto = "";
            expresion = "";
            ultimoResultado = 0;

            txtPantalla.Text = "0";

            nuevaEntrada = true;
            resultadoMostrado = false;
        }

        public void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void btnRetroceso_Click(object sender, EventArgs e)
        {
            // Si la pantalla está vacía o contiene cero
            if (string.IsNullOrEmpty(txtPantalla.Text) ||
                txtPantalla.Text == "0")
            {
                txtPantalla.Text = "0";
                return;
            }

            // Eliminar el último carácter
            txtPantalla.Text = txtPantalla.Text.Substring(
                0, txtPantalla.Text.Length - 1);

            // Si ya no quedan números, mostrar cero
            if (txtPantalla.Text == "" || txtPantalla.Text == "-")
            {
                txtPantalla.Text = "0";
            }
        }
    }
}
