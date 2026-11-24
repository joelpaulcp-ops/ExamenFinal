using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLLCasierraJoel
{
    public class ClsOperadores
    {
        public double Sumar(double N1, double N2)
        {
            return N1 + N2;
        }
        public double Restar(double N1, double N2)
        {
            return N1 - N2;
        }
        public double Multiplicar(double N1, double N2)
        {
            return N1 * N2;
        }
        public double Dividir(double N1, double N2)
        {
            if (N2 != 0)
            {
                return N1 / N2;
            }
            else
            {
                throw new DivideByZeroException(
                    "No se puede dividir entre cero."
                );
            }
        }
    }
}
