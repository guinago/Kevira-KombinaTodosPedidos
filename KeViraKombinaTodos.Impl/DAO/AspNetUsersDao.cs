using KeViraKombinaTodos.Core;
using KeViraKombinaTodos.Core.DAO;
using KeViraKombinaTodos.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace KeViraKombinaTodos.Impl.DAO
{
    [Component]
    public class AspNetUsersDao : IAspNetUsersDao
    {
        public void CriarNovoUsuarioWorkflow(AspNetUsers objeto)
        {
            string query = "INSERT INTO AspNetUsers " +
                "VALUES( " +
                string.Format("'{0}',", objeto.Email) +
                string.Format("{0},", Convert.ToInt32(objeto.EmailConfirmed)) +
                string.Format("'{0}',", objeto.PasswordHash) +
                string.Format("'{0}',", objeto.SecurityStamp) +
                string.Format("{0},", Convert.ToInt32(objeto.TwoFactorEnabled)) +
                string.Format("NULL,") +
                string.Format("{0},", Convert.ToInt32(objeto.LockoutEnabled)) +
                string.Format("{0},", objeto.AccessFailedCount) +
                string.Format("'{0}',", objeto.Email) +

                string.Format("GETDATE(),") +
                string.Format("GETDATE(),") +
                string.Format("'{0}',", objeto.Password) +
                string.Format("'{0}',", objeto.Nome) +
                string.Format("{0},", Convert.ToInt32(objeto.IsEnabled)) +
                string.Format("{0}", objeto.PerfilID) + ")";

            ExecutarQuery(query);

        }
        public void AtualizarUsuario(AspNetUsers objeto)
        {
            string query = "UPDATE AspNetUsers " +
                "SET " +
                string.Format("Email ='{0}', ", objeto.Email) +
                string.Format("PhoneNumber = '{0}', ", objeto.PhoneNumber) +
                string.Format("UserName = '{0}' ", objeto.Nome) +
                string.Format("WHERE ID = " + objeto.ID);

            ExecutarQuery(query);

        }
        public int AtualizarColunaIDMaster(string email)
        {
            int id = RetornaIDAspNetUserPorEmail(email);

            string query = string.Format("UPDATE AspNetUsers SET IDMaster = {0} WHERE Email = '{1}'", id, email);
            ExecutarQuery(query);

            return id;
        }

        public void AtualizarPerfilUsuario(int idUser, int perfilID)
        {
            string query = string.Format("UPDATE AspNetUsers SET PerfilID = {0} WHERE ID = {1}", perfilID, idUser);
            ExecutarQuery(query);
        }
        public void ExcluirUsuario(int usuarioID)
        {
            string query = string.Format(" DELETE FROM AspNetUsers WHERE ID IN({0})", usuarioID);
            ExecutarQuery(query);
        }

        #region

        public void DesativarUsuario(int usuarioID)
        {
            string query = string.Format(" UPDATE AspNetUsers SET IsEnabled = 0 WHERE ID IN({0})", usuarioID);
            ExecutarQuery(query);
        }
        public void NovaSenhaUsuario(int usuarioID)
        {
            // senha = "Teste123@"
            string query = string.Format(" UPDATE AspNetUsers SET PassWordHash = 'AAGvhkLQSNlBwegKSCpnhxfaM/TPvS9ZSMpYSdMWftjBI+8ykn2rKKC02cB17ZJBgg==' WHERE ID IN({0})", usuarioID);
            ExecutarQuery(query);
        }

        public IList<AspNetUsers> CarregarUsuariosWorkflow(int workflowID)
        {
            return LoadAllUsersWF(workflowID);
        }
        public AspNetUsers CarregarUsuario(int id)
        {
            return CarregarUnicoUsuario(id);
        }
        #endregion

        #region Methods Private
        private AspNetUsers RetornaAspNetUsersReader(SqlDataReader reader)
        {
            AspNetUsers user = new AspNetUsers();

            user.Id = Convert.ToInt32(reader["Id"]);
            user.Email = (string)reader["Email"];
            user.PhoneNumber = (string)reader["PhoneNumber"];
            user.UserName = (string)reader["UserName"];
            user.DataCadastro = (DateTime)reader["DataCadastro"];
            user.CGC = (string)reader["CGC"];
            user.Nome = (string)reader["Nome"];
            user.IsEnabled = Convert.ToBoolean(reader["IsEnabled"]);
            user.PerfilID = (int)reader["PerfilID"];

            return user;
        }
        private int RetornaIDAspNetUserPorEmail(string email)
        {
            ConexaoDB conexao = new ConexaoDB(TipoConexao.Conexao.Classe);

            int id = 0;

            try
            {
                SqlDataReader reader;
                SqlCommand cmd = new SqlCommand(string.Format("SELECT * FROM AspNetUsers WHERE Email = '{0}'", email), conexao.conn);
                cmd.CommandType = System.Data.CommandType.Text;

                if (conexao.OpenConexao() == false)
                {
                    //erro
                }

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    id = Convert.ToInt32(reader["Id"]);
                }
            }
            catch (SqlException e)
            {
                // erro
            }

            conexao.CloseConexao();

            return id;
        }



        private void ExecutarQuery(string query)
        {
            ConexaoDB conexao = new ConexaoDB(TipoConexao.Conexao.Classe);

            if (conexao.ExisteErro())
            {
                // erro
            }

            try
            {
                SqlDataReader reader;
                SqlCommand cmd = new SqlCommand(query, conexao.conn);
                cmd.CommandType = System.Data.CommandType.Text;

                if (conexao.OpenConexao() == false)
                {
                    //erro
                }

                reader = cmd.ExecuteReader();

            }
            catch (SqlException e)
            {
                // erro
            }

            conexao.CloseConexao();
        }

        private IList<AspNetUsers> LoadAllUsersWF(int workflowID)
        {
            ConexaoDB conexao = new ConexaoDB(TipoConexao.Conexao.Classe);

            if (conexao.ExisteErro())
            {
                // erro
            }

            IList<AspNetUsers> list = new List<AspNetUsers>();

            try
            {
                SqlDataReader reader;
                string query = "SELECT * FROM AspNetUsers WHERE IsEnabled = 1 AND IDMaster = " + workflowID;
                SqlCommand cmd = new SqlCommand(query, conexao.conn);
                cmd.CommandType = System.Data.CommandType.Text;

                if (conexao.OpenConexao() == false)
                {
                    //erro
                }

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(RetornaAspNetUsersReader(reader));
                }
            }
            catch (SqlException e)
            {
                // erro
            }

            conexao.CloseConexao();

            return list;
        }

        private AspNetUsers CarregarUnicoUsuario(int id)
        {
            ConexaoDB conexao = new ConexaoDB(TipoConexao.Conexao.Classe);

            if (conexao.ExisteErro())
            {
                // erro
            }

            AspNetUsers user = new AspNetUsers();

            try
            {
                SqlDataReader reader;
                string query = "SELECT * FROM AspNetUsers WHERE ID = " + id;
                SqlCommand cmd = new SqlCommand(query, conexao.conn);
                cmd.CommandType = System.Data.CommandType.Text;

                if (conexao.OpenConexao() == false)
                {
                    //erro
                }

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    user = RetornaAspNetUsersReader(reader);
                }
            }
            catch (SqlException e)
            {
                // erro
            }

            conexao.CloseConexao();

            return user;
        }
        #endregion
    }
}
