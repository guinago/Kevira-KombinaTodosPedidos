using System.Collections.Generic;
using KeViraKombinaTodos.Core.Models;

namespace KeViraKombinaTodos.Core.DAO {
	public interface IPedidoDao {
		IList<Pedido> CarregarPedidos(int vendedorID);
		int CriarPedido(Pedido Pedidos);
		Pedido CarregarPedido(int PedidoID);
        void AtualizarPedido(Pedido Pedidos);
    }
}
