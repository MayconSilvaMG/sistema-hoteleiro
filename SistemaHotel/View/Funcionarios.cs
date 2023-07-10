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
using System.Text.RegularExpressions;

namespace SistemaHotel.View
{

    public partial class frmFuncionarios : Form
    {
        Conexao conexao = new Conexao();
        string sql;
        MySqlCommand cmd;
        string id;

        string CpfAntigo;

        public frmFuncionarios()
        {
            InitializeComponent();
        }

        private void FormatarDG()
        {
            grid.Columns[0].HeaderText = "ID";
            grid.Columns[1].HeaderText = "Nome";
            grid.Columns[2].HeaderText = "CPF";
            grid.Columns[3].HeaderText = "Endereço";
            grid.Columns[4].HeaderText = "Telefone";
            grid.Columns[5].HeaderText = "Cargo";
            grid.Columns[6].HeaderText = "Data";

            grid.Columns[0].Visible = false;
        }

        //private void Listar()
        //{
        //    conexao.AbrirConect();
        //    sql = "SELECT * FROM funcionarios order by nome asc";
        //    cmd = new MySqlCommand(sql, conexao.con);
        //    MySqlDataAdapter da = new MySqlDataAdapter();
        //    da.SelectCommand = cmd;
        //    DataTable dt = new DataTable();
        //    da.Fill(dt);
        //    grid.DataSource = dt;
        //    conexao.FecharConect();
        //    FormatarDG();
        //}

        //private void BuscarCpf()
        //{
        //    conexao.AbrirConect();
        //    sql = "SELECT * FROM funcionarios where cpf = @cpf order by nome asc";
        //    cmd = new MySqlCommand(sql, conexao.con);
        //    cmd.Parameters.AddWithValue("@cpf", txtBuscarCpf.Text);
        //    MySqlDataAdapter da = new MySqlDataAdapter();
        //    da.SelectCommand = cmd;
        //    DataTable dt = new DataTable();
        //    da.Fill(dt);
        //    grid.DataSource = dt;
        //    conexao.FecharConect();
        //    FormatarDG();
        //}

        //private void BuscarNome()
        //{
        //    conexao.AbrirConect();
        //    sql = "SELECT * FROM funcionarios where nome LIKE @nome order by nome asc";
        //    cmd = new MySqlCommand(sql, conexao.con);
        //    cmd.Parameters.AddWithValue("@nome", txtBuscarNome.Text + "%");
        //    MySqlDataAdapter da = new MySqlDataAdapter();
        //    da.SelectCommand = cmd;
        //    DataTable dt = new DataTable();
        //    da.Fill(dt);
        //    grid.DataSource = dt;
        //    conexao.FecharConect();
        //    FormatarDG();
        //}

        //private void CarregarComboBox()
        //{
        //    conexao.AbrirConect();
        //    sql = "SELECT * FROM cargos order by cargo asc";
        //    cmd = new MySqlCommand(sql, conexao.con);
        //    MySqlDataAdapter da = new MySqlDataAdapter();
        //    da.SelectCommand = cmd;
        //    DataTable dt = new DataTable();
        //    da.Fill(dt);
        //    cbCargo.DataSource = dt;
        //    //cbCargo.ValueMember = "id";
        //    cbCargo.DisplayMember = "cargo";
        //    conexao.FecharConect();
        //}

        private void habilitarCampos()
        {
            txtNome.Enabled = true;
            txtCpf.Enabled = true;
            txtEndereco.Enabled = true;
            cbCargo.Enabled = true;
            txtTelefone.Enabled = true;
            txtNome.Focus();

        }

        private void desabilitarCampos()
        {
            txtNome.Enabled = false;
            txtCpf.Enabled = false;
            txtEndereco.Enabled = false;
            cbCargo.Enabled = false;
            txtTelefone.Enabled = false;
        }

        private void limparCampos()
        {
            txtNome.Text = "";
            txtCpf.Text = "";
            txtEndereco.Text = "";
            txtTelefone.Text = "";
        }

