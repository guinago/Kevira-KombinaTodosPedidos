using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using KeViraKombinaTodos.Core.Models;

namespace KeViraKombinaTodos.Web.Models {
	public class ProdutoModel {		
        public int ProdutoID { get; set; }
		[Required]				
		public string Descricao { get; set; }
        [Required]
        public string Codigo { get; set; }
        public double Valor { get; set; }
        public double Quantidade { get; set; }
        public bool Ativo { get; set; }
        public DateTime? DataCriacao { get; set; }
        public DateTime? DataModificacao { get; set; }
    }	
}