using System.Collections.Generic;
using KeViraKombinaTodos.Core.Models;

namespace KeViraKombinaTodos.Core.Services {
	public interface ITransportadoraService {

		#region Methods
		IList<Transportadora> CarregarTransportadoras();
		int CriarTransportadora(Transportadora Transportadora);
		Transportadora CarregarTransportadora(int TransportadoraID);
		void DesativarTransportadora(int TransportadoraID);
		void AtualizarTransportadora(Transportadora Transportadora);
		void ExcluirTransportadora(int TransportadoraID);
		#endregion
	}
}
