using KeViraKombinaTodos.Core;
using KeViraKombinaTodos.Core.DAO;
using KeViraKombinaTodos.Core.Models;
using KeViraKombinaTodos.Core.Services;
using System;
using System.Collections;
using System.Collections.Generic;

namespace KeViraKombinaTodos.Impl.Services {
	[Component]
	public class BaseCopiaService : IBaseCopiaService {

		#region Private Read-Only Fields

		private readonly IBaseCopiaDao _BaseCopiaDao;

		#endregion

		#region Public Constructors

		public BaseCopiaService(IBaseCopiaDao BaseCopiaDao) {
            _BaseCopiaDao = BaseCopiaDao ?? throw new ArgumentNullException(nameof(BaseCopiaDao));
		}

		#endregion

		#region Members
		public IList<object> CarregarTodos(int workflowID) {
			return _BaseCopiaDao.CarregarTodos(workflowID);
		}
		public int Cadastrar(object obj) {
            return _BaseCopiaDao.Cadastrar(obj);
		}
		public object Carregar(int key) {
			return _BaseCopiaDao.Carregar(key);
		}
		public void Atualizar(object obj) {
            _BaseCopiaDao.Atualizar(obj);
        }
        public void Excluir(int key) {
            _BaseCopiaDao.Excluir(key);
        }
        public void Desativar(int key) {
            _BaseCopiaDao.Desativar(key);
        }      
        #endregion
    }
}