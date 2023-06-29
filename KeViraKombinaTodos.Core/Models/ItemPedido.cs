using System;

namespace KeViraKombinaTodos.Core.Models {
	public class ItemPedido {
        #region Public Properties
        public int PedidoID { get; set; }
        public int? ProdutoID { get; set; }
        public decimal? Preco { get; set; }
        public decimal? Quantidade { get; set; }
        public string Descricao { get; set; }
        public string Codigo { get; set; }
        public DateTime? DataCriacao { get; set; }
        public DateTime? DataModificacao { get; set; }
        #endregion
    }
}
