using System.Collections.Generic;
using KeViraKombinaTodos.Core.Models;

namespace KeViraKombinaTodos.Core.Services {
	public interface IBaseCopiaService {

		#region Methods
		IList<object> CarregarTodos(int workflowID);
		int Cadastrar(object obj);
		object Carregar(int key);		
		void Atualizar(object obj);
        void Excluir(int key);
        void Desativar(int key);        
        #endregion
    }
}
