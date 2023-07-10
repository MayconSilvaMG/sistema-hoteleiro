using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemaHotel
{
    public partial class frmMenu : Form
    {
        public frmMenu()
        {
            InitializeComponent();
        }

        private void frmMenu_Resize(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmMenu_Load(object sender, EventArgs e)
        {
            pnlTopo.BackColor = Color.FromArgb(230, 230, 230);
            pnlRight.BackColor = Color.FromArgb(125, 125, 125);

            lblUsuario.Text = Program.nomeUsuario;
            lblCargo.Text = Program.cargoUsuario;

        }

        private void funcionariosToolStripMenuItem_Click(object sender, EventArgs e) 
        {
            View.frmFuncionarios form = new View.frmFuncionarios();
            form.Show();
        }

        private void cargoToolStripMenuItem_Click(object sender, EventArgs e) {
            View.frmCargo form = new View.frmCargo();
            form.Show();
        }

        private void button1_Click(object sender, EventArgs e) {
            View.frmProdutos form = new View.frmProdutos();
            form.Show();
        }

        private void novoProdutoToolStripMenuItem_Click(object sender, EventArgs e) {
            View.frmProdutos form = new View.frmProdutos();
            form.Show();
        }

        private void usuáriosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            View.frmUsuario form = new View.frmUsuario();
            form.Show();
        }

        private void fornecedoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            View.frmFornecedores form = new View.frmFornecedores();
            form.Show();
        }

        private void estoqueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            View.frmEstoque form = new View.frmEstoque();
            form.Show();
        }

        private void serviçosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            View.frmServicos form = new View.frmServicos();
            form.Show();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}