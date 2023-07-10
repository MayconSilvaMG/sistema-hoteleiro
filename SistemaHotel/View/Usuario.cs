using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace SistemaHotel.View
{
    public partial class frmUsuario : Form
    {
        Conexao conexao = new Conexao();
        string sql;
        MySqlCommand cmd;
        string id;

        string usuarioAntigo;

        public frmUsuario()
        {
            InitializeComponent();
        }

        private void FormatarDG()
        {
            grid.Columns[0].HeaderText = "ID";
            grid.Columns[1].HeaderText = "Nome";
            grid.Columns[2].HeaderText = "Cargo";
            grid.Columns[3].HeaderText = "Usuario";
            grid.Columns[4].HeaderText = "Senha";
            grid.Columns[5].HeaderText = "Data";

            grid.Columns[0].Visible = false;
        }

        private void Listar()
        {
            conexao.AbrirConect();
            sql = "SELECT * FROM usuarios order by nome asc";
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
            sql = "SELECT * FROM usuarios where nome LIKE @nome order by nome asc";
            cmd = new MySqlCommand(sql, conexao.con);
            cmd.Parameters.AddWithValue("@nome", txtBuscarNome.Text + "%");
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
            txtUsuario.Enabled = true;
            txtSenha.Enabled = true;
            cbCargo.Enabled = true;
            txtNome.Focus();
        }

        private void desabilitarCampos()
        {
            txtNome.Enabled = false;
            txtUsuario.Enabled = false;
            txtSenha.Enabled = false;    
        }

        private void CarregarComboBox()
        {
            conexao.AbrirConect();
            sql = "SELECT * FROM cargos order by cargo asc";
            cmd = new MySqlCommand(sql, conexao.con);
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            DataTable dt = new DataTable();
            da.Fill(dt);
            cbCargo.DataSource = dt;
            //cbCargo.ValueMember = "id";
            cbCargo.DisplayMember = "cargo";
            conexao.FecharConect();
        }

        private void limparCampos()
        {
            txtNome.Text = "";
            txtUsuario.Text = "";
            txtSenha.Text = "";
        }

        private void frmUsuario_Load(object sender, EventArgs e)
        {
            Listar();     
            CarregarComboBox();
        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            if (cbCargo.Text == "")
            {
                MessageBox.Show("Cadastre um cargo!!");
                View.frmCargo form = new View.frmCargo();
                form.Show();
                Close();
            }

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
            if (txtSenha.Text == "")
            {
                txtSenha.Text = "";
                MessageBox.Show("Preencha o campo Senha", "Campo vazio", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtSenha.Focus();
                return;
            }
            if (txtUsuario.Text == "")
            {
                txtUsuario.Text = "";
                MessageBox.Show("Preencha o campo Usuario", "Campo vazio", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtSenha.Focus();
                return;
            }
            conexao.AbrirConect();
            sql = "INSERT INTO usuarios (nome, cargo, usuario, senha, data) VALUES (@nome, @cargo, @usuario, @senha, curDate() )";
            cmd = new MySqlCommand(sql, conexao.con);
            cmd.Parameters.AddWithValue("@nome", txtNome.Text);
            cmd.Parameters.AddWithValue("@cargo", cbCargo.Text);
            cmd.Parameters.AddWithValue("@usuario", txtUsuario.Text);
            cmd.Parameters.AddWithValue("@senha", txtSenha.Text);

            //VERIFICAR SE O USUARIO JA EXISTE NO BANCO
            MySqlCommand cmdVerificar;
            cmdVerificar = new MySqlCommand("SELECT * FROM usuarios where usuario = @usuario", conexao.con);
            cmdVerificar.Parameters.AddWithValue("@usuario", txtUsuario.Text);
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmdVerificar;
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                MessageBox.Show("Esse usuario já existe!!", "Usuario já existe", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtUsuario.Text = "";
                txtUsuario.Focus();
                return;
            }
            FormatarDG();
            cmd.ExecuteNonQuery();
            conexao.FecharConect();

            MessageBox.Show("Registro salvo com sucesso", "Dados Salvos", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnNovo.Enabled = true;
            btnSalvar.Enabled = false;
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
                sql = "DELETE FROM usuarios where id = @id";
                cmd = new MySqlCommand(sql, conexao.con);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                conexao.FecharConect();

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
            sql = "UPDATE usuarios SET nome = @nome, cargo = @cargo, usuario = @usuario, senha = @senha where id = @id";
            cmd = new MySqlCommand(sql, conexao.con);
            cmd.Parameters.AddWithValue("@nome", txtNome.Text);
            cmd.Parameters.AddWithValue("@cargo", cbCargo.Text);
            cmd.Parameters.AddWithValue("@usuario", txtUsuario.Text);
            cmd.Parameters.AddWithValue("@senha", txtSenha.Text);
            cmd.Parameters.AddWithValue("@id", id);


            //VERIFICAR SE O USUARIO JA EXISTE NO BANCO
            if (txtUsuario.Text != usuarioAntigo)
            {
                MySqlCommand cmdVerificar;
                cmdVerificar = new MySqlCommand("SELECT * FROM usuarios where usuario = @usuario", conexao.con);
                cmdVerificar.Parameters.AddWithValue("@usuario", txtUsuario.Text);
                MySqlDataAdapter da = new MySqlDataAdapter();
                da.SelectCommand = cmdVerificar;
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show("Usuario já registrado!!", "Usuario já registrado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtUsuario.Text = "";
                    txtUsuario.Focus();
                    return;
                }
            }

            cmd.ExecuteNonQuery();
            //conexao.FecharConect();

            MessageBox.Show("Registro salvo com sucesso", "Dados Salvos", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnNovo.Enabled = true;
            btnSalvar.Enabled = false;
            txtNome.Text = "";
            txtNome.Enabled = false;
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
            cbCargo.Text = grid.CurrentRow.Cells[2].Value.ToString();
            txtUsuario.Text = grid.CurrentRow.Cells[3].Value.ToString();
            txtSenha.Text = grid.CurrentRow.Cells[4].Value.ToString();

            usuarioAntigo = grid.CurrentRow.Cells[3].Value.ToString();
        }

        private void txtBuscarNome_TextChanged(object sender, EventArgs e)
        {
            BuscarNome();
        }
    }
}