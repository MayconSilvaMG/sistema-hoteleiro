using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemaHotel.View
{
    public partial class frmFornecedores : Form
    {
        Conexao conexao = new Conexao();
        string sql;
        MySqlCommand cmd;
        string id;

        private void FormatarDG()
        {
            grid.Columns[0].HeaderText = "ID";
            grid.Columns[1].HeaderText = "Nome";
            grid.Columns[2].HeaderText = "Endereço";
            grid.Columns[3].HeaderText = "Telefone";

            grid.Columns[0].Visible = false;
        }

        private void Listar()
        {
            conexao.AbrirConect();
            sql = "SELECT * FROM fornecedores order by nome asc";
            cmd = new MySqlCommand(sql, conexao.con);
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            DataTable dt = new DataTable();
            da.Fill(dt);
            grid.DataSource = dt;
            conexao.FecharConect();
            FormatarDG();
        }

        private void BuscarNome()
        {
            conexao.AbrirConect();
            sql = "SELECT * FROM fornecedores where nome LIKE @nome order by nome asc";
            cmd = new MySqlCommand(sql, conexao.con);
            cmd.Parameters.AddWithValue("@nome", txtBuscar.Text + "%");
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
            txtEndereco.Enabled = true;
            txtTelefone.Enabled = true;
            txtNome.Focus();

        }

        private void desabilitarCampos()
        {
            txtNome.Enabled = false;
            txtEndereco.Enabled = false;
            txtTelefone.Enabled = false;
        }

        private void limparCampos()
        {
            txtNome.Text = "";
            txtEndereco.Text = "";
            txtTelefone.Text = "";
        }

        public frmFornecedores()
        {
            InitializeComponent();
        }

        private void frmFornecedores_Load(object sender, EventArgs e)
        {
            Listar();
        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
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
            Regex valiTelefone = new Regex(@"^\(\d{2}\)\d{4}-\d{4}$");
            if (!valiTelefone.IsMatch(txtTelefone.Text))
            {
                MessageBox.Show("Insira Telefone corretamente!!", "Tel Errado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtTelefone.Text = "";
                txtTelefone.Focus();
                return;
            }
            conexao.AbrirConect();
            sql = "INSERT INTO fornecedores (nome, endereco, telefone) VALUES (@nome, @endereco, @telefone)";
            cmd = new MySqlCommand(sql, conexao.con);
            cmd.Parameters.AddWithValue("@nome", txtNome.Text);
            cmd.Parameters.AddWithValue("@endereco", txtEndereco.Text);
            cmd.Parameters.AddWithValue("@telefone", txtTelefone.Text);

            cmd.ExecuteNonQuery();
            conexao.FecharConect();

            MessageBox.Show("Registro salvo com sucesso", "Dados Salvos", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnNovo.Enabled = true;
            btnSalvar.Enabled = false;
            limparCampos();
            desabilitarCampos();
            Listar();
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
            sql = "UPDATE fornecedores SET nome = @nome, endereco = @endereco, telefone = @telefone where id = @id";
            cmd = new MySqlCommand(sql, conexao.con);
            cmd.Parameters.AddWithValue("@nome", txtNome.Text);
            cmd.Parameters.AddWithValue("@endereco", txtEndereco.Text);
            cmd.Parameters.AddWithValue("@telefone", txtTelefone.Text);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
            conexao.FecharConect();

            MessageBox.Show("Registro salvo com sucesso", "Dados Salvos", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                sql = "DELETE FROM fornecedores where id = @id";
                cmd = new MySqlCommand(sql, conexao.con);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                //conexao.FecharConect();

                //Codigo do botao excluir
                MessageBox.Show("Registro excluido com sucesso", "Registro exluido", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnNovo.Enabled = true;
                btnEditar.Enabled = false;
                btnExcluir.Enabled = false;
                limparCampos();
                desabilitarCampos();
                Listar();
            }
        }

        private void grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            btnEditar.Enabled = true;
            btnExcluir.Enabled = true;
            btnSalvar.Enabled = false;
            habilitarCampos();

            id = grid.CurrentRow.Cells[0].Value.ToString();
            txtNome.Text = grid.CurrentRow.Cells[1].Value.ToString();          
            txtEndereco.Text = grid.CurrentRow.Cells[2].Value.ToString();
            txtTelefone.Text = grid.CurrentRow.Cells[3].Value.ToString();
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            BuscarNome();
        }
    }
}