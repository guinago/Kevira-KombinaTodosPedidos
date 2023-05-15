using System.Collections.Generic;
using KeViraKombinaTodos.Core.Models;

namespace KeViraKombinaTodos.Core.DAO {
	public interface IClienteDao {

        int CriarCliente(Cliente Cliente);
        IList<Cliente> CarregarClientes();
        Cliente CarregarCliente(int clienteID);
        void AtualizarCliente(Cliente clienteID);
        //void ExcluirCliente(int clienteID);

    }
}
