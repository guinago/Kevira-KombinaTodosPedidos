using KeViraKombinaTodos.Core;
using KeViraKombinaTodos.Core.DAO;
using KeViraKombinaTodos.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace KeViraKombinaTodos.Impl.DAO {
	[Component]
	public class BaseCopiaDao : IBaseCopiaDao {	

		#region Methods Public
		public IList<Object> CarregarTodos(int workflowID) {
			return LoadAll(workflowID);
		}
		public int Cadastrar(Object obj) {
			string query = "INSERT INTO Object " +
				"VALUES(" +
				string.Format("{0},", obj.ToString()) +
				string.Format("'{0}',", obj.ToString()) +	
				string.Format("{0},", 1) +								
				string.Format("{0}", "GETDATE(), null")+
				")" +

				" DECLARE @ObjectID INT = (SELECT @@IDENTITY AS ObjectID)" +				
				" SELECT @ObjectID AS ObjectID";

			return ExecutarQueryCadastrar(query);
		}

		public Object Carregar(int key) {
			return LoadOnlyOne(key);
		}

		public void Desativar(int key) {			
				string query = string.Format("UPDATE Object SET Ativo = 0 WHERE ObjectID = {0}", key);
				ExecutarQuery(query);			
		}
		public void Excluir(int ObjectID) {
			string query = string.Format("DELETE Object WHERE ObjectID = {0}", ObjectID);
			ExecutarQuery(query);
		}
		public void Atualizar(Object obj) {
			string query = "UPDATE Object " +
				"SET " +
				string.Format("Descricao = '{0}', DataModificacao = GETDATE()", obj.ToString()) +
				string.Format(" WHERE ObjectID = {0}", obj.ToString());

			ExecutarQuery(query);
		}
		#endregion

		#region Methods Private
		private Object RetornaDataReader(SqlDataReader reader) {
			Object band = new Object();

			//band.ObjectID = Convert.ToInt32(reader["ObjectID"]);
			//band.WorkflowID = (int)reader["WorkflowID"];
			//band.Descricao = (string)reader["Descricao"];			
			//band.Ativo = (int)reader["Ativo"];			

			return band;
		}
		private IList<Object> LoadAll(int workflowID) {
			ConexaoDB conexao = new ConexaoDB(TipoConexao.Conexao.Classe);

			if (conexao.ExisteErro()) {
				// erro
			}

			IList<Object> Objects = new List<Object>();

			try {
				SqlDataReader reader;
				string query = "SELECT * FROM Object WHERE Ativo = 1 AND WorkflowID = " + workflowID;
				SqlCommand cmd = new SqlCommand(query, conexao.conn);
				cmd.CommandType = System.Data.CommandType.Text;

				if (conexao.OpenConexao() == false) {
					//erro
				}

				reader = cmd.ExecuteReader();

				while (reader.Read()) {
					Objects.Add(RetornaDataReader(reader));
				}
			} catch (SqlException) {
				// erro
			}

			conexao.CloseConexao();

			return Objects;
		}

		private Object LoadOnlyOne(int ObjectID) {
			ConexaoDB conexao = new ConexaoDB(TipoConexao.Conexao.Classe);

			if (conexao.ExisteErro()) {
				// erro
			}

			IList<Object> list = new List<Object>();

			try {
				SqlDataReader reader;
				string query = "SELECT * FROM Object WHERE ObjectID = " + ObjectID;
				SqlCommand cmd = new SqlCommand(query, conexao.conn);
				cmd.CommandType = System.Data.CommandType.Text;

				if (conexao.OpenConexao() == false) {
					//erro
				}

				reader = cmd.ExecuteReader();

				while (reader.Read()) {
					list.Add(RetornaDataReader(reader));
				}
			} catch (SqlException) {
				// erro
			}

			conexao.CloseConexao();

			return list.FirstOrDefault(d => d.ToString() == ObjectID.ToString());
		}

		private int ExecutarQueryCadastrar(string query) {
			ConexaoDB conexao = new ConexaoDB(TipoConexao.Conexao.Classe);

			if (conexao.ExisteErro()) {
				// erro
			}

			int key = 0;

			try {
				SqlDataReader reader;
				SqlCommand cmd = new SqlCommand(query, conexao.conn);
				cmd.CommandType = System.Data.CommandType.Text;

				if (conexao.OpenConexao() == false) {
					//erro
				}

				reader = cmd.ExecuteReader();

				while (reader.Read()) {
					key = Convert.ToInt32(reader["ObjectID"]);
				}
			} catch (SqlException) {
				// erro
			}

			conexao.CloseConexao();

			return key;
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


			} catch (SqlException) {
				// erro
			}

			conexao.CloseConexao();
		}
		#endregion
	}
}
