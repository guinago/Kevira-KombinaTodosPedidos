using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KeViraKombinaTodos.Web.Models {
	public class CondicaoPagamentoModel {

        public int CondicaoPagamentoID { get; set; }
        [Required]				
		public string Descricao { get; set; }
        public string Codigo { get; set; }
        public DateTime? DataCriacao { get; set; }
        public DateTime? DataModificacao { get; set; }
        public IDictionary<int, string> ListCondPagamento { get; set; }
        public CondicaoPagamentoModel()
        {
            this.ListCondPagamento = new Dictionary<int, string>();
        }
    }	
}