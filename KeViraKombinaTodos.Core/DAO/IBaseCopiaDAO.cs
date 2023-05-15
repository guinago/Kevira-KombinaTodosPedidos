using System.Collections.Generic;
using KeViraKombinaTodos.Core.Models;

namespace KeViraKombinaTodos.Core.DAO {
	public interface IBaseCopiaDao {
		IList<object> CarregarTodos(int workflowID);
		int Cadastrar(object obj);
        object Carregar(int key);
		void Desativar(int key);
		void Atualizar(object obj);
		void Excluir(int key);
	}
}