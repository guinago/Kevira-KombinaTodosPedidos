using System;
using System.ComponentModel.DataAnnotations;

namespace KeViraKombinaTodos.Web.Models {
	public class ProdutoModel {		
        public int ProdutoID { get; set; }
        [Required, Display(Name = "Descrição")]
        public string Descricao { get; set; }
        [MinLength(1), Required, Display(Name = "Código")]
        public string Codigo { get; set; }
        public decimal Valor { get; set; }
        public decimal Quantidade { get; set; }
        public bool Ativo { get; set; }
        public DateTime? DataCriacao { get; set; }
        public DateTime? DataModificacao { get; set; }
    }	
}