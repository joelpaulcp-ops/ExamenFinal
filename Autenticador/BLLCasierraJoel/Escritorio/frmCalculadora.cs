using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Forms;

namespace Escritorio
{
    public partial class frmCalculadora : Form
    {
        public frmCalculadora()
        {
            InitializeComponent();
            ConfigurarCalculadora();

        }
    
  // ---------- Campos ----------
    private TextBox pantalla;
        private double memoria = 0;
        private bool resultadoMostrado = false;
        private bool hayError = false;

        private const string ErrDivCero = "No se puede dividir entre cero";
        private const string ErrInvalido = "Entrada no válida";

        // ---------- Configuración inicial ----------
        private void ConfigurarCalculadora()
        {
            pantalla = BuscarTextBox(this);
            if (pantalla != null)
            {
                pantalla.ReadOnly = true;
                pantalla.TextAlign = HorizontalAlignment.Right;
                pantalla.Text = "";
            }
            ConectarBotones(this);
        }

        private TextBox BuscarTextBox(Control padre)
        {
            foreach (Control c in padre.Controls)
            {
                if (c is TextBox tb) return tb;
                TextBox r = BuscarTextBox(c);
                if (r != null) return r;
            }
            return null;
        }

        private void ConectarBotones(Control padre)
        {
            foreach (Control c in padre.Controls)
            {
                if (c is Button b)
                {
                    b.Click -= Boton_Click;
                    b.Click += Boton_Click;
                }
                else ConectarBotones(c);
            }
        }

        // ---------- Manejador único de botones ----------
        private void Boton_Click(object sender, EventArgs e)
        {
            string t = ((Button)sender).Text.Trim();

            // Si hay un mensaje de error en pantalla, se limpia antes de seguir
            if (hayError)
            {
                LimpiarTodo();
                if (t == "C" || t == "CE" || t == "←" || t == "⌫") return;
            }

            if (t.Length == 1 && char.IsDigit(t[0])) { Digito(t[0]); return; }

            switch (t)
            {
                case ",":
                case ".": Coma(); break;

                case "+": Operador('+'); break;
                case "-": Operador('-'); break;
                case "x":
                case "X":
                case "×":
                case "*": Operador('x'); break;
                case "÷":
                case "/": Operador('÷'); break;

                case "=": Igual(); break;

                case "C": LimpiarTodo(); break;
                case "CE": LimpiarEntrada(); break;
                case "←":
                case "⌫": Retroceso(); break;

                case "+/-": CambiarSigno(); break;
                case "%": Unaria(v => v / 100.0); break;
                case "x²": Unaria(v => v * v); break;

                case "MC": memoria = 0; break;
                case "MR": RecuperarMemoria(); break;
                case "M+": memoria += ValorActual(); break;
                case "M-": memoria -= ValorActual(); break;
                case "MS": memoria = ValorActual(); break;

                default:
                    if (t.Contains("√")) Unaria(RaizCuadrada);       // botón de raíz
                    else if (t.EndsWith("/x")) Unaria(Inverso);      // botón 1/x
                    break;
            }
        }

        // ---------- Utilidades de texto ----------
        private static bool EsOp(char c) => c == '+' || c == '-' || c == 'x' || c == '÷';

        // Índice donde empiezan los dígitos del número actual (sin signo)
        private int InicioNumero()
        {
            string s = pantalla.Text;
            int i = s.Length;
            while (i > 0 && (char.IsDigit(s[i - 1]) || s[i - 1] == ',')) i--;
            return i;
        }

        // Igual que el anterior pero incluye el signo "-" si es negativo (unario)
        private int InicioConSigno()
        {
            string s = pantalla.Text;
            int i = InicioNumero();
            if (i > 0 && s[i - 1] == '-' && (i - 1 == 0 || EsOp(s[i - 2]))) i--;
            return i;
        }

        private bool ObtenerNumeroActual(out double valor)
        {
            valor = 0;
            string s = pantalla.Text.Substring(InicioConSigno());
            if (s == "" || s == "-") return false;
            return double.TryParse(s.Replace(',', '.'), NumberStyles.Float,
                                   CultureInfo.InvariantCulture, out valor);
        }

        private static string Formato(double v)
        {
            return Math.Round(v, 10).ToString("0.##########", CultureInfo.InvariantCulture)
                                    .Replace('.', ',');
        }

        private void ReemplazarNumeroActual(double v)
        {
            pantalla.Text = pantalla.Text.Substring(0, InicioConSigno()) + Formato(v);
        }

        private void MostrarError(string msg)
        {
            pantalla.Text = msg;
            hayError = true;
            resultadoMostrado = false;
        }

        // ---------- Entrada de dígitos, coma y operadores ----------
        private void Digito(char d)
        {
            if (resultadoMostrado) { pantalla.Text = ""; resultadoMostrado = false; }

            string s = pantalla.Text;
            int i = InicioNumero();
            // Evita ceros a la izquierda (05)
            if (s.Substring(i) == "0") s = s.Substring(0, i);
            pantalla.Text = s + d;
        }

        private void Coma()
        {
            if (resultadoMostrado) { pantalla.Text = ""; resultadoMostrado = false; }

            string num = pantalla.Text.Substring(InicioNumero());
            if (num.Contains(",")) return;
            pantalla.Text += (num == "") ? "0," : ",";
        }

