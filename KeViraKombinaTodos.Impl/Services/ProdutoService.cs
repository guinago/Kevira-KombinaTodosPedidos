using KeViraKombinaTodos.Core;
using KeViraKombinaTodos.Core.DAO;
using KeViraKombinaTodos.Core.Models;
using KeViraKombinaTodos.Core.Services;
using System;
using System.Collections;
using System.Collections.Generic;

namespace KeViraKombinaTodos.Impl.Services {
	[Component]
	public class ProdutoService : IProdutoService {

		#region Private Read-Only Fields

		private readonly IProdutoDao _produtoDao;

		#endregion

		#region Public Constructors

		public ProdutoService(IProdutoDao produtoDao) {
			_produtoDao = produtoDao ?? throw new ArgumentNullException(nameof(produtoDao));
		}

		#endregion

		#region Members
		public IList<Produto> CarregarProdutos() {
			IList<Produto> produtos = _produtoDao.CarregarProdutos();

			return produtos;
		}
		public int CriarProduto(Produto produto) {
			return _produtoDao.CriarProduto(produto);		
		}
		public Produto CarregarProduto(int produtoID) {
			return _produtoDao.CarregarProduto(produtoID);
		}
		public void AtualizarProduto(Produto produto) {
			_produtoDao.AtualizarProduto(produto);
		}
		public void ExcluirProduto(int produtoID) {
			_produtoDao.ExcluirProduto(produtoID);
		}
		#endregion
	}
}