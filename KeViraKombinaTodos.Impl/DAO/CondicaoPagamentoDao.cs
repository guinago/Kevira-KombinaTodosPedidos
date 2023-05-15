using KeViraKombinaTodos.Core;
using KeViraKombinaTodos.Core.DAO;
using KeViraKombinaTodos.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace KeViraKombinaTodos.Impl.DAO {
	[Component]
	public class CondicaoPagamentoDao : ICondicaoPagamentoDao {	

		#region Methods Public
		public IList<CondicaoPagamento> CarregarCondicoesPagamento() {
			IList<CondicaoPagamento> CondicaoPagamentos = LoadAllCondicaoPagamentos();

			return CondicaoPagamentos;
		}

		public int CriarCondicaoPagamento(CondicaoPagamento CondicaoPagamento) {
            string query = "INSERT INTO CondicaoPagamento " +
                "VALUES(" +
                string.Format("'{0}',", CondicaoPagamento.Descricao) +
                string.Format("{0},", "GETDATE()") +
                string.Format("{0}", "GETDATE()") +
                ")" +

                " DECLARE @CondicaoPagamentoID INT = (SELECT @@IDENTITY AS CondicaoPagamentoID)" +
                " SELECT @CondicaoPagamentoID AS CondicaoPagamentoID";

            return ExecutarQueryCriarCondicaoPagamento(query);
		}

		public CondicaoPagamento CarregarCondicaoPagamento(int CondicaoPagamentoID) {
			return LoadOnlyOneCondicaoPagamento(CondicaoPagamentoID);
		}

		public void DesativarCondicaoPagamento(int CondicaoPagamentoID) {			
				string query = string.Format("UPDATE CondicaoPagamento SET Ativo = 0 WHERE CondicaoPagamentoID = {0}", CondicaoPagamentoID);
				ExecutarQuery(query);			
		}
		public void ExcluirCondicaoPagamento(int CondicaoPagamentoID) {
			string query = string.Format("DELETE CondicaoPagamento WHERE CondicaoPagamentoID = {0}", CondicaoPagamentoID);
			ExecutarQuery(query);
		}
		public void AtualizarCondicaoPagamento(CondicaoPagamento CondicaoPagamento) {
            string query = "UPDATE CondicaoPagamento " +
                "SET " +
                string.Format("Descricao = '{0}',", CondicaoPagamento.Descricao) +
                "DataModif = GETDATE()" +
                string.Format(" WHERE CondicaoPagamentoID = {0}", CondicaoPagamento.CondicaoPagamentoID);

            ExecutarQuery(query);
		}
		#endregion

		#region Methods Private
		private CondicaoPagamento RetornaCondicaoPagamentoReader(SqlDataReader reader) {
			CondicaoPagamento band = new CondicaoPagamento();

            band.CondicaoPagamentoID = Convert.ToInt32(reader["CondicaoPagamentoID"]);
            band.Descricao = (string)reader["Descricao"];
            band.DataCriacao = (DateTime)reader["Datacriacao"];
            band.DataModificacao = (DateTime)reader["DataModif"];

            return band;
		}
		private IList<CondicaoPagamento> LoadAllCondicaoPagamentos() {
			ConexaoDB conexao = new ConexaoDB(TipoConexao.Conexao.Classe);

			if (conexao.ExisteErro()) {
				// erro
			}

			IList<CondicaoPagamento> CondicaoPagamentos = new List<CondicaoPagamento>();

			try {
				SqlDataReader reader;
				string query = "SELECT * FROM CondicaoPagamento";
				SqlCommand cmd = new SqlCommand(query, conexao.conn);
				cmd.CommandType = System.Data.CommandType.Text;

				if (conexao.OpenConexao() == false) {
					//erro
				}

				reader = cmd.ExecuteReader();

				while (reader.Read()) {
					CondicaoPagamentos.Add(RetornaCondicaoPagamentoReader(reader));
				}
			} catch (SqlException e) {
				// erro
			}

			conexao.CloseConexao();

			return CondicaoPagamentos;
		}

		private CondicaoPagamento LoadOnlyOneCondicaoPagamento(int CondicaoPagamentoID) {
			ConexaoDB conexao = new ConexaoDB(TipoConexao.Conexao.Classe);

			if (conexao.ExisteErro()) {
				// erro
			}

			IList<CondicaoPagamento> CondicaoPagamentos = new List<CondicaoPagamento>();

			try {
				SqlDataReader reader;
				string query = "SELECT * FROM CondicaoPagamento WHERE CondicaoPagamentoID = " + CondicaoPagamentoID;
				SqlCommand cmd = new SqlCommand(query, conexao.conn);
				cmd.CommandType = System.Data.CommandType.Text;

				if (conexao.OpenConexao() == false) {
					//erro
				}

				reader = cmd.ExecuteReader();

				while (reader.Read()) {
					CondicaoPagamentos.Add(RetornaCondicaoPagamentoReader(reader));
				}
			} catch (SqlException e) {
				// erro
			}

			conexao.CloseConexao();

			return CondicaoPagamentos.FirstOrDefault();
		}

		private int ExecutarQueryCriarCondicaoPagamento(string query) {
			ConexaoDB conexao = new ConexaoDB(TipoConexao.Conexao.Classe);

			if (conexao.ExisteErro()) {
				// erro
			}

			int CondicaoPagamentoID = 0;

			try {
				SqlDataReader reader;
				SqlCommand cmd = new SqlCommand(query, conexao.conn);
				cmd.CommandType = System.Data.CommandType.Text;

				if (conexao.OpenConexao() == false) {
					//erro
				}

				reader = cmd.ExecuteReader();

				while (reader.Read()) {
					CondicaoPagamentoID = Convert.ToInt32(reader["CondicaoPagamentoID"]);
				}
			} catch (SqlException e) {
				// erro
			}

			conexao.CloseConexao();

			return CondicaoPagamentoID;
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