        private void Operador(char op)
        {
            resultadoMostrado = false;   // si venía de un resultado, se continúa con él
            string t = pantalla.Text;

            if (t == "")
            {
                if (op == '-') pantalla.Text = "-";   // permite iniciar con número negativo
                return;
            }

            if (EsOp(t[t.Length - 1]))
            {
                // Reemplaza el operador anterior por el nuevo
                string baseT = t.Substring(0, t.Length - 1);
                if (baseT == "") { pantalla.Text = (op == '-') ? "-" : ""; return; }
                pantalla.Text = baseT + op;
                return;
            }

            pantalla.Text = t + op;
        }

        // ---------- Cálculo ----------
        private void Igual()
        {
            string t = pantalla.Text;
            while (t.Length > 0 && EsOp(t[t.Length - 1])) t = t.Substring(0, t.Length - 1);
            if (t == "") return;

            try
            {
                double r = Evaluar(t);
                pantalla.Text = Formato(r);
                resultadoMostrado = true;
            }
            catch (DivideByZeroException) { MostrarError(ErrDivCero); }
            catch { MostrarError(ErrInvalido); }
        }

        // Evalúa respetando la jerarquía (x y ÷ antes que + y -)
        private double Evaluar(string t)
        {
            List<double> nums = new List<double>();
            List<char> ops = new List<char>();
            int i = 0;

            while (true)
            {
                int signo = 1;
                if (i < t.Length && t[i] == '-') { signo = -1; i++; }

                int ini = i;
                while (i < t.Length && (char.IsDigit(t[i]) || t[i] == ',')) i++;
                if (ini == i) throw new FormatException();

                nums.Add(signo * double.Parse(t.Substring(ini, i - ini).Replace(',', '.'),
                                              CultureInfo.InvariantCulture));
                if (i >= t.Length) break;

                ops.Add(t[i]);
                i++;
            }

            // Primero x y ÷
            for (int k = 0; k < ops.Count;)
            {
                if (ops[k] == 'x' || ops[k] == '÷')
                {
                    double r;
                    if (ops[k] == 'x') r = nums[k] * nums[k + 1];
                    else
                    {
                        if (nums[k + 1] == 0) throw new DivideByZeroException();
                        r = nums[k] / nums[k + 1];
                    }
                    nums[k] = r;
                    nums.RemoveAt(k + 1);
                    ops.RemoveAt(k);
                }
                else k++;
            }

            // Luego + y -
            double res = nums[0];
            for (int k = 0; k < ops.Count; k++)
                res = (ops[k] == '+') ? res + nums[k + 1] : res - nums[k + 1];

            return res;
        }

        // ---------- Funciones sobre el número actual ----------
        private double Inverso(double v)
        {
            if (v == 0) throw new DivideByZeroException();
            return 1.0 / v;
        }

        private double RaizCuadrada(double v)
        {
            if (v < 0) throw new ArgumentException();
            return Math.Sqrt(v);
        }

        private void Unaria(Func<double, double> f)
        {
            if (!ObtenerNumeroActual(out double v)) return;
            try
            {
                double r = f(v);
                if (double.IsNaN(r) || double.IsInfinity(r)) throw new ArgumentException();
                ReemplazarNumeroActual(r);
            }
            catch (DivideByZeroException) { MostrarError(ErrDivCero); }
            catch { MostrarError(ErrInvalido); }
        }

        private void CambiarSigno()
        {
            string s = pantalla.Text;
            int i = InicioNumero();
            if (i == s.Length) return;   // no hay número actual

            if (i > 0 && s[i - 1] == '-' && (i - 1 == 0 || EsOp(s[i - 2])))
                pantalla.Text = s.Remove(i - 1, 1);                          // quita el negativo
            else if (i > 0 && s[i - 1] == '+')
                pantalla.Text = s.Substring(0, i - 1) + "-" + s.Substring(i); // 5+3 -> 5-3
            else if (i > 0 && s[i - 1] == '-')
                pantalla.Text = s.Substring(0, i - 1) + "+" + s.Substring(i); // 5-3 -> 5+3
            else
                pantalla.Text = s.Insert(i, "-");                             // 3 -> -3
        }

        // ---------- Borrado ----------
        private void LimpiarTodo()
        {
            pantalla.Text = "";
            resultadoMostrado = false;
            hayError = false;
        }

        private void LimpiarEntrada()
        {
            if (resultadoMostrado) { LimpiarTodo(); return; }
            pantalla.Text = pantalla.Text.Substring(0, InicioConSigno());
        }

        private void Retroceso()
        {
            if (pantalla.Text.Length > 0)
                pantalla.Text = pantalla.Text.Substring(0, pantalla.Text.Length - 1);
            resultadoMostrado = false;
        }

        // ---------- Memoria ----------
        private double ValorActual()
        {
            string t = pantalla.Text;
            if (t == "") return 0;

            if (EsOp(t[t.Length - 1]))
                return ObtenerNumeroActual(out double v) ? v : 0;

            try { return Evaluar(t); }
            catch { return 0; }
        }

        private void RecuperarMemoria()
        {
            if (resultadoMostrado) { pantalla.Text = ""; resultadoMostrado = false; }
            ReemplazarNumeroActual(memoria);
        }
    }
}
