using KeViraKombinaTodos.Core;
using KeViraKombinaTodos.Core.DAO;
using KeViraKombinaTodos.Core.Models;
using KeViraKombinaTodos.Core.Services;
using System;
using System.Collections;
using System.Collections.Generic;

namespace KeViraKombinaTodos.Impl.Services {
	[Component]
	public class ClienteService : IClienteService {

		#region Private Read-Only Fields

		private readonly IClienteDao _ClienteDao;

		#endregion

		#region Public Constructors

		public ClienteService(IClienteDao ClienteDao) {
            _ClienteDao = ClienteDao ?? throw new ArgumentNullException(nameof(ClienteDao));
		}

		#endregion

		#region Members
		public IList<Cliente> CarregarClientes() {
			IList<Cliente> Clientes = _ClienteDao.CarregarClientes();
			return Clientes;
		}
		public int CriarCliente(Cliente Cliente) {
			return _ClienteDao.CriarCliente(Cliente);		
		}
		public Cliente CarregarCliente(int ClienteID) {
			return _ClienteDao.CarregarCliente(ClienteID);
		}

		public void AtualizarCliente(Cliente Cliente) {
            _ClienteDao.AtualizarCliente(Cliente);
		}
		////public void ExcluirCliente(int ClienteID) {
        ////    _ClienteDao.ExcluirCliente(ClienteID);
		////}
		#endregion
	}
}