using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace SistemaHotel.View
{
    public partial class frmServicos : Form
    {
        Conexao conexao = new Conexao();
        string sql;
        MySqlCommand cmd;
        string id;

        private void FormatarDG()
        {
            grid.Columns[0].HeaderText = "ID";
            grid.Columns[1].HeaderText = "Servico";
            grid.Columns[2].HeaderText = "Valor";

            grid.Columns[2].DefaultCellStyle.Format = "C2";

            grid.Columns[0].Visible = false;
        }

        private void Listar()
        {
            conexao.AbrirConect();
            sql = "SELECT * FROM servicos order by nome asc";
            cmd = new MySqlCommand(sql, conexao.con);
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            DataTable dt = new DataTable();
            da.Fill(dt);
            grid.DataSource = dt;
            conexao.FecharConect();
            FormatarDG();
        }

        private void habilitarCampos()
        {
            txtNome.Enabled = true;
            txtValor.Enabled = true;
            txtNome.Focus();
        }

        private void desabilitarCampos()
        {
            txtNome.Enabled = false;
            txtValor.Enabled= false;
        }

        private void limparCampos()
        {
            txtNome.Text = "";
            txtValor.Text = "";
        }

        public frmServicos()
        {
            InitializeComponent();
        }

        private void frmServicos_Load(object sender, EventArgs e)
        {
            Listar();
        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            limparCampos();
            habilitarCampos();
            btnSalvar.Enabled = true;
            btnNovo.Enabled = false;
            btnEditar.Enabled = false;
            btnExcluir.Enabled = false;
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (txtNome.Text.ToString().Trim() == "")
            {
                txtNome.Text = "";
                MessageBox.Show("Preencha o campo Nome", "Campo vazio", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNome.Focus();
                return;
            }
            conexao.AbrirConect();
            sql = "INSERT INTO servicos (nome, valor) VALUES (@nome, @valor)";
            cmd = new MySqlCommand(sql, conexao.con);
            cmd.Parameters.AddWithValue("@nome", txtNome.Text);
            cmd.Parameters.AddWithValue("@valor", txtValor.Text.Replace(",",".")); 
            
            cmd.ExecuteNonQuery();
            conexao.FecharConect();

            MessageBox.Show("Registro salvo com sucesso", "Dados Salvos", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnNovo.Enabled = true;
            btnSalvar.Enabled = false;
            limparCampos();
            desabilitarCampos();
            Listar();
        }

        private void grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            btnEditar.Enabled = true;
            btnExcluir.Enabled = true;
            btnSalvar.Enabled = false;
            habilitarCampos();

            id = grid.CurrentRow.Cells[0].Value.ToString();
            txtNome.Text = grid.CurrentRow.Cells[1].Value.ToString();
            txtValor.Text = grid.CurrentRow.Cells[2].Value.ToString();

        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (txtNome.Text.ToString().Trim() == "")
            {
                txtNome.Text = "";
                MessageBox.Show("Preencha o campo Nome", "Campo vazio", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNome.Focus();
                return;
            }

            conexao.AbrirConect();
            sql = "UPDATE servicos SET nome = @nome, valor = @valor where id = @id";
            cmd = new MySqlCommand(sql, conexao.con);
            cmd.Parameters.AddWithValue("@nome", txtNome.Text);
            cmd.Parameters.AddWithValue("@valor", txtValor.Text);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
            conexao.FecharConect();

            MessageBox.Show("Registro editado com sucesso", "Dados Salvos", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnNovo.Enabled = true;
            btnSalvar.Enabled = false;
            txtNome.Text = "";
            txtNome.Enabled = false;
            limparCampos();
            desabilitarCampos();
            Listar();
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            var resultado = MessageBox.Show("Deseja realmente exluir o registro?", "Excluir registro", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resultado == DialogResult.Yes)
            {

                conexao.AbrirConect();
                sql = "DELETE FROM servicos where id = @id";
                cmd = new MySqlCommand(sql, conexao.con);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                //onexao.FecharConect();

                //Codigo do botao excluir
                MessageBox.Show("Registro excluido com sucesso", "Registro exluido", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnNovo.Enabled = true;
                btnEditar.Enabled = false;
                btnExcluir.Enabled = false;
                txtNome.Text = "";
                txtNome.Enabled = false;
                Listar();
                limparCampos();
            }
        }
    }
}