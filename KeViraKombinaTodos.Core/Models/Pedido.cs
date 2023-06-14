using System;

namespace KeViraKombinaTodos.Core.Models {
	public class Pedido : EntityBase {
        #region Public Properties
        public int PedidoID { get; set; }
        public int VendedorID{ get; set; }
        public int ClienteID { get; set; }
        public int TransportadoraID { get; set; }
        public int CondicaoPagamentoID { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string CEP { get; set; }
        public string Endereco { get; set; }
        public string Estado { get; set; }
        public string Municipio { get; set; }
        public string Bairro { get; set; }
        public DateTime? DataEntrega { get; set; }
        public string Observacao { get; set; }
        public string Restricao { get; set; }
        public string NotaFiscal { get; set; }
        public int Status { get; set; }
        public double? ValorTotal { get; set; }
		public DateTime? DataCriacao { get; set; }
        public DateTime? DataModificacao { get; set; }
        public string Cliente { get; set; }
        public string CPF { get; set; }
        public string Complemento { get; set; }
        public string Vendedor { get; set; }
        public string CondicaoPagamento { get; set; }
        public string Transportadora { get; set; }
        public double? Frete { get; set; }
        public string PedidoInterno { get; set; }
        #endregion
    }
}