        private void frmFuncionarios_Load(object sender, EventArgs e)
        {
            //Listar();
            rbNome.Checked = true;
            //CarregarComboBox();
        }

        private void rbNome_CheckedChanged(object sender, EventArgs e)
        {
            txtBuscarNome.Visible = true;
            txtBuscarCpf.Visible = false;

            txtBuscarNome.Text = "";
            txtBuscarCpf.Text = "";
        }

        private void rbCpf_CheckedChanged(object sender, EventArgs e)
        {
            txtBuscarNome.Visible = false;
            txtBuscarCpf.Visible = true;

            txtBuscarNome.Text = "";
            txtBuscarCpf.Text = "";
        }

        #region CODIGO PARA BOTAO NOVO
        private void btnNovo_Click(object sender, EventArgs e)
        {
           
                MessageBox.Show("Preencha o Telefone", "Campo vazio", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNome.Focus();
                return;
            
            //MessageBox.Show("Registro salvo com sucesso", "Dados Salvos", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //btnNovo.Enabled = true;
            //btnSalvar.Enabled = false;
            //limparCampos();
            //desabilitarCampos();
            //if (cbCargo.Text == "")
            //{
            //    MessageBox.Show("Cadastre um cargo!!");
            //    View.frmCargo form = new View.frmCargo();
            //    form.Show();
            //    Close();
            //}

            //habilitarCampos();
            //btnSalvar.Enabled = true;
            //btnNovo.Enabled = false;
            //btnEditar.Enabled = false;
            //btnExcluir.Enabled = false;

        }
        #endregion

        #region CODIGO PARA BOTAO SALVAR
        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (txtNome.Text.ToString().Trim() == "")
            {
                txtNome.Text = "";
                MessageBox.Show("Preencha o campo Nome", "Campo vazio", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNome.Focus();
                return;
            }
            if (txtCpf.Text == "   .   .   -")
            {
                txtCpf.Text = "";
                MessageBox.Show("Preencha o campo CPF", "Campo vazio", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtCpf.Focus();
                return;
            }
            Regex validacao = new Regex(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$");
            if (!validacao.IsMatch(txtCpf.Text))
            {
                MessageBox.Show("Insira CPF corretamente!!", "CPF Errado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Regex valiTelefone = new Regex(@"^\(\d{2}\)\d{4}\-\d{4}$");
            if (!validacao.IsMatch(txtTelefone.Text))
            {
                MessageBox.Show("Insira Telefone corretamente!!", "Tel Errado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            conexao.AbrirConect();
            sql = "INSERT INTO funcionarios (nome, cpf, endereco, telefone, cargo, data) VALUES (@nome, @cpf, @endereco, @telefone, @cargo, curDate() )";
            cmd = new MySqlCommand(sql, conexao.con);
            cmd.Parameters.AddWithValue("@nome", txtNome.Text);
            cmd.Parameters.AddWithValue("@cpf", txtCpf.Text);
            cmd.Parameters.AddWithValue("@endereco", txtEndereco.Text);
            cmd.Parameters.AddWithValue("@telefone", txtTelefone.Text);
            cmd.Parameters.AddWithValue("@cargo", cbCargo.Text);

            //VERIFICAR SE O CPF JA EXISTE NO BANCO
            MySqlCommand cmdVerificar;
            cmdVerificar = new MySqlCommand("SELECT * FROM funcionarios where cpf = @cpf", conexao.con);
            cmdVerificar.Parameters.AddWithValue("@cpf", txtCpf.Text);
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmdVerificar;
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                MessageBox.Show("CPF já registrado!!", "CPF já registrado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtCpf.Text = "";
                txtCpf.Focus();
                return;
            }

            cmd.ExecuteNonQuery();
            conexao.FecharConect();

            MessageBox.Show("Registro salvo com sucesso", "Dados Salvos", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnNovo.Enabled = true;
            btnSalvar.Enabled = false;
            limparCampos();
            desabilitarCampos();
            //Listar();
        }
        #endregion

        #region CODIGO PARA BOTAO EDITAR
        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (txtNome.Text.ToString().Trim() == "")
            {
                txtNome.Text = "";
                MessageBox.Show("Preencha o campo Nome", "Campo vazio", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNome.Focus();
                return;
            }
            if (txtCpf.Text == "   .   .   -")
            {
                txtCpf.Text = "";
                MessageBox.Show("Preencha o campo CPF", "Campo vazio", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtCpf.Focus();
                return;
            }
            conexao.AbrirConect();
            sql = "UPDATE funcionarios SET nome = @nome, cpf = @cpf, endereco = @endereco, telefone = @telefone, cargo = @cargo  where id = @id";
            cmd = new MySqlCommand(sql, conexao.con);
            cmd.Parameters.AddWithValue("@nome", txtNome.Text);
            cmd.Parameters.AddWithValue("@cpf", txtCpf.Text);
            cmd.Parameters.AddWithValue("@endereco", txtEndereco.Text);
            cmd.Parameters.AddWithValue("@telefone", txtTelefone.Text);
            cmd.Parameters.AddWithValue("@cargo", cbCargo.Text);
            cmd.Parameters.AddWithValue("@id", id);

            //VERIFICAR SE O CPF JA EXISTE NO BANCO
            if(txtCpf.Text != CpfAntigo)
            {
                MySqlCommand cmdVerificar;
                cmdVerificar = new MySqlCommand("SELECT * FROM funcionarios where cpf = @cpf", conexao.con);
                cmdVerificar.Parameters.AddWithValue("@cpf", txtCpf.Text);
                MySqlDataAdapter da = new MySqlDataAdapter();
                da.SelectCommand = cmdVerificar;
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show("CPF já registrado!!", "CPF já registrado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtCpf.Text = "";
                    txtCpf.Focus();
                    return;
                }
            }

            cmd.ExecuteNonQuery();
            conexao.FecharConect();

            MessageBox.Show("Registro salvo com sucesso", "Dados Salvos", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnNovo.Enabled = true;
            btnSalvar.Enabled = false;
            txtNome.Text = "";
            txtNome.Enabled = false;
            limparCampos();
            desabilitarCampos();
            //Listar();
        }
        #endregion

        #region CODIGO PARA BOTAO EXCLUIR
        private void btnExcluir_Click_1(object sender, EventArgs e)
        {
            var resultado = MessageBox.Show("Deseja realmente exluir o registro?", "Excluir registro", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resultado == DialogResult.Yes)
            {

                conexao.AbrirConect();
                sql = "DELETE FROM funcionarios where id = @id";
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
                limparCampos();
                desabilitarCampos();
                //Listar();
            }
        }
        #endregion

        private void grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            btnEditar.Enabled = true;
            btnExcluir.Enabled = true;
            btnSalvar.Enabled = false;
            habilitarCampos();

            id = grid.CurrentRow.Cells[0].Value.ToString();
            txtNome.Text = grid.CurrentRow.Cells[1].Value.ToString();
            txtCpf.Text = grid.CurrentRow.Cells[2].Value.ToString();
            txtEndereco.Text = grid.CurrentRow.Cells[3].Value.ToString();
            txtTelefone.Text = grid.CurrentRow.Cells[4].Value.ToString();
            cbCargo.Text = grid.CurrentRow.Cells[5].Value.ToString();

            CpfAntigo = grid.CurrentRow.Cells[2].Value.ToString();
        }

        private void txtBuscarNome_TextChanged(object sender, EventArgs e)
        {
            //BuscarNome();
        }

        private void txtBuscarCpf_TextChanged(object sender, EventArgs e)
        {
            if (txtBuscarCpf.Text == "   .   .   -")
            {
                //Listar();
            }
            else { 
                //BuscarCpf();
            }
        }

        private void txtNome_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtCpf_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }
    }
}