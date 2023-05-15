using KeViraKombinaTodos.Core;
using KeViraKombinaTodos.Core.DAO;
using KeViraKombinaTodos.Core.Models;
using KeViraKombinaTodos.Core.Services;
using System;
using System.Collections;
using System.Collections.Generic;

namespace KeViraKombinaTodos.Impl.Services {
	[Component]
	public class PedidoService : IPedidoService {

		#region Private Read-Only Fields

		private readonly IPedidoDao _pedidoDao;
        private readonly IItemPedidoDao _itemPedidoDao;
        #endregion

        #region Public Constructors

        public PedidoService(IPedidoDao pedidoDao, IItemPedidoDao itemPedidoDao) {
			_pedidoDao = pedidoDao ?? throw new ArgumentNullException(nameof(pedidoDao));
            _itemPedidoDao = itemPedidoDao ?? throw new ArgumentNullException(nameof(itemPedidoDao));
        }

		#endregion

		#region IPedidosService Members

		public int CriarPedido(Pedido Pedido) {
			return _pedidoDao.CriarPedido(Pedido);
		}
		public IList<Pedido> CarregarPedidos(int vendedorID) {
			return _pedidoDao.CarregarPedidos(vendedorID);
		}
		public Pedido CarregarPedido(int PedidoID) {
			return _pedidoDao.CarregarPedido(PedidoID);
		}

        public void AtualizarPedido(Pedido Pedido){
            _pedidoDao.AtualizarPedido(Pedido);
        }

        public int CriarItemPedido(ItemPedido Pedido)
        {
            return _itemPedidoDao.CriarItemPedido(Pedido);
        }

        public IList<ItemPedido> CarregarItensPedido(int PedidoID)
        {
            return _itemPedidoDao.CarregarItensPedido(PedidoID);
        }
        #endregion
    }
}