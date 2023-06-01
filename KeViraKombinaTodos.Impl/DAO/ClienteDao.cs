using KeViraKombinaTodos.Core;
using KeViraKombinaTodos.Core.DAO;
using KeViraKombinaTodos.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace KeViraKombinaTodos.Impl.DAO {
	[Component]
	public class ClienteDao : IClienteDao
    {

        #region IClienteDao Members

        public int CriarCliente(Cliente Cliente)
        {
            string query = "INSERT INTO Cliente " +
            "VALUES(" +
            string.Format("'{0}',", Cliente.Nome) +
            string.Format("'{0}',", Cliente.CPF) +
            string.Format("'{0}',", Cliente.Telefone) +
            string.Format("'{0}',", Cliente.Email) +
            string.Format("'{0}',", Cliente.CEP) +
            string.Format("'{0}',", Cliente.Estado) +
            string.Format("'{0}',", Cliente.Municipio) +
            string.Format("'{0}',", Cliente.Bairro) +
            string.Format("'{0}',", Cliente.Endereco) +
            string.Format("'{0}',", Cliente.Complemento) +
            string.Format("{0},", "GETDATE()") +
            string.Format("{0}", "GETDATE()") +
            ")" +

            " DECLARE @ClienteID INT = (SELECT @@IDENTITY AS ClienteID)" +
            " SELECT @ClienteID AS ClienteID";

            return ExecutarQueryCriaCliente(query);
        }
	
        public IList<Cliente> CarregarClientes()
        {
            return LoadAllClientes();
        }
        public Cliente CarregarCliente(int clienteID)
        {
            return LoadOnlyOneCliente(clienteID);
        }

        public void AtualizarCliente(Cliente cliente)
        {
            string query = "UPDATE Cliente " +
                "SET " +
                string.Format("Nome = '{0}', ", cliente.Nome) +
                string.Format("CPF = '{0}', ", cliente.CPF) +
                string.Format("Telefone = '{0}', ", cliente.Telefone) +
                string.Format("Email = '{0}', ", cliente.Email) +
                string.Format("CEP = '{0}', ", cliente.CEP) +
                string.Format("Estado = '{0}', ", cliente.Estado) +
                string.Format("Municipio = '{0}', ", cliente.Municipio) +
                string.Format("Bairro = '{0}', ", cliente.Bairro) +
                string.Format("Endereco = '{0}', ", cliente.Endereco) +
                string.Format("Complemento = '{0}', ", cliente.Complemento) +
                "DataModif = GetDate()" +
                string.Format(" WHERE clienteID = {0}", cliente.ClienteID);

            ExecutarQuery(query);
        }

        #endregion

        #region Methods Private
        private Cliente RetornaClienteReader(SqlDataReader reader) {
            Cliente cliente = new Cliente();

			cliente.ClienteID = (int)(reader["ClienteID"]);
			cliente.Nome = (string)reader["Nome"];
            cliente.CPF = (string)reader["CPF"];
            cliente.Telefone = (string)reader["Telefone"];
            cliente.Email = (string)reader["Email"];
            cliente.CEP = (string)reader["CEP"];
            cliente.Estado = (string)reader["Estado"];
            cliente.Municipio = (string)reader["Municipio"];
            cliente.Bairro = (string)reader["Bairro"];
            cliente.Endereco = (string)reader["Endereco"];
            cliente.Complemento = (string)reader["Complemento"];

            return cliente;
		}
		private IList<Cliente> LoadAllClientes() {
			ConexaoDB conexao = new ConexaoDB(TipoConexao.Conexao.Classe);

			if (conexao.ExisteErro()) {
				// erro
			}

			IList<Cliente> clientes = new List<Cliente>();

			try {
				SqlDataReader reader;
				string query = "SELECT * FROM Cliente";
				SqlCommand cmd = new SqlCommand(query, conexao.conn);
				cmd.CommandType = System.Data.CommandType.Text;

				if (conexao.OpenConexao() == false) {
					//erro
				}

				reader = cmd.ExecuteReader();

				while (reader.Read()) {
                    clientes.Add(RetornaClienteReader(reader));
				}
			} catch (SqlException e) {
				// erro
			}

			conexao.CloseConexao();

			return clientes;
		}

		private Cliente LoadOnlyOneCliente(int clienteID) {
			ConexaoDB conexao = new ConexaoDB(TipoConexao.Conexao.Classe);

			if (conexao.ExisteErro()) {
				// erro
			}

			IList<Cliente> clientes = new List<Cliente>();

			try {
				SqlDataReader reader;
				string query = "SELECT * FROM Cliente WHERE ClienteID = " + clienteID;
				SqlCommand cmd = new SqlCommand(query, conexao.conn);
				cmd.CommandType = System.Data.CommandType.Text;

				if (conexao.OpenConexao() == false) {
					//erro
				}

				reader = cmd.ExecuteReader();

				while (reader.Read()) {
                    clientes.Add(RetornaClienteReader(reader));
				}
			} catch (SqlException e) {
				// erro
			}

			conexao.CloseConexao();

			return clientes.FirstOrDefault(d => d.ClienteID == clienteID);
		}

        private int ExecutarQueryCriaCliente(string query)
        {
            ConexaoDB conexao = new ConexaoDB(TipoConexao.Conexao.Classe);

            if (conexao.ExisteErro())
            {
                // erro
            }

            int clienteID = 0;

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

                while (reader.Read())
                {
                    clienteID = Convert.ToInt32(reader["ClienteID"]);
                }
            }
            catch (SqlException e)
            {
                // erro
            }

            conexao.CloseConexao();

            return clienteID;
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
