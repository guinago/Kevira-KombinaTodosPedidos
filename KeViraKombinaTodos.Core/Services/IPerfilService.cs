using System.Collections.Generic;
using KeViraKombinaTodos.Core.Models;

namespace KeViraKombinaTodos.Core.Services {
	public interface IPerfilService {
		#region Methods
		int CriarPerfil(Perfil perfil);
		IList<Perfil> CarregarPerfis();
		Perfil CarregarPerfil(int perfilID);
		void AtualizarPerfil(Perfil perfil);
		void ExcluirPerfil(int perfilID);

		#endregion
	}
}
