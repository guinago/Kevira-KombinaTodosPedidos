using System.Collections.Generic;
using KeViraKombinaTodos.Core.Models;

namespace KeViraKombinaTodos.Core.Services {
	public interface IUsuarioService {

		#region Methods
		IList<AspNetUsers> CarregarUsuarios();
        AspNetUsers CarregarUsuario(int usuarioID);
        void AtualizarAspNetUsers(AspNetUsers usuario);
        void ExcluirUsuario(int usuarioID);
		void DesativarUsuario(int usuarioID);
		void NovaSenhaUsuario(int usuarioID);

        #endregion
    }
}
