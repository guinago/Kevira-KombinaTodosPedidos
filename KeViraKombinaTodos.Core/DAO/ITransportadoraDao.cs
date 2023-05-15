using System.Collections.Generic;
using KeViraKombinaTodos.Core.Models;

namespace KeViraKombinaTodos.Core.DAO {
	public interface ITransportadoraDao {
		IList<Transportadora> CarregarTransportadoras();
		int CriarTransportadora(Transportadora Transportadora);
		Transportadora CarregarTransportadora(int TransportadoraID);
		void ExcluirTransportadora(int TransportadoraID);
		void AtualizarTransportadora(Transportadora Transportadora);
		void DesativarTransportadora(int TransportadoraID);
	}
}
