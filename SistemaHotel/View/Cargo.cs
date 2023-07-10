using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemaHotel.View {
    public partial class frmCargo : Form
    {

        Conexao conexao = new Conexao();
        string sql;
        MySqlCommand cmd;
        string id;

        public frmCargo()
        {
            InitializeComponent();
        }

        private void FormatarDG()
        {
            grid.Columns[0].HeaderText = "ID";
            grid.Columns[1].HeaderText = "CARGO";

            grid.Columns[0].Visible = false;
            grid.Columns[1].Width = 200; //Dobrando a largura da coluna CARGO
        }

        private void Listar()
        {
            conexao.AbrirConect();
            sql = "SELECT * FROM cargos order by cargo  asc";
            cmd = new MySqlCommand(sql, conexao.con);
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            DataTable dt = new DataTable();
            da.Fill(dt);
            grid.DataSource = dt;
            conexao.FecharConect();
            FormatarDG();
        }
        private void frmCargo_Load(object sender, EventArgs e)
        {
            Listar();
        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            txtNome.Enabled = true;
            btnSalvar.Enabled = true;
            btnNovo.Enabled = false;
            btnEditar.Enabled = false;
            btnExcluir.Enabled = false;
            txtNome.Focus();
        }

        #region PROGRAMANDO O BOTÃO SALVAR
        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (txtNome.Text.ToString().Trim() == "")
            {
                txtNome.Text = "";
                MessageBox.Show("Preencha o campo Nome", "Campo vazio", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNome.Focus();
                return;
            }

            //PROGRAMANDO O BOTAO SALVAR
            conexao.AbrirConect();
            sql = "INSERT INTO cargos (cargo) VALUES (@cargo)";
            cmd = new MySqlCommand(sql, conexao.con);
            cmd.Parameters.AddWithValue("@cargo", txtNome.Text);
            cmd.ExecuteNonQuery();
            conexao.FecharConect();

            MessageBox.Show("Registro salvo com sucesso", "Dados Salvos", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnNovo.Enabled = true;
            btnSalvar.Enabled = false;
            txtNome.Text = "";
            Listar();
        }
        #endregion

        #region PROGRAMANDO O BOTÃO EDITAR
        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (txtNome.Text.ToString().Trim() == "")
            {
                txtNome.Text = "";
                MessageBox.Show("Preencha o campo Nome", "Campo vazio", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNome.Focus();
                return;
            }

            //PROGRAMANDO O BOTAO EDITAR
            conexao.AbrirConect();
            sql = "UPDATE cargos SET cargo = @cargo where id = @id";
            cmd = new MySqlCommand(sql, conexao.con);
            cmd.Parameters.AddWithValue("@cargo", txtNome.Text);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            conexao.FecharConect();

            MessageBox.Show("Registro salvo com sucesso", "Dados Salvos", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnNovo.Enabled = true;
            btnSalvar.Enabled = false;
            txtNome.Text = "";
            txtNome.Enabled = false;
            Listar();
        }
        #endregion

        #region PROGRAMANDO O BOTÃO EXCLUIR
        private void btnExcluir_Click(object sender, EventArgs e)
        {
            var resultado = MessageBox.Show("Deseja realmente exluir o registro?", "Excluir registro", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resultado == DialogResult.Yes)
            {

                //PROGRAMANDO O BOTAO EXCLUIR
                conexao.AbrirConect();
                sql = "DELETE FROM cargos where id = @id";
                cmd = new MySqlCommand(sql, conexao.con);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                conexao.FecharConect();

                MessageBox.Show("Registro excluido com sucesso", "Registro exluido", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnNovo.Enabled = true;
                btnEditar.Enabled = false;
                btnExcluir.Enabled = false;
                txtNome.Text = "";
                txtNome.Enabled = false;
                Listar();
            }
        }
        #endregion

        private void grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            btnEditar.Enabled = true;
            btnExcluir.Enabled = true;
            btnSalvar.Enabled = false;
            txtNome.Enabled = true;

            id = grid.CurrentRow.Cells[0].Value.ToString();
            txtNome.Text = grid.CurrentRow.Cells[1].Value.ToString();
        }
    }
}
