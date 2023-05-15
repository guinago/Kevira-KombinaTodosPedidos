using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeViraKombinaTodos.Web.Models
{
    public class EntregaPedidoModel
    {
        public int PedidoID { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        [Display(Name = "CEP")]
        public string EnderecoCEP { get; set; }
        public string EnderecoCompleto { get; set; }
        public string Estado { get; set; }
        public string Municipio { get; set; }
        public string EnderecoBairro { get; set; }
        public DateTime DataEntregaInicio { get; set; }
        public DateTime DataEntregaFim { get; set; }
        public string Transportadora { get; set; }
        public string CondicaoPagamento { get; set; }
        public string Observacao { get; set; }
        public string Restricao { get; set; }
        public int Status { get; set; }
        public DateTime? DataCriacao { get; set; }
        public DateTime? DataModificacao { get; set; }

    }
}