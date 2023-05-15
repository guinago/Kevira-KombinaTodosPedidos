using System.Collections.Generic;
using KeViraKombinaTodos.Core.Models;

namespace KeViraKombinaTodos.Core.DAO {
	public interface IProdutoDao
    {
		IList<Produto> CarregarProdutos();
		int CriarProduto(Produto obj);
        Produto CarregarProduto(int ProdutoID);		
		void AtualizarProduto(Produto obj);
		void ExcluirProduto(int ProdutoID);
	}
}