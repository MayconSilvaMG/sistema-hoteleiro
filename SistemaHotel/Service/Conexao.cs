using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaHotel
{
    internal class Conexao
    {
        //CONEXAO COM O SERVIDOR
        //string conect = "SERVER=testeconexao.mysql.database.azure.com; DATABASE=hotel; UID=MaycaoTI@testeconexao; PWD=Vaisefoder12@; PORT=3306";
        
        //CONEXAO COM O BANCO DE DADOS LOCAL
        string conect = "SERVER=localhost; DATABASE=hotel; UID=root; PWD=Vaisefoder12@; PORT= 3306";
        public MySqlConnection con = null;

        public void AbrirConect()
        {
            try
            {
                con = new MySqlConnection(conect);
                con.Open();
            }
            catch (Exception ex)
            {
                throw ex;
            }           
        }

        public void FecharConect()
        {
            try
            {
                con = new MySqlConnection(conect);
                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}