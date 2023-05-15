using System.Collections.Generic;
using KeViraKombinaTodos.Core.Models;

namespace KeViraKombinaTodos.Core.DAO {
	public interface IPerfilDao {
		int CriarPerfil(Perfil Perfil);
		IList<Perfil> CarregarPerfis();
		Perfil CarregarPerfil(int perfilID);
		void AtualizarPerfil(Perfil Perfil);	
		void ExcluirPerfil(int perfilID);	
	}
}
