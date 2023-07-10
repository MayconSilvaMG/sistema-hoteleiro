using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace SistemaHotel.View
{
    public partial class frmEstoque : Form
    {
        Conexao conexao = new Conexao();
        string sql;
        MySqlCommand cmd;
        string id;

        private void CarregarComboBox()
        {
            conexao.AbrirConect();
            sql = "SELECT * FROM fornecedores order by nome asc";
            cmd = new MySqlCommand(sql, conexao.con);
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            DataTable dt = new DataTable();
            da.Fill(dt);
            cbFornece.DataSource = dt;
            cbFornece.ValueMember = "id";
            cbFornece.DisplayMember = "nome";
            
            //conexao.FecharConect();
        }

        private void habilitarCampos()
        {
            //txtEstoque.Enabled = true;
            //txtProduto.Enabled = true;
            txtValor.Enabled = true;
            cbFornece.Enabled = true;
            txtQuantidade.Enabled = true;
            btnSalvar.Enabled = true;
            txtQuantidade.Focus();
        }

        private void desabilitarCampos()
        {
            txtEstoque.Enabled = false;
            txtProduto.Enabled = false;
            txtValor.Enabled = false;
            cbFornece.Enabled = false;
            txtQuantidade.Enabled = false;
            btnSalvar.Enabled = false; 
        }

        private void limparCampos()
        {
            txtEstoque.Text = "";
            txtProduto.Text="";
            txtValor.Text = "";
            txtQuantidade.Text = "";
        }

        public frmEstoque()
        {
            InitializeComponent();
        }

        private void frmEstoque_Load(object sender, EventArgs e)
        {
            desabilitarCampos();
            CarregarComboBox();
        }

        private void btnProduto_Click(object sender, EventArgs e)
        {
            habilitarCampos();
            limparCampos();

            Program.chamadaProdutos = "estoque";
            View.frmProdutos form = new View.frmProdutos();
            form.Show();
        }

        private void frmEstoque_Activated(object sender, EventArgs e)
        {
            txtEstoque.Text = Program.estoqueProduto;
            txtProduto.Text = Program.nomeProduto;
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (txtProduto.Text.ToString().Trim() == "")
            {
                txtProduto.Text = "";
                MessageBox.Show("Selecione um Produto", "Campo vazio", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtProduto.Focus();
                return;
            }
            if (txtQuantidade.Text == "")
            {
                txtQuantidade.Text = "";
                MessageBox.Show("Preencha a quantidade", "Campo vazio", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtQuantidade.Focus();
                return;
            }

            //Codigo do botao editar os produtos
            conexao.AbrirConect();
            sql = "UPDATE produtos SET fornecedor = @fornecedor, valor_compra = @valor_compra, estoque = @estoque where id = @id";
            cmd = new MySqlCommand(sql, conexao.con);
            cmd.Parameters.AddWithValue("@fornecedor",cbFornece.SelectedValue);
            cmd.Parameters.AddWithValue("@valor_compra", txtValor.Text.Replace(",", "."));
            cmd.Parameters.AddWithValue("@estoque", Convert.ToDouble(txtQuantidade.Text) + Convert.ToDouble(txtEstoque.Text));     
            cmd.Parameters.AddWithValue("@id", Program.idProduto);

            cmd.ExecuteNonQuery();
            conexao.FecharConect();

            MessageBox.Show("Lançamento feito com sucesso", "Dados Salvos", MessageBoxButtons.OK, MessageBoxIcon.Information);
            limparCampos();
            desabilitarCampos();
        }
    }
}
