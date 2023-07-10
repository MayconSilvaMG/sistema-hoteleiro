using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace SistemaHotel.View {
    public partial class frmProdutos : Form {

        Conexao conexao = new Conexao();
        string sql;
        MySqlCommand cmd;
        string id;

        public frmProdutos()
        {
            InitializeComponent();
        }

        private void FormatarDG()
        {
            grid.Columns[0].HeaderText = "ID";
            grid.Columns[1].HeaderText = "Nome";
            grid.Columns[2].HeaderText = "Descricao";
            grid.Columns[3].HeaderText = "Estoque";
            grid.Columns[4].HeaderText = "Fornecedor";
            grid.Columns[5].HeaderText = "Valor Venda";
            grid.Columns[6].HeaderText = "Valor Compra";
            grid.Columns[7].HeaderText = "Data";
            grid.Columns[8].HeaderText = "Imagem";
            grid.Columns[9].HeaderText = "Id Fornecedor";

            //formatar coluna para moeda
            grid.Columns[5].DefaultCellStyle.Format = "C2";
            grid.Columns[6].DefaultCellStyle.Format = "C2";

            grid.Columns[0].Visible = false;
            grid.Columns[8].Visible = false;
            grid.Columns[9].Visible = false;
        }

        private void Listar()
        {
            conexao.AbrirConect();
            sql = "SELECT pro.id, pro.nome, pro.descricao, pro.estoque, forn.nome, pro.valor_venda, pro.valor_compra, pro.data, pro.imagem, pro.fornecedor FROM produtos as pro INNER JOIN fornecedores as forn ON pro.fornecedor = forn.id order by pro.nome asc";
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
            sql = "SELECT pro.id, pro.nome, pro.descricao, pro.estoque, forn.nome, pro.valor_venda, pro.valor_compra, pro.data, pro.imagem, pro.fornecedor FROM produtos as pro INNER JOIN fornecedores as forn ON pro.fornecedor = forn.id where pro.nome LIKE @nome order by pro.nome asc";
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

        private void CarregarComboBox()
        {
            conexao.AbrirConect();
            sql = "SELECT * FROM fornecedores order by nome asc";
            cmd = new MySqlCommand(sql, conexao.con);
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            DataTable dt = new DataTable();
            da.Fill(dt);
            cbCargo.DataSource = dt;
            cbCargo.ValueMember = "id";
            cbCargo.DisplayMember = "nome";
            conexao.FecharConect();
        }

        private void habilitarCampos() {
            txtNome.Enabled = true;
            txtDescricao.Enabled = true;
            txtValor.Enabled = true;
            cbCargo.Enabled = true;
            btnBuscarImagem.Enabled = true;
            txtNome.Focus();

        }

        private void desabilitarCampos() {
            txtNome.Enabled = false;
            txtDescricao.Enabled = false;
            txtValor.Enabled = false;
            cbFornecedor.Enabled = false;
            txtEstoque.Enabled = false;
            btnBuscarImagem.Enabled = false;
        }

        private void limparCampos() {
            txtNome.Text = "";
            txtDescricao.Text = "";
            txtValor.Text = "";
            txtEstoque.Text = "";
            LimparImagem();
        }

        private void LimparImagem() {
            img.Image = Properties.Resources.sem_foto;
        }

        private void frmProdutos_Load(object sender, EventArgs e) {

            CarregarComboBox();
            Listar();
            LimparImagem();
        } 

        private void btnNovo_Click(object sender, EventArgs e) {
            if (cbCargo.Text == "")
            {
                MessageBox.Show("Cadastre um Fornecedor!!");
                Close();
            }

            habilitarCampos();
            btnSalvar.Enabled = true;
            btnNovo.Enabled = false;
            btnEditar.Enabled = false;
            btnExcluir.Enabled = false;
        }

        private void btnSalvar_Click(object sender, EventArgs e) {
            if (txtNome.Text.ToString().Trim() == "")
            {
                txtNome.Text = "";
                MessageBox.Show("Preencha o campo Nome", "Campo vazio", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNome.Focus();
                return;
            }
            conexao.AbrirConect();
            sql = "INSERT INTO produtos (nome, descricao, estoque, fornecedor, valor_venda, valor_compra, data) VALUES (@nome, @descricao, @estoque, @fornecedor, @valor_venda, @valor_compra, curDate() )";
            cmd = new MySqlCommand(sql, conexao.con);
            cmd.Parameters.AddWithValue("@nome", txtNome.Text);
            cmd.Parameters.AddWithValue("@descricao", txtDescricao.Text);
            cmd.Parameters.AddWithValue("@estoque", 0);
            cmd.Parameters.AddWithValue("@fornecedor", cbCargo.SelectedValue );
            cmd.Parameters.AddWithValue("@valor_venda", txtValor.Text.Replace(",","."));
            cmd.Parameters.AddWithValue("@valor_compra", 0);

            cmd.ExecuteNonQuery();
            conexao.FecharConect();

            MessageBox.Show("Produto salvo com sucesso", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnNovo.Enabled = true;
            btnSalvar.Enabled = false;
            limparCampos();
            desabilitarCampos();
            Listar();
        }

        private void btnEditar_Click(object sender, EventArgs e) {
            if (txtNome.Text.ToString().Trim() == "") {
                txtNome.Text = "";
                MessageBox.Show("Preencha o campo Nome", "Campo vazio", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNome.Focus();
                return;
            }
            if (txtValor.Text == "") {
                txtValor.Text = "";
                MessageBox.Show("Preencha o Valor", "Campo vazio", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtValor.Focus();
                return;
            }

            //Codigo do botao editar
            conexao.AbrirConect();
            sql = "UPDATE produtos SET nome = @nome, descricao = @descricao, fornecedor = @fornecedor, valor_venda = @valor_venda where id = @id";
            cmd = new MySqlCommand(sql, conexao.con);
            cmd.Parameters.AddWithValue("@nome", txtNome.Text);
            cmd.Parameters.AddWithValue("@descricao", txtDescricao.Text);
            cmd.Parameters.AddWithValue("@fornecedor", cbCargo.SelectedValue);
            cmd.Parameters.AddWithValue("@valor_venda", txtValor.Text.Replace(",","."));
            cmd.Parameters.AddWithValue("@id", id);
            
            cmd.ExecuteNonQuery();
            conexao.FecharConect();

            MessageBox.Show("Registro salvo com sucesso", "Dados Salvos", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnNovo.Enabled = true;
            btnEditar.Enabled = false;
            btnExcluir.Enabled = false;
            limparCampos();
            desabilitarCampos();
            Listar();
        }

        private void btnExcluir_Click(object sender, EventArgs e) {
            var resultado = MessageBox.Show("Deseja realmente exluir o registro?", "Excluir registro", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resultado == DialogResult.Yes) {

                //Codigo do botao excluir
                conexao.AbrirConect();
                sql = "DELETE FROM produtos where id = @id";
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
                limparCampos();
                desabilitarCampos();
            }
        }

        private void btnBuscarImagem_Click(object sender, EventArgs e) {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Arquivo de imagens(*.jpg;*.png)|*.jpg;*.png|Todos os arquivos(*.*)|*.*";
            if (dialog.ShowDialog() == DialogResult.OK) {
                string foto = dialog.FileName.ToString();
                img.ImageLocation = foto;
            }
        }

        private void grid_Click(object sender, EventArgs e)
        {
            btnEditar.Enabled = true;
            btnExcluir.Enabled = true;
            btnSalvar.Enabled = false;
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            BuscarNome();
        }

        private void grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            btnEditar.Enabled = true;
            btnExcluir.Enabled = true;
            btnSalvar.Enabled = false;
            habilitarCampos();

            id = grid.CurrentRow.Cells[0].Value.ToString();
            txtNome.Text = grid.CurrentRow.Cells[1].Value.ToString();
            txtDescricao.Text = grid.CurrentRow.Cells[2].Value.ToString();
            txtEstoque.Text = grid.CurrentRow.Cells[3].Value.ToString();
            cbCargo.Text = grid.CurrentRow.Cells[4].Value.ToString();
            txtValor.Text = grid.CurrentRow.Cells[5].Value.ToString();      
        }

        private void grid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if(Program.chamadaProdutos == "estoque")
            {
                Program.nomeProduto = grid.CurrentRow.Cells[1].Value.ToString();
                Program.estoqueProduto = grid.CurrentRow.Cells[3].Value.ToString();
                Program.idProduto = grid.CurrentRow.Cells[0].Value.ToString();
                Close();
            }
        }
    }
}