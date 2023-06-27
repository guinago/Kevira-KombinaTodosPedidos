using System;
using System.ComponentModel.DataAnnotations;

namespace KeViraKombinaTodos.Web.Models {
    public class PerfilModel {
        public int PerfilID { get; set; }
        [Required, MinLength(2), Display(Name = "Descrição")]				
		public string Descricao { get; set; }
		public string Codigo { get; set; }
		public bool SouTodoPoderoso { get; set; }
        public bool SouComprador { get; set; }
        public bool SouTransportador { get; set; }
        public DateTime? Datacriacao { get; set; }
        public DateTime? DataModificacao { get; set; }
    }
}