using System.Collections.Generic;
using KeViraKombinaTodos.Core.Models;

namespace KeViraKombinaTodos.Core.DAO {
	public interface IUsuariosDao {

        void CriarUsuario(string email, string cpf);
        IList<Usuario> CarregarUsuarios();
        Usuario CarregarUsuario(int usuarioID);
        void AtualizarUsuario(Usuario usuarioID);
        //void ExcluirUsuario(int usuarioID);

    }
}
