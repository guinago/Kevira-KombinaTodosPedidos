using System.Collections.Generic;
using KeViraKombinaTodos.Core.Models;

namespace KeViraKombinaTodos.Core.DAO {
	public interface IAspNetUsersDao {
		int AtualizarColunaIDMaster(string email);
		void AtualizarPerfilUsuario(int idUser, int perfilID);
		IList<AspNetUsers> CarregarUsuariosWorkflow(int workflowID);
		AspNetUsers CarregarUsuario(int id);
		void CriarNovoUsuarioWorkflow(AspNetUsers objeto);
		void AtualizarUsuario(AspNetUsers objeto);
		void ExcluirUsuario(int usuarioID);
        void DesativarUsuario(int usuarioID);
		void NovaSenhaUsuario(int usuarioID);
	}
}
