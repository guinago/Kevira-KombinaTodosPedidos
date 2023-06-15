using System;
using System.ComponentModel.DataAnnotations;

namespace KeViraKombinaTodos.Web.Models {
	public class TransportadoraModel {
        public int TransportadoraID { get; set; }
        [Required, Display(Name = "Descrição")]
        public string Descricao { get; set; }
        [MinLength(1), Required, Display(Name = "Código")]
        public string Codigo { get; set; }
		public DateTime? DataCriacao { get; set; }
        public DateTime? DataModificacao { get; set; }
    }	
}