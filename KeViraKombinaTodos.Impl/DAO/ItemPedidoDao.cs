﻿using KeViraKombinaTodos.Core;
using KeViraKombinaTodos.Core.DAO;
using KeViraKombinaTodos.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace KeViraKombinaTodos.Impl.DAO {
	[Component]
	public class ItemPedidoDao : IItemPedidoDao {	

		#region Methods Public
		public IList<ItemPedido> CarregarItensPedido(int PedidoID) {
			IList<ItemPedido> Itens = LoadAllItensPedido(PedidoID);

			return Itens;
		}
		public int CriarItemPedido(ItemPedido obj) {

            string query = "INSERT INTO ItemPedido " +
                "VALUES(" +
                string.Format("'{0}',", obj.PedidoID) +
                string.Format("'{0}',", obj.ProdutoID) +
                string.Format("CONVERT(FLOAT, REPLACE({0},',','.') ), ", obj.Preco.ToString().Replace(",", ".")) +
                string.Format("{0}, ", obj.Quantidade.ToString().Replace(",", ".")) +
                string.Format("{0}, ", "GETDATE() ") +
                string.Format("{0} ", "null") +
                ") ";

            return ExecutarQueryCriarRetorno(query);
		}

		public ItemPedido CarregarItemPedido(int PedidoID) {
			return LoadOnlyOneRetorno(PedidoID);
		}				

        public void AtualizarItemPedido(ItemPedido obj)
        {
            StringBuilder query = new StringBuilder();
            query.AppendLine(string.Format("UPDATE ItemPedido "));
            query.AppendLine(string.Format("SET "));
            if (!string.IsNullOrWhiteSpace(obj.Preco.ToString()))
                query.AppendLine(string.Format("Preco = CONVERT(FLOAT, REPLACE({0},',','.') ), ", obj.Preco.ToString().Replace(",", ".")));
            if (!string.IsNullOrWhiteSpace(obj.Quantidade.ToString()))
                query.AppendLine(string.Format("Quantidade = CONVERT(FLOAT, REPLACE({0},',','.') ), ", obj.Quantidade.ToString().Replace(",", ".")));
            query.AppendLine(string.Format("DataModif = GETDATE()"));
            query.AppendLine(string.Format(" WHERE PedidoID = {0}", obj.PedidoID));
            query.AppendLine(string.Format(" AND ProdutoID = {0}", obj.ProdutoID));

            ExecutarQuery(query.ToString());
        }

        public void ExcluirItemPedido(int PedidoID, int ProdutoID)
        {
            string query = string.Format("DELETE ItemPedido WHERE PedidoID = {0} AND ProdutoID = {1}", PedidoID, ProdutoID);
            ExecutarQuery(query);
        }
        #endregion

        #region Methods Private
        private ItemPedido RetornaRetornoReader(SqlDataReader reader) {
			ItemPedido band = new ItemPedido();

			band.PedidoID = Convert.ToInt32(reader["PedidoID"]);
            band.ProdutoID = Convert.ToInt32(reader["ProdutoID"]);
            band.Preco = Convert.ToDecimal((reader["Preco"]));
            band.Quantidade = Convert.ToDecimal(reader["Quantidade"]);
            band.Descricao = Convert.ToString(reader["Descricao"]);
            band.Codigo = Convert.ToString(reader["Codigo"]);

            return band;
		}
        private IList<ItemPedido> LoadAllItensPedido(int PedidoID = 0)
        {
            ConexaoDB conexao = new ConexaoDB(TipoConexao.Conexao.Classe);

            if (conexao.ExisteErro())
            {
                // erro
            }

            IList<Pedido> pedidos = new List<Pedido>();
            var condicao = (PedidoID > 0 ? $" Where P.PedidoID = {PedidoID}" : string.Empty);

            IList<ItemPedido> Itens = new List<ItemPedido>();

            try
            {
                SqlDataReader reader;
                string query = "SELECT IP.PedidoID, IP.ProdutoID, IP.Preco, IP.Quantidade, Prod.Descricao, Prod.Codigo " +
                    "FROM ItemPedido IP " +
                    "INNER JOIN Pedido P ON P.PedidoID = IP.PedidoID " +
                    "INNER JOIN Produto Prod ON Prod.ProdutoID = IP.ProdutoID " +
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
                    Itens.Add(RetornaRetornoReader(reader));
                }
            }
            catch (SqlException e)
            {
                // erro
            }
            conexao.CloseConexao();

            return Itens;
        }

        private ItemPedido LoadOnlyOneRetorno(int PedidoID)
        {
            ConexaoDB conexao = new ConexaoDB(TipoConexao.Conexao.Classe);

            if (conexao.ExisteErro())
            {
                // erro
            }
            IList<ItemPedido> Itens = new List<ItemPedido>();

            try
            {
                SqlDataReader reader;
                string query = "SELECT P.PedidosID, P.ProdutoID, P.Preco, P.Quantidade, " +
                    "FROM ItemPedido IP(NOLOCK) " +
                    "INNER JOIN Pedidos P(NOLOCK)ON(P.PedidoID = IP.PedidoID) " +
                    "WHERE P.PedidosID = " + PedidoID;

                SqlCommand cmd = new SqlCommand(query, conexao.conn);
                cmd.CommandType = System.Data.CommandType.Text;

                if (conexao.OpenConexao() == false)
                {
                    //erro
                }
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Itens.Add(RetornaRetornoReader(reader));
                }
            }
            catch (SqlException)
            {
                // erro
            }
            conexao.CloseConexao();
            //return Pedidos.FirstOrDefault(d => d.PedidosID == PedidosID);
            return Itens.FirstOrDefault();
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
