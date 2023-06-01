using System.Collections.Generic;
using KeViraKombinaTodos.Core.Models;

namespace KeViraKombinaTodos.Core.DAO {
	public interface IItemPedidoDao {
		IList<ItemPedido> CarregarItensPedido(int PedidoID);
		int CriarItemPedido(ItemPedido Pedidos);
		ItemPedido CarregarItemPedido(int PedidoID);
        void AtualizarItemPedido(ItemPedido Pedidos);
        void ExcluirItemPedido(int PedidoID, int ProdutoID);
    }
}
