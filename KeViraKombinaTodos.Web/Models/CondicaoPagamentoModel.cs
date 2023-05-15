using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KeViraKombinaTodos.Web.Models {
	public class CondicaoPagamentoModel {

        public int CondicaoPagamentoID { get; set; }
        [Required]				
		public string Descricao { get; set; }
		public int? CondPgto { get; set; }
        public DateTime? DataCriacao { get; set; }
        public DateTime? DataModificacao { get; set; }
        public IDictionary<int, string> ListCondPgto { get; set; }		
		public CondicaoPagamentoModel() {
			this.ListCondPgto = new Dictionary<int, string>();

			this.ListCondPgto.Add(1, "DINHEIRO");
			this.ListCondPgto.Add(2, "CARTÃO DE DÉBITO");
			this.ListCondPgto.Add(3, "CARTÃO DE CRÉDITO");
			this.ListCondPgto.Add(4, "PIX");
		}
	}	
}