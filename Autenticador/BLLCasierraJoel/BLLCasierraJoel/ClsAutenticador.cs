using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLLCasierraJoel
{
    public class ClsAutenticador
    {
        public string Usuario = "JoelCasierra";
        public string Clave = "12345abcde";
        public bool Autenticacion (String usuario, String clave)
        {
         if (Usuario == usuario && Clave == clave)
            {
                return true;
            }
         else
            {
                return false;
            }
        }
    }
}
