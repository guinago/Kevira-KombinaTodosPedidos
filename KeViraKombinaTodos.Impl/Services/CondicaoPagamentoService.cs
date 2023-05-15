using KeViraKombinaTodos.Core;
using KeViraKombinaTodos.Core.DAO;
using KeViraKombinaTodos.Core.Models;
using KeViraKombinaTodos.Core.Services;
using System;
using System.Collections;
using System.Collections.Generic;

namespace KeViraKombinaTodos.Impl.Services {
	[Component]
	public class CondicaoPagamentoService : ICondicaoPagamentoService {

		#region Private Read-Only Fields

		private readonly ICondicaoPagamentoDao _CondicaoPagamentoDao;

		#endregion

		#region Public Constructors

		public CondicaoPagamentoService(ICondicaoPagamentoDao CondicaoPagamentoDao) {
			_CondicaoPagamentoDao = CondicaoPagamentoDao ?? throw new ArgumentNullException(nameof(CondicaoPagamentoDao));
		}

		#endregion

		#region Members
		public IList<CondicaoPagamento> CarregarCondicoesPagamento() {
			IList<CondicaoPagamento> CondicaoPagamentos = _CondicaoPagamentoDao.CarregarCondicoesPagamento();

			return CondicaoPagamentos;
		}

		public int CriarCondicaoPagamento(CondicaoPagamento CondicaoPagamento) {
			return _CondicaoPagamentoDao.CriarCondicaoPagamento(CondicaoPagamento);		
		}

		public CondicaoPagamento CarregarCondicaoPagamento(int CondicaoPagamentoID) {
			return _CondicaoPagamentoDao.CarregarCondicaoPagamento(CondicaoPagamentoID);
		}

		public void DesativarCondicaoPagamento(int CondicaoPagamentoID) {
			_CondicaoPagamentoDao.DesativarCondicaoPagamento(CondicaoPagamentoID);
		}

		public void AtualizarCondicaoPagamento(CondicaoPagamento CondicaoPagamento) {
			_CondicaoPagamentoDao.AtualizarCondicaoPagamento(CondicaoPagamento);
		}
		public void ExcluirCondicaoPagamento(int CondicaoPagamentoID) {
			_CondicaoPagamentoDao.ExcluirCondicaoPagamento(CondicaoPagamentoID);
		}
		#endregion
	}
}