using KeViraKombinaTodos.Core;
using KeViraKombinaTodos.Core.DAO;
using KeViraKombinaTodos.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace KeViraKombinaTodos.Impl.DAO {
	[Component]
	public class PedidoDao : IPedidoDao {	

		#region Methods Public
		public IList<Pedido> CarregarPedidos(int vendedorID) {
			IList<Pedido> pedidos = LoadAllPedidos(vendedorID);

			return pedidos;
		}
		public int CriarPedido(Pedido obj) {
            
            string query = "INSERT INTO Pedido " +
                "VALUES(" +
                string.Format("'{0}',", obj.ClienteID) +
                string.Format("'{0}',", obj.VendedorID) +
                string.Format("'{0}',", obj.TransportadoraID) +
                string.Format("'{0}',", obj.CondicaoPagamentoID) +
                string.Format("'{0}',", obj.Telefone) +
                string.Format("'{0}',", obj.Email) +
                string.Format("'{0}',", obj.CEP) +
                string.Format("'{0}',", obj.Estado) +
                string.Format("'{0}',", obj.Municipio) +
                string.Format("'{0}',", obj.Bairro) +
                string.Format("'{0}',", obj.Endereco) +
                string.Format("'{0}',", obj.DataEntrega.Value.ToString("yyyy-MM-dd HH:mm:ss")) +
                string.Format("'{0}',", obj.Observacao) +
                string.Format("'{0}',", obj.Restricao) +
                string.Format("'{0}',", obj.NotaFiscal) +
                string.Format("'{0}',", obj.Status) +
                string.Format("CONVERT(FLOAT, REPLACE({0},',','.') ), ", obj.ValorTotal.ToString().Replace(",", ".")) +
                string.Format("{0}, ", "GETDATE() ") +
                string.Format("{0} ", "null") +
                ") " +

                " SELECT @@IDENTITY AS PedidoID";

            return ExecutarQueryCriarRetorno(query);
		}

		public Pedido CarregarPedido(int PedidoID) {
			return LoadOnlyOneRetorno(PedidoID);
		}				
		public void AtualizarPedido(Pedido obj) {
            string query = "UPDATE Pedido " +
                "SET " +
            string.Format("TransportadoraID = '{0}',", obj.TransportadoraID) +
            string.Format("CondicaoPagamentoID = '{0}',", obj.CondicaoPagamentoID) +
            string.Format("Telefone = '{0}',", obj.Telefone) +
            string.Format("Email = '{0}',", obj.Email) +
            string.Format("CEP = '{0}',", obj.CEP) +
            string.Format("Endereco = '{0}',", obj.Endereco) +
            string.Format("Estado = '{0}',", obj.Estado) +
            string.Format("Municipio = '{0}',", obj.Municipio) +
            string.Format("Bairro = '{0}',", obj.Bairro) +
            string.Format("DataEntrega = '{0}',", obj.DataEntrega.Value.ToString("yyyy-MM-dd HH:mm:ss")) +
            string.Format("Observacao = '{0}',", obj.Observacao) +
            string.Format("Restricao = '{0}',", obj.Restricao) +
            string.Format("NotaFiscal = '{0}',", obj.NotaFiscal) +
            string.Format("Status = '{0}',", obj.Status) +
            string.Format("ValorTotal = CONVERT(FLOAT, REPLACE({0},',','.') ), ", obj.ValorTotal.ToString().Replace(",", ".")) +
            string.Format("DataModif = GETDATE() ") +
            string.Format(" WHERE PedidoID = {0}", obj.PedidoID);

            ExecutarQuery(query);
		}

        public void ExcluirPedido(int PedidoID)
        {
            string query = string.Format("DELETE Pedido WHERE PedidoID = {0}", PedidoID);
            ExecutarQuery(query);
        }
        #endregion

        #region Methods Private
        private Pedido RetornaRetornoReader(SqlDataReader reader) {
			Pedido band = new Pedido();

			band.PedidoID = Convert.ToInt32(reader["PedidoID"]);
            band.ClienteID = Convert.ToInt32(reader["ClienteID"]);
            band.VendedorID = Convert.ToInt32(reader["VendedorID"]);
            band.TransportadoraID = Convert.ToInt32(reader["TransportadoraID"]);
            band.CondicaoPagamentoID = Convert.ToInt32(reader["CondicaoPagamentoID"]);
            band.Telefone = (string)reader["Telefone"];
            band.Email = (string)reader["Email"];
            band.CEP = (string)reader["CEP"];
            band.Endereco = (string)reader["Endereco"];
            band.Estado = (string)reader["Estado"];
            band.Municipio = (string)reader["Municipio"];
            band.Bairro = (string)reader["Bairro"];
            band.DataEntrega = (DateTime)reader["DataEntrega"];
            band.Observacao = (string)reader["Observacao"];
            band.Restricao = (string)reader["Restricao"];
            band.NotaFiscal = (string)reader["NotaFiscal"];
            band.Status = (int)reader["Status"];
            band.ValorTotal = (double)reader["ValorTotal"];
            band.DataCriacao = (DateTime)reader["Datacriacao"];
            band.Cliente = (string)reader["Cliente"];
            band.CPF = (string)reader["CPF"];
            band.Complemento = (string)reader["Complemento"];
            band.Vendedor = (string)reader["Vendedor"];
            band.CondicaoPagamento = (string)reader["CondicaoPagamento"];
            band.Transportadora = (string)reader["Transportadora"];

            return band;
		}
        private IList<Pedido> LoadAllPedidos(int vendedorID = 0)
        {
            ConexaoDB conexao = new ConexaoDB(TipoConexao.Conexao.Classe);

            if (conexao.ExisteErro())
            {
                // erro
            }

            IList<Pedido> pedidos = new List<Pedido>();
            var condicao = (vendedorID > 0 ? $" Where VendedorID = {vendedorID}" : string.Empty);

            try
            {
                SqlDataReader reader;
                string query = "SELECT DISTINCT P.PedidoID, P.ClienteID, P.VendedorID, P.TransportadoraID, P.CondicaoPagamentoID, P.Telefone, P.Email, P.CEP, " +
                    "P.Endereco, P.Estado, P.Municipio, P.Bairro, P.DataEntrega, P.Observacao, P.Restricao, P.NotaFiscal, P.ValorTotal, P.DataCriacao, P.Status, " +
                    "C.Nome AS Cliente, C.CPF, C.Complemento, A.Nome AS Vendedor, CP.Descricao AS CondicaoPagamento, T.Descricao AS Transportadora " +
                    "FROM Pedido P " +
                    "INNER JOIN Cliente C ON C.ClienteID = P.ClienteID " +
                    "INNER JOIN AspNetUsers A ON A.ID = P.VendedorID " +
                    "INNER JOIN CondicaoPagamento CP ON CP.CondicaoPagamentoID = P.CondicaoPagamentoID " +
                    "INNER JOIN Transportadora T ON T.TransportadoraID = P.TransportadoraID " +
                condicao;

                SqlCommand cmd = new SqlCommand(query, conexao.conn);
                cmd.CommandType = System.Data.CommandType.Text;

                if (conexao.OpenConexao() == false)
                {
                    //erro
                }

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    pedidos.Add(RetornaRetornoReader(reader));
                }
            }
            catch (SqlException e)
            {
                // erro
            }
            conexao.CloseConexao();

            return pedidos;
        }

        private Pedido LoadOnlyOneRetorno(int PedidoID)
        {
            ConexaoDB conexao = new ConexaoDB(TipoConexao.Conexao.Classe);

            if (conexao.ExisteErro())
            {
                // erro
            }
            IList<Pedido> Pedidos = new List<Pedido>();

            try
            {
                SqlDataReader reader;
                string query = "SELECT DISTINCT P.PedidoID, P.ClienteID, P.VendedorID, P.TransportadoraID, P.CondicaoPagamentoID, P.Telefone, P.Email, P.CEP, " +
                    "P.Endereco, P.Estado, P.Municipio, P.Bairro, P.DataEntrega, P.Observacao, P.Restricao, P.NotaFiscal, P.ValorTotal, P.DataCriacao, P.Status, " +
                    "C.Nome AS Cliente, C.CPF, C.Complemento, A.Nome AS Vendedor, CP.Descricao AS CondicaoPagamento, T.Descricao AS Transportadora " +
                    "FROM Pedido P " +
                    "INNER JOIN Cliente C ON C.ClienteID = P.ClienteID " +
                    "INNER JOIN AspNetUsers A ON A.ID = P.VendedorID " +
                    "INNER JOIN CondicaoPagamento CP ON CP.CondicaoPagamentoID = P.CondicaoPagamentoID " +
                    "INNER JOIN Transportadora T ON T.TransportadoraID = P.TransportadoraID " +
                    "WHERE P.PedidoID = " + PedidoID;

                SqlCommand cmd = new SqlCommand(query, conexao.conn);
                cmd.CommandType = System.Data.CommandType.Text;

                if (conexao.OpenConexao() == false)
                {
                    //erro
                }
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Pedidos.Add(RetornaRetornoReader(reader));
                }
            }
            catch (SqlException)
            {
                // erro
            }
            conexao.CloseConexao();
            //return Pedidos.FirstOrDefault(d => d.PedidosID == PedidosID);
            return Pedidos.FirstOrDefault();
        }

        private int ExecutarQueryCriarRetorno(string query) {
			ConexaoDB conexao = new ConexaoDB(TipoConexao.Conexao.Classe);

			if (conexao.ExisteErro()) {
				// erro
			}

			int PedidosID = 0;

			try {
				SqlDataReader reader;
				SqlCommand cmd = new SqlCommand(query, conexao.conn);
				cmd.CommandType = System.Data.CommandType.Text;

				if (conexao.OpenConexao() == false) {
					//erro
				}

				reader = cmd.ExecuteReader();

				while (reader.Read()) {
					PedidosID = Convert.ToInt32(reader["PedidoID"]);
				}
			} catch (SqlException) {
				// erro
			}

			conexao.CloseConexao();

			return PedidosID;
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
