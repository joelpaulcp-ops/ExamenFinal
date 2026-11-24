using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLLCasierraJoel
{
    public static class ClsDigitoVerificador
    {
        
        public static bool ValidarCedula(string cedula, out string mensaje)
        {
            mensaje = "";

            if (string.IsNullOrWhiteSpace(cedula))
            {
                mensaje = "Ingrese la cédula.";
                return false;
            }

            if (cedula.Length != 10)
            {
                mensaje = "La cédula debe tener 10 dígitos.";
                return false;
            }

            foreach (char c in cedula)
            {
                if (!char.IsDigit(c))
                {
                    mensaje = "La cédula solo debe contener números.";
                    return false;
                }
            }

            // Código de provincia: 01 a 24 (o 30 para ecuatorianos en el exterior)
            int provincia = int.Parse(cedula.Substring(0, 2));
            if (!((provincia >= 1 && provincia <= 24) || provincia == 30))
            {
                mensaje = "Los dos primeros dígitos (provincia) no son válidos.";
                return false;
            }

            // El tercer dígito debe ser menor a 6 (persona natural)
            int tercero = int.Parse(cedula.Substring(2, 1));
            if (tercero >= 6)
            {
                mensaje = "El tercer dígito de la cédula no es válido.";
                return false;
            }

            // Algoritmo módulo 10
            int[] coeficientes = { 2, 1, 2, 1, 2, 1, 2, 1, 2 };
            int suma = 0;

            for (int i = 0; i < 9; i++)
            {
                int valor = int.Parse(cedula[i].ToString()) * coeficientes[i];
                if (valor > 9) valor -= 9;
                suma += valor;
            }

            int verificadorCalculado = (10 - (suma % 10)) % 10;
            int verificadorIngresado = int.Parse(cedula[9].ToString());

            if (verificadorCalculado != verificadorIngresado)
            {
                mensaje = "La cédula es incorrecta (el dígito verificador no coincide).";
                return false;
            }

            mensaje = "Cédula válida.";
            return true;
        }
    }
}
