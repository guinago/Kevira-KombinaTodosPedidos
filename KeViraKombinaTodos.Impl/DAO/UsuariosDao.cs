using KeViraKombinaTodos.Core;
using KeViraKombinaTodos.Core.DAO;
using KeViraKombinaTodos.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace KeViraKombinaTodos.Impl.DAO {
	[Component]
	public class UsuariosDao : IUsuariosDao {

        #region IUsuariosDao Members

        public void CriarUsuario(string email, string cpf)
        {
            string criarUser = string.Format(" DECLARE @UsuarioID INT, @Nome VARCHAR(2000),  @Email VARCHAR(200) = '{0}', @CPF VARCHAR(40) = '{1}', @Telefone VARCHAR(40)", email, cpf) +
                " SELECT @UsuarioID = ID, @Nome = UserName, @Telefone = ISNULL(PhoneNumber, '') FROM AspnetUsers WHERE Email = @Email " +
                " INSERT INTO Usuario(UsuarioID, Nome, CPF, Telefone, Email, DataCriacao) VALUES(@UsuarioID, @Nome, @CPF, @Telefone, @Email, GETDATE())";
            ExecutarQuery(criarUser);
        }

        //public int Create(Perfil perfil) {
        //	string query = "INSERT INTO Perfil " +
        //		"VALUES(" +
        //		string.Format("'{0}',", perfil.Descricao) +
        //		string.Format("'{0}',", perfil.Codigo) +
        //		string.Format("{0}", Convert.ToInt32(perfil.SouTodoPoderoso)) +
        //		") " +

        //		" SELECT @@IDENTITY AS PerfilID";				

        //	return ExecutarQueryCriarPerfil(query);
        //}		
        public IList<Usuario> CarregarUsuarios()
        {
            return LoadAllUsuarios();
        }
        public Usuario CarregarUsuario(int usuarioID)
        {
            return LoadOnlyOneUsuario(usuarioID);
        }

        public void AtualizarUsuario(Usuario usuario)
        {
            string query = "UPDATE Usuario " +
                "SET " +
                string.Format("Nome = '{0}', ", usuario.Nome) +
                string.Format("CPF = '{0}', ", usuario.CPF) +
                string.Format("Telefone = '{0}', ", usuario.Telefone) +
                string.Format("Email = '{0}', ", usuario.Email) +
                "DataModif = GetDate()" +
                string.Format(" WHERE UsuarioID = {0}", usuario.UsuarioID);

            ExecutarQuery(query);
        }

        #endregion

        #region Methods Private
        private Usuario RetornaUsuarioReader(SqlDataReader reader) {
            Usuario usuario = new Usuario();

			usuario.UsuarioID = (int)(reader["UsuarioID"]);
			usuario.Nome = (string)reader["Nome"];
            usuario.CPF = (string)reader["CPF"];
            usuario.Telefone = (string)reader["Telefone"];
            usuario.Email = (string)reader["Email"];

            return usuario;
		}
		private IList<Usuario> LoadAllUsuarios() {
			ConexaoDB conexao = new ConexaoDB(TipoConexao.Conexao.Classe);

			if (conexao.ExisteErro()) {
				// erro
			}

			IList<Usuario> usuarios = new List<Usuario>();

			try {
				SqlDataReader reader;
				string query = "SELECT * FROM Usuario";
				SqlCommand cmd = new SqlCommand(query, conexao.conn);
				cmd.CommandType = System.Data.CommandType.Text;

				if (conexao.OpenConexao() == false) {
					//erro
				}

				reader = cmd.ExecuteReader();

				while (reader.Read()) {
                    usuarios.Add(RetornaUsuarioReader(reader));
				}
			} catch (SqlException e) {
				// erro
			}

			conexao.CloseConexao();

			return usuarios;
		}

		private Usuario LoadOnlyOneUsuario(int usuarioID) {
			ConexaoDB conexao = new ConexaoDB(TipoConexao.Conexao.Classe);

			if (conexao.ExisteErro()) {
				// erro
			}

			IList<Usuario> usuarios = new List<Usuario>();

			try {
				SqlDataReader reader;
				string query = "SELECT * FROM Usuario WHERE UsuarioID = " + usuarioID;
				SqlCommand cmd = new SqlCommand(query, conexao.conn);
				cmd.CommandType = System.Data.CommandType.Text;

				if (conexao.OpenConexao() == false) {
					//erro
				}

				reader = cmd.ExecuteReader();

				while (reader.Read()) {
                    usuarios.Add(RetornaUsuarioReader(reader));
				}
			} catch (SqlException e) {
				// erro
			}

			conexao.CloseConexao();

			return usuarios.FirstOrDefault(d => d.UsuarioID == usuarioID);
		}

		private int ExecutarQueryCriaUsuario(string query) {
			ConexaoDB conexao = new ConexaoDB(TipoConexao.Conexao.Classe);

			if (conexao.ExisteErro()) {
				// erro
			}

			int usuarioID = 0;

			try {
				SqlDataReader reader;
				SqlCommand cmd = new SqlCommand(query, conexao.conn);
				cmd.CommandType = System.Data.CommandType.Text;

				if (conexao.OpenConexao() == false) {
					//erro
				}

				reader = cmd.ExecuteReader();

				while (reader.Read()) {
                    usuarioID = Convert.ToInt32(reader["UsuarioID"]);
				}
			} catch (SqlException e) {
				// erro
			}

			conexao.CloseConexao();

			return usuarioID;
		}

		private void ExecutarQuery(string query) {
			ConexaoDB conexao = new ConexaoDB(TipoConexao.Conexao.Classe);

			if (conexao.ExisteErro()) {
				// erro
			}

			try {
				SqlDataReader reader;
				SqlCommand cmd = new SqlCommand(query, conexao.conn);
				cmd.CommandType = System.Data.CommandType.Text;

				if (conexao.OpenConexao() == false) {
					//erro
				}

				reader = cmd.ExecuteReader();

				
			} catch (SqlException e) {
				// erro
			}

			conexao.CloseConexao();
		}
		#endregion
	}
}
