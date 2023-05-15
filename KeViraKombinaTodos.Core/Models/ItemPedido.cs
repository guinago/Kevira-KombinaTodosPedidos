using System;

namespace KeViraKombinaTodos.Core.Models {
	public class ItemPedido {
        #region Public Properties
        public int PedidoID { get; set; }
        public int? ProdutoID { get; set; }
        public double? Preco { get; set; }
        public double? Quantidade { get; set; }
        public string Descricao { get; set; }
        public DateTime? DataCriacao { get; set; }
        public DateTime? DataModificacao { get; set; }
        #endregion
    }
}
