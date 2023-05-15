using System.Collections.Generic;
using KeViraKombinaTodos.Core.Models;

namespace KeViraKombinaTodos.Core.DAO {
	public interface ICondicaoPagamentoDao {
        IList<CondicaoPagamento> CarregarCondicoesPagamento();
        int CriarCondicaoPagamento(CondicaoPagamento CondicaoPagamento);
        CondicaoPagamento CarregarCondicaoPagamento(int CondicaoPagamentoID);
        void DesativarCondicaoPagamento(int CondicaoPagamentoID);
        void ExcluirCondicaoPagamento(int CondicaoPagamentoID);
        void AtualizarCondicaoPagamento(CondicaoPagamento CondicaoPagamento);
    }
}