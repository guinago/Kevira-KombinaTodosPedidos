using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KeViraKombinaTodos.Web.Models
{    
    public class PedidoModel
    {
        public int PedidoID { get; set; }
        public int VendedorID{ get; set; }
        public int ClienteID { get; set; }
        public int TransportadoraID { get; set; }
        public int CondicaoPagamentoID { get; set; }
        [Required]
        public string Telefone { get; set; }
        public string Email { get; set; }
        [Required]
        public string CEP { get; set; }
        [Required]
        public string Endereco { get; set; }
        [Required]
        public string Estado { get; set; }
        [Required]
        public string Municipio { get; set; }
        [Required]
        public string Bairro { get; set; }
        [DisplayFormat(DataFormatString ="dd/MM/yyyy")]
        public DateTime? DataEntrega { get; set; }
        public string Observacao { get; set; }
        public string Restricao { get; set; }
        public string NotaFiscal { get; set; }
        public int Status { get; set; }
        public double? ValorTotal { get; set; }
		public DateTime? DataCriacao { get; set; }
        public DateTime? DataModificacao { get; set; }
        [Required]
        public string Cliente { get; set; }
        public string CPF { get; set; }
        public string Complemento { get; set; }
        public string Vendedor { get; set; }
        [Required, Display(Name = "Condição de Pagamento")]
        public string CondicaoPagamento { get; set; }
        [Required]
        public string Transportadora { get; set; }
        public string StatusPedido { get; set; }
        public double? Frete { get; set; }
        public string PedidoInterno { get; set; }
        public IList<PedidoItemModel> Itens { get; set; }
        public PedidoItemModel PedidoItem { get; set; }
        public CondicaoPagamentoModel CondPagtoPedido { get; set; }
        public IDictionary<int, string> ListStatus { get; set; }
        public PedidoModel()
        {
            this.Itens = new List<PedidoItemModel>();
            this.ListStatus = new Dictionary<int, string>();

            this.ListStatus.Add(0, "Pendente");
            this.ListStatus.Add(1, "Entregue");
            this.ListStatus.Add(2, "Cancelado");
        }
        public DadosClienteModel DadosCliente { get; set; }
        public DadosEntregaModel DadosEntrega { get; set; }
    }
    public class PedidoItemModel
    {
        public int PedidoID { get; set; }
        public int? ProdutoID { get; set; }        
        public DateTime? DataCriacao { get; set; }
        public DateTime? DataModificacao { get; set; }       
        [Required, Display(Name = "Preço")]
        public double? Preco { get; set; }
        [Required]
        public double? Quantidade { get; set; }
        public bool Checked { get; set; }
        public bool NoCarrinho { get; set; }
        public string Descricao { get; set; }
        public string Codigo { get; set; }
    }
    public class DadosClienteModel
    {
        [Required]
        public string Nome { get; set; }
        [Required]
        public string CPF { get; set; }
        [Required]
        public string Telefone { get; set; }
        public string Email { get; set; }

    }
    public class DadosEntregaModel
    {
        [Required]
        public string CEP { get; set; }
        public string Estado { get; set; }
        [Required]
        public string Municipio { get; set; }
        public string Bairro { get; set; }
        [Required]
        public string Logradouro { get; set; }
        [Required, Display(Name = "Número")]
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Observacao { get; set; }
        public string Restricao { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "dd/MM/yyyy")]
        public DateTime? DataEntrega { get; set; }
        [Required]
        public int TransportadoraIDSelected { get; set; }
        public IDictionary<int, string> ListTransportadora { get; set; }
        public double? Frete { get; set; }

        public DadosEntregaModel()
        {
            this.ListTransportadora = new Dictionary<int, string>();
        }
    }
}