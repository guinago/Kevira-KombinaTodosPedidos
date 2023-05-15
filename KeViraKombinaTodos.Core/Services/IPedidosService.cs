using System.Collections.Generic;
using KeViraKombinaTodos.Core.Models;

namespace KeViraKombinaTodos.Core.Services {
	public interface IPedidoService {

		#region Methods
		IList<Pedido> CarregarPedidos(int vendedorID);
		int CriarPedido(Pedido Pedidos);
		Pedido CarregarPedido(int PedidoID);
        void AtualizarPedido(Pedido Pedidos);

        int CriarItemPedido(ItemPedido Pedido);
        IList<ItemPedido> CarregarItensPedido(int PedidoID);

        #endregion
    }
}
