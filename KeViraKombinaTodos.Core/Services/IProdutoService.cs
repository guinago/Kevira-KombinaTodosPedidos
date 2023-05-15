using System.Collections.Generic;
using KeViraKombinaTodos.Core.Models;

namespace KeViraKombinaTodos.Core.Services {
	public interface IProdutoService {

		#region Methods
		IList<Produto> CarregarProdutos();
		int CriarProduto(Produto produto);
		Produto CarregarProduto(int produtoID);		
		void AtualizarProduto(Produto produto);
		void ExcluirProduto(int produtoID);
		#endregion
	}
}
