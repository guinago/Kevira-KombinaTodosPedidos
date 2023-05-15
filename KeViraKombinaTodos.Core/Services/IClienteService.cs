using System.Collections.Generic;
using KeViraKombinaTodos.Core.Models;

namespace KeViraKombinaTodos.Core.Services {
	public interface IClienteService {

        #region Methods
        int CriarCliente(Cliente Cliente);
        IList<Cliente> CarregarClientes();
        Cliente CarregarCliente(int usuarioID);
        void AtualizarCliente(Cliente usuario);
        //////void ExcluirCliente(int usuarioID);

        #endregion
    }
}
