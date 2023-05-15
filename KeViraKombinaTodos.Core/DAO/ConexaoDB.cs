using System;
using System.Data.SqlClient;

namespace KeViraKombinaTodos.Core.DAO {
	public class ConexaoDB {

		public string mErro = "";
		
		private string ConexaoWebConfig = "SqlServer";
		
		private string Server = "localhost";
		private string Database = "KeViraKombinaTodos";
		private string Usuario = "autoMeGenerator";
		private string Senha = "123456";

		public SqlConnection conn;

		public ConexaoDB(TipoConexao.Conexao TConexao) {
			GetConexao(TConexao);
		}
		public ConexaoDB() {
			GetConexao(TipoConexao.Conexao.Classe);
		}		
		public Boolean ExisteErro() {
			if (mErro.Length > 0) {
				return true;
			}
			return false;
		}
		// Faz a Conexao com o Banco de Dados
		private void GetConexao(TipoConexao.Conexao TConexao) {
			try {
				string connectionStrings = "";
				if (TConexao == TipoConexao.Conexao.Classe) {
					connectionStrings = string.Format("Server={0},1433;Database={1};User ID={2};Password={3}", this.Server, this.Database, this.Usuario, this.Senha);
				} else {
					connectionStrings = getWebConfig(this.ConexaoWebConfig);
				}

				this.conn = new SqlConnection(connectionStrings);
			} catch (Exception erro) {
				this.mErro = erro.Message;
				this.conn = null;
			}
		}

		// Abre conexao com o Banco de Dados
		public Boolean OpenConexao() {
			Boolean _return = true;
			try {
				conn.Open();
			} catch (Exception erro) {
				this.mErro = erro.Message;
				_return = false;
			}

			return _return;
		}

		// Fecha conexao com o Banco de Dados
		public void CloseConexao() {
			conn.Close();
			conn.Dispose();
		}		
		public string getWebConfig(string Variavel) {
			string strValue = "";
			//System.Configuration.Configuration rootWebConfig =
			//		System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~/");
			//System.Configuration.ConnectionStringSettings connString;
			//if (0 < rootWebConfig.ConnectionStrings.ConnectionStrings.Count) {
			//	connString = rootWebConfig.ConnectionStrings.ConnectionStrings[Variavel];
			//	if (null != connString)
			//		strValue = connString.ConnectionString;
			//	else
			//		strValue = "erro";
			//}

			return strValue;
		}
	}
	public class TipoConexao {
		public enum Conexao { WebConfig = 1, Classe = 2 };
	}
}