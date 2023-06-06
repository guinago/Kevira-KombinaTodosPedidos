using KeViraKombinaTodos.Core;
using KeViraKombinaTodos.Core.DAO;
using KeViraKombinaTodos.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace KeViraKombinaTodos.Impl.DAO {
	[Component]
	public class PerfilDao : IPerfilDao {

		#region IPerfilDao Members

		public int CriarPerfil(Perfil perfil) {
			string query = "INSERT INTO Perfil " +
				"VALUES(" +
				string.Format("'{0}', ", perfil.Descricao) +
				string.Format("'{0}', ", perfil.Codigo) +
				string.Format("{0}, ", Convert.ToInt32(perfil.SouTodoPoderoso)) +
                string.Format("{0}, ", Convert.ToInt32(perfil.SouComprador)) +
                string.Format("{0}, ", Convert.ToInt32(perfil.SouTransportador)) +
                " GetDate(), " +
                " GetDate() " +
                ") " +

				" SELECT @@IDENTITY AS PerfilID";				

			return ExecutarQueryCriarPerfil(query);
		}		
		public IList<Perfil> CarregarPerfis() {
			return LoadAllPerfil();
		}
		public Perfil CarregarPerfil(int perfilID) {
			return LoadOnlyOnePerfil(perfilID);
		}
		public void ExcluirPerfil(int perfilID) {
			string query = string.Format("DELETE Perfil WHERE perfilID = '{0}' ", perfilID);
			ExecutarQuery(query);
		}

        public void AtualizarPerfil(Perfil Perfil)
        {
            StringBuilder query = new StringBuilder();
            query.AppendLine(string.Format("UPDATE Perfil "));
            query.AppendLine(string.Format("SET "));
            if (!string.IsNullOrWhiteSpace(Perfil.Descricao))
                query.AppendLine(string.Format("Descricao = '{0}',", Perfil.Descricao));
            if (!string.IsNullOrWhiteSpace(Perfil.Codigo))
                query.AppendLine(string.Format("Codigo = '{0}',", Perfil.Codigo));
            if (!string.IsNullOrWhiteSpace(Perfil.SouComprador.ToString()))
                query.AppendLine(string.Format("SouComprador = '{0}',", Convert.ToInt32(Perfil.SouComprador)));
            if (!string.IsNullOrWhiteSpace(Perfil.SouTransportador.ToString()))
                query.AppendLine(string.Format("SouTransportador = '{0}',", Convert.ToInt32(Perfil.SouTransportador)));
            if (!string.IsNullOrWhiteSpace(Perfil.SouTodoPoderoso.ToString()))
                query.AppendLine(string.Format("SouTodoPoderoso = '{0}',", Convert.ToInt32(Perfil.SouTodoPoderoso)));
            query.AppendLine(string.Format("DataModif = GETDATE()"));
            query.AppendLine(string.Format(" WHERE PerfilID = {0}", Perfil.PerfilID));

            ExecutarQuery(query.ToString());
        }

        #endregion

        #region Methods Private
        private Perfil RetornaPerfilReader(SqlDataReader reader) {
			Perfil band = new Perfil();

            band.PerfilID = Convert.ToInt32(reader["PerfilID"]);
            band.Descricao = (string)reader["Descricao"];
            band.Codigo = (string)reader["Codigo"];
            band.SouTodoPoderoso = Convert.ToBoolean((int)reader["SouTodoPoderoso"]);
            band.SouComprador = Convert.ToBoolean((int)reader["SouComprador"]);
            band.SouTransportador = Convert.ToBoolean((int)reader["SouTransportador"]);
            band.Datacriacao = (DateTime)reader["DataCriacao"];
            band.DataModificacao = (DateTime)reader["DataModif"];

            return band;
		}
		private IList<Perfil> LoadAllPerfil() {
			ConexaoDB conexao = new ConexaoDB(TipoConexao.Conexao.Classe);

			if (conexao.ExisteErro()) {
				// erro
			}

			IList<Perfil> perfis = new List<Perfil>();

			try {
				SqlDataReader reader;
				string query = "SELECT * FROM Perfil";
				SqlCommand cmd = new SqlCommand(query, conexao.conn);
				cmd.CommandType = System.Data.CommandType.Text;

				if (conexao.OpenConexao() == false) {
					//erro
				}

				reader = cmd.ExecuteReader();

				while (reader.Read()) {
                    perfis.Add(RetornaPerfilReader(reader));
				}
			} catch (SqlException e) {
				// erro
			}

			conexao.CloseConexao();

			return perfis;
		}

		private Perfil LoadOnlyOnePerfil(int perfilID) {
			ConexaoDB conexao = new ConexaoDB(TipoConexao.Conexao.Classe);

			if (conexao.ExisteErro()) {
				// erro
			}

			IList<Perfil> perfis = new List<Perfil>();

			try {
				SqlDataReader reader;
				string query = "SELECT * FROM Perfil WHERE PerfilID = " + perfilID;
				SqlCommand cmd = new SqlCommand(query, conexao.conn);
				cmd.CommandType = System.Data.CommandType.Text;

				if (conexao.OpenConexao() == false) {
					//erro
				}

				reader = cmd.ExecuteReader();

				while (reader.Read()) {
					perfis.Add(RetornaPerfilReader(reader));
				}
			} catch (SqlException e) {
				// erro
			}

			conexao.CloseConexao();

			return perfis.FirstOrDefault(d => d.PerfilID == perfilID);
		}

		private int ExecutarQueryCriarPerfil(string query) {
			ConexaoDB conexao = new ConexaoDB(TipoConexao.Conexao.Classe);

			if (conexao.ExisteErro()) {
				// erro
			}

			int perfilID = 0;

			try {
				SqlDataReader reader;
				SqlCommand cmd = new SqlCommand(query, conexao.conn);
				cmd.CommandType = System.Data.CommandType.Text;

				if (conexao.OpenConexao() == false) {
					//erro
				}

				reader = cmd.ExecuteReader();

				while (reader.Read()) {
					perfilID = Convert.ToInt32(reader["PerfilID"]);
				}
			} catch (SqlException e) {
				// erro
			}

			conexao.CloseConexao();

			return perfilID;
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
