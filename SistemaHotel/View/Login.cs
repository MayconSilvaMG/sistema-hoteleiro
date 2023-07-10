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

namespace SistemaHotel
{
    public partial class frmLogin : Form
    {
        Conexao conexao = new Conexao();

        public frmLogin() //Construtor 
        {
            InitializeComponent();
            pnlLogin.Visible = false;
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            //Centralizar o Painel
            pnlLogin.Location = new Point(this.Width / 2 - 166, this.Height / 2 - 170);

            //Exibir painel depois de carregar as informações acima
            pnlLogin.Visible = true ;

            //Mudar a cor quando passar o mouse em cima
            btnLogin.FlatAppearance.MouseOverBackColor = Color.FromArgb(21, 114, 160);
            
            //Mudar a cor de quando clica no botão
            btnLogin.FlatAppearance.MouseDownBackColor = Color.FromArgb(8, 72, 103);
        }

        private void pnlLogin_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            Application.Run(new frmMenu());
            //ChamarLogin();//Chamando o método ChamarLogin
        }

        private void frmLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                ChamarLogin();//Chamando o método ChamarLogin
            }
        }

        public void ChamarLogin()
        {
            //Condição dupla de validação para usuário //Utilização dos validadores ||(OU)
            /* 
             * if (txtUsuario.Text == "" || txtSenha.Text == "")
            {
                MessageBox.Show("Preencha o usuário"); //Caixa de mensagem
                txtUsuario.Focus(); //txtUsuario volta a receber o cursor do mouse 
                return;            
            } */

            //Ou fazer 1 condição pra cada
            if (txtUsuario.Text.ToString().Trim() == "")
            {
                MessageBox.Show("Preencha o campo usuário", "Campo vazio", MessageBoxButtons.OK, MessageBoxIcon.Information); //Caixa de mensagem
                txtUsuario.Focus(); //txtUsuario volta a receber o cursor do mouse 
                return;
            } 
            if (txtSenha.Text.ToString().Trim() == "")
            {
                MessageBox.Show("Preencha o campo senha" ,"Campo vazio", MessageBoxButtons.OK,MessageBoxIcon.Information); //Caixa de mensagem
                txtSenha.Focus(); //txtUsuario volta a receber o cursor do mouse 
                return;
            }

            //Código para o LOGIN
            MySqlCommand cmdVerificar;
            MySqlDataReader reader;

            conexao.AbrirConect();
            cmdVerificar = new MySqlCommand("SELECT * FROM usuarios where usuario = @usuario and senha = @senha", conexao.con);
            cmdVerificar.Parameters.AddWithValue("@usuario", txtUsuario.Text);
            cmdVerificar.Parameters.AddWithValue("@senha", txtSenha.Text);
            reader = cmdVerificar.ExecuteReader();
            
            if (reader.HasRows)
            {
                //Extraindo informações da consulta do login
                while (reader.Read())
                {
                    Program.nomeUsuario = Convert.ToString(reader["nome"]);
                    Program.cargoUsuario = Convert.ToString(reader["cargo"]);

                }

                MessageBox.Show("Bem Vindo!! " + Program.nomeUsuario, "Login efetuado", MessageBoxButtons.OK, MessageBoxIcon.Information);

                //Inicializando o formulario
                frmMenu form = new frmMenu(); //Instanciando o objeto, só instancia quando não está na classe dele
                Limpar();
                form.Show();//Pegando a propriedade de exibição e exibindo e pedindo pra abrir frmMenu
            }
            else
            {
                MessageBox.Show("Usuario ou senha não encontrado", "Dados incorretos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtUsuario.Text = "";
                txtUsuario.Focus();
                txtSenha.Text = "";
            }
            //conexao.FecharConect();
        }

        private void Limpar()
        {
            //Limpando os campos de login assim que entrar na tela menu
            txtUsuario.Text = "";
            txtSenha.Text = "";
            //voltando o foco para o campo usuario
            txtUsuario.Focus();
        }

        private void frmLogin_Resize(object sender, EventArgs e)
        {
            //Redimencionando o painel caso e tela minimize
            pnlLogin.Location = new Point(this.Width / 2 - 166, this.Height / 2 - 170);
        }
    }
}
