using KeViraKombinaTodos.Core;
using KeViraKombinaTodos.Core.DAO;
using KeViraKombinaTodos.Core.Models;
using KeViraKombinaTodos.Core.Services;
using System;
using System.Collections;
using System.Collections.Generic;

namespace KeViraKombinaTodos.Impl.Services {
	[Component]
	public class TransportadoraService : ITransportadoraService {

		#region Private Read-Only Fields

		private readonly ITransportadoraDao _TransportadoraDao;

		#endregion

		#region Public Constructors

		public TransportadoraService(ITransportadoraDao TransportadoraDao) {
			_TransportadoraDao = TransportadoraDao ?? throw new ArgumentNullException(nameof(TransportadoraDao));
		}

		#endregion

		#region Members
		public IList<Transportadora> CarregarTransportadoras() {
			IList<Transportadora> Transportadoras = _TransportadoraDao.CarregarTransportadoras();

			return Transportadoras;
		}
		public int CriarTransportadora(Transportadora Transportadora) {
			return _TransportadoraDao.CriarTransportadora(Transportadora);		
		}
		public Transportadora CarregarTransportadora(int TransportadoraID) {
			return _TransportadoraDao.CarregarTransportadora(TransportadoraID);
		}
		public void DesativarTransportadora(int TransportadoraID) {
			_TransportadoraDao.DesativarTransportadora(TransportadoraID);
		}
		public void AtualizarTransportadora(Transportadora Transportadora) {
			_TransportadoraDao.AtualizarTransportadora(Transportadora);
		}
		public void ExcluirTransportadora(int TransportadoraID) {
			_TransportadoraDao.ExcluirTransportadora(TransportadoraID);
		}
		#endregion
	}
}