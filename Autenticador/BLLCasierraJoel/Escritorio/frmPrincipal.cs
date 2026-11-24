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
    public partial class frmPrincipal : Form
    {
        public frmPrincipal()
        {
            InitializeComponent();
        }

        private void operadoresBasicosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmOperadores operadores = new frmOperadores();
            operadores.MdiParent = this;    
            operadores.Show();
        }

        private void calculadoraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCalculadora calculadora = new frmCalculadora();
            calculadora.MdiParent = this;
            calculadora.Show();
        }

        private void digitoVerificadorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDigitoVerificador digitoVerificador = new frmDigitoVerificador();
            digitoVerificador.MdiParent = this;
            digitoVerificador.Show();
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void acercaDeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAcercaDe AcercaDe = new frmAcercaDe();
            AcercaDe.MdiParent = this;
            AcercaDe.Show();
        }
    }
}
