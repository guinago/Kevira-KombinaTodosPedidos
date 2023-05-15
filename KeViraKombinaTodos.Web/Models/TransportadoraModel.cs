using System;
using System.ComponentModel.DataAnnotations;

namespace KeViraKombinaTodos.Web.Models {
	public class TransportadoraModel {

        public int TransportadoraID { get; set; }
        [Required]				
		public string Descricao { get; set; }				
		public string Codigo { get; set; }
		public DateTime? DataCriacao { get; set; }
        public DateTime? DataModificacao { get; set; }
    }	
}