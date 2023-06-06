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
	public class TransportadoraDao : ITransportadoraDao {	

		#region Methods Public
		public IList<Transportadora> CarregarTransportadoras() {
			IList<Transportadora> Transportadoras = LoadAllTransportadoras();

			return Transportadoras;
		}

		public int CriarTransportadora(Transportadora Transportadora) {
            string query = "INSERT INTO Transportadora " +
                "VALUES(" +
                string.Format("'{0}',", Transportadora.Descricao) +
                string.Format("{0},", Transportadora.Codigo) +
                string.Format("{0},", "GETDATE()") +
                string.Format("{0}", "GETDATE()") +
                ")" +

                " DECLARE @TransportadoraID INT = (SELECT @@IDENTITY AS TransportadoraID)" +
                " SELECT @TransportadoraID AS TransportadoraID";

            return ExecutarQueryCriarTransportadora(query);
		}

		public Transportadora CarregarTransportadora(int TransportadoraID) {
			return LoadOnlyOneTransportadora(TransportadoraID);
		}

		public void DesativarTransportadora(int TransportadoraID) {			
				string query = string.Format("UPDATE Transportadora SET Ativo = 0 WHERE TransportadoraID = {0}", TransportadoraID);
				ExecutarQuery(query);			
		}
		public void ExcluirTransportadora(int TransportadoraID) {
			string query = string.Format("DELETE Transportadora WHERE TransportadoraID = {0}", TransportadoraID);
			ExecutarQuery(query);
		}

        public void AtualizarTransportadora(Transportadora Transportadora)
        {
            StringBuilder query = new StringBuilder();
            query.AppendLine(string.Format("UPDATE Transportadora "));
            query.AppendLine(string.Format("SET "));
            if (!string.IsNullOrWhiteSpace(Transportadora.Descricao))
                query.AppendLine(string.Format("Descricao = '{0}',", Transportadora.Descricao));
            if (!string.IsNullOrWhiteSpace(Transportadora.Codigo))
                query.AppendLine(string.Format("Codigo = '{0}',", Transportadora.Codigo));
            query.AppendLine(string.Format("DataModif = GETDATE()"));
            query.AppendLine(string.Format(" WHERE TransportadoraID = {0}", Transportadora.TransportadoraID));

            ExecutarQuery(query.ToString());
        }
        #endregion

        #region Methods Private
        private Transportadora RetornaTransportadoraReader(SqlDataReader reader) {
			Transportadora band = new Transportadora();

            band.TransportadoraID = Convert.ToInt32(reader["TransportadoraID"]);
            band.Descricao = (string)reader["Descricao"];
            band.Codigo = (string)reader["Codigo"];
            band.DataCriacao = (DateTime)reader["Datacriacao"];
            band.DataModificacao = (DateTime)reader["DataModif"];

            return band;
		}
		private IList<Transportadora> LoadAllTransportadoras() {
			ConexaoDB conexao = new ConexaoDB(TipoConexao.Conexao.Classe);

			if (conexao.ExisteErro()) {
				// erro
			}

			IList<Transportadora> Transportadoras = new List<Transportadora>();

			try {
				SqlDataReader reader;
				string query = "SELECT * FROM Transportadora ";
				SqlCommand cmd = new SqlCommand(query, conexao.conn);
				cmd.CommandType = System.Data.CommandType.Text;

				if (conexao.OpenConexao() == false) {
					//erro
				}

				reader = cmd.ExecuteReader();

				while (reader.Read()) {
					Transportadoras.Add(RetornaTransportadoraReader(reader));
				}
			} catch (SqlException e) {
				// erro
			}

			conexao.CloseConexao();

			return Transportadoras;
		}

		private Transportadora LoadOnlyOneTransportadora(int TransportadoraID) {
			ConexaoDB conexao = new ConexaoDB(TipoConexao.Conexao.Classe);

			if (conexao.ExisteErro()) {
				// erro
			}

			IList<Transportadora> Transportadoras = new List<Transportadora>();

			try {
				SqlDataReader reader;
				string query = "SELECT * FROM Transportadora WHERE TransportadoraID = " + TransportadoraID;
				SqlCommand cmd = new SqlCommand(query, conexao.conn);
				cmd.CommandType = System.Data.CommandType.Text;

				if (conexao.OpenConexao() == false) {
					//erro
				}

				reader = cmd.ExecuteReader();

				while (reader.Read()) {
					Transportadoras.Add(RetornaTransportadoraReader(reader));
				}
			} catch (SqlException e) {
				// erro
			}

			conexao.CloseConexao();

			return Transportadoras.FirstOrDefault();
		}

		private int ExecutarQueryCriarTransportadora(string query) {
			ConexaoDB conexao = new ConexaoDB(TipoConexao.Conexao.Classe);

			if (conexao.ExisteErro()) {
				// erro
			}

			int TransportadoraID = 0;

			try {
				SqlDataReader reader;
				SqlCommand cmd = new SqlCommand(query, conexao.conn);
				cmd.CommandType = System.Data.CommandType.Text;

				if (conexao.OpenConexao() == false) {
					//erro
				}

				reader = cmd.ExecuteReader();

				while (reader.Read()) {
					TransportadoraID = Convert.ToInt32(reader["TransportadoraID"]);
				}
			} catch (SqlException e) {
				// erro
			}

			conexao.CloseConexao();

			return TransportadoraID;
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
