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
	public class ProdutoDao : IProdutoDao {	

		#region Methods Public
		
		public IList<Produto> CarregarProdutos() {
			IList<Produto> Produto = LoadAllProdutos();

			return Produto;
		}
		public int CriarProduto(Produto Produto) {
            string query = "INSERT INTO Produto " +
                "VALUES(" +
                string.Format("'{0}', ", Produto.Descricao) +
                string.Format("'{0}', ", Produto.Codigo) +
                string.Format("CONVERT(FLOAT, REPLACE({0},',','.') ), ", Produto.Valor.ToString().Replace(",", ".")) +
                string.Format("{0}, ", Produto.Quantidade.ToString().Replace(",", ".")) +
                string.Format("{0}, ", 1) +
                string.Format("{0}, ", "GETDATE()") +
                string.Format("{0}", "GETDATE()") +
                ")" +

                " DECLARE @ProdutoID INT = (SELECT @@IDENTITY AS ProdutoID)" +
                " SELECT @ProdutoID AS ProdutoID";

            return ExecutarQueryCriarProduto(query);
		}
		public Produto CarregarProduto(int ProdutoID) {
			return LoadOnlyOneProduto(ProdutoID);
		}		
		public void ExcluirProduto(int ProdutoID) {
			string query = string.Format("DELETE Produto WHERE ProdutoID = {0}", ProdutoID);
			ExecutarQuery(query);
		}
        public void AtualizarProduto(Produto Produto)
        {
            StringBuilder query = new StringBuilder();
            query.AppendLine(string.Format("UPDATE Produto "));
            query.AppendLine(string.Format("SET "));
            if (!string.IsNullOrWhiteSpace(Produto.Descricao))
                query.AppendLine(string.Format("Descricao = '{0}',", Produto.Descricao));
            if (!string.IsNullOrWhiteSpace(Produto.Codigo))
                query.AppendLine(string.Format("Codigo = '{0}',", Produto.Codigo));
            if (!Produto.Valor.Equals(0))
                query.AppendLine(string.Format("Valor = '{0}',", Produto.Valor));
            if (!Produto.Quantidade.Equals(0))
                query.AppendLine(string.Format("Quantidade = '{0}',", Produto.Quantidade));
            if (!string.IsNullOrWhiteSpace(Produto.Ativo.ToString()))
                query.AppendLine(string.Format("Ativo = '{0}',", Convert.ToInt32(Produto.Ativo)));
            query.AppendLine(string.Format("DataModif = GETDATE()"));
            query.AppendLine(string.Format(" WHERE ProdutoID = {0}", Produto.ProdutoID));

            ExecutarQuery(query.ToString());
        }
        #endregion

        #region Methods Private
        private Produto RetornaProdutoReader(SqlDataReader reader) {
			Produto band = new Produto();

			band.ProdutoID = Convert.ToInt32(reader["ProdutoID"]);
			band.Descricao = (string)reader["Descricao"];
			band.Codigo = (string)reader["Codigo"];
			band.Valor = (double)reader["Valor"];
            band.Quantidade = (double)reader["Quantidade"];
            band.Ativo = Convert.ToBoolean((int)reader["Ativo"]);
            band.DataCriacao = (DateTime)reader["DataCriacao"];
            band.DataModificacao = (DateTime)reader["DataModif"];

            return band;
		}
		private IList<Produto> LoadAllProdutos() {
			ConexaoDB conexao = new ConexaoDB(TipoConexao.Conexao.Classe);

			if (conexao.ExisteErro()) {
				// erro
			}

			IList<Produto> Produto = new List<Produto>();

			try {
				SqlDataReader reader;
				string query = "SELECT * FROM Produto ";
				SqlCommand cmd = new SqlCommand(query, conexao.conn);
				cmd.CommandType = System.Data.CommandType.Text;

				if (conexao.OpenConexao() == false) {
					//erro
				}

				reader = cmd.ExecuteReader();

				while (reader.Read()) {
					Produto.Add(RetornaProdutoReader(reader));
				}
			} catch (SqlException e) {
				// erro
			}

			conexao.CloseConexao();

			return Produto;
		}

		private Produto LoadOnlyOneProduto(int ProdutoID) {
			ConexaoDB conexao = new ConexaoDB(TipoConexao.Conexao.Classe);

			if (conexao.ExisteErro()) {
				// erro
			}

			IList<Produto> Produto = new List<Produto>();

			try {
				SqlDataReader reader;
				string query = "SELECT * FROM Produto WHERE ProdutoID = " + ProdutoID;
				SqlCommand cmd = new SqlCommand(query, conexao.conn);
				cmd.CommandType = System.Data.CommandType.Text;

				if (conexao.OpenConexao() == false) {
					//erro
				}

				reader = cmd.ExecuteReader();

				while (reader.Read()) {
					Produto.Add(RetornaProdutoReader(reader));
				}
			} catch (SqlException e) {
				// erro
			}

			conexao.CloseConexao();

			return Produto.FirstOrDefault();
		}

		private int ExecutarQueryCriarProduto(string query) {
			ConexaoDB conexao = new ConexaoDB(TipoConexao.Conexao.Classe);

			if (conexao.ExisteErro()) {
				// erro
			}

			int ProdutoID = 0;

			try {
				SqlDataReader reader;
				SqlCommand cmd = new SqlCommand(query, conexao.conn);
				cmd.CommandType = System.Data.CommandType.Text;

				if (conexao.OpenConexao() == false) {
					//erro
				}

				reader = cmd.ExecuteReader();

				while (reader.Read()) {
					ProdutoID = Convert.ToInt32(reader["ProdutoID"]);
				}
			} catch (SqlException e) {
				// erro
			}

			conexao.CloseConexao();

			return ProdutoID;
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
