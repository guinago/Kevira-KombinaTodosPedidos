using System.Collections.Generic;
using KeViraKombinaTodos.Core.Models;

namespace KeViraKombinaTodos.Core.Services {
	public interface ICondicaoPagamentoService {

		#region Methods
		IList<CondicaoPagamento> CarregarCondicoesPagamento();
		int CriarCondicaoPagamento(CondicaoPagamento CondicaoPagamento);
		CondicaoPagamento CarregarCondicaoPagamento(int CondicaoPagamentoID);
		void DesativarCondicaoPagamento(int CondicaoPagamentoID);
		void AtualizarCondicaoPagamento(CondicaoPagamento CondicaoPagamento);
		void ExcluirCondicaoPagamento(int CondicaoPagamentoID);
		#endregion
	}
}
