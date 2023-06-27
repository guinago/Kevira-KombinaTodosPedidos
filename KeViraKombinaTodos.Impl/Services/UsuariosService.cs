using KeViraKombinaTodos.Core;
using KeViraKombinaTodos.Core.DAO;
using KeViraKombinaTodos.Core.Models;
using KeViraKombinaTodos.Core.Services;
using System;
using System.Collections.Generic;

namespace KeViraKombinaTodos.Impl.Services {
	[Component]
	public class UsuariosService : IUsuarioService {

		#region Private Read-Only Fields

		private readonly IAspNetUsersDao _AspNetUsersDao;

        #endregion

        #region Public Constructors

        public UsuariosService(IAspNetUsersDao aspNetUsersDao) {
			_AspNetUsersDao = aspNetUsersDao ?? throw new ArgumentNullException(nameof(aspNetUsersDao));
        }

		#endregion

		#region Members

		public int AtualizarColunaIDMaster(string email) {
			return _AspNetUsersDao.AtualizarColunaIDMaster(email);
		}

		public void AtualizarPerfilUsuario(int idUser, int perfilID) {
			_AspNetUsersDao.AtualizarPerfilUsuario(idUser, perfilID);
		}
		public IList<AspNetUsers> CarregarUsuarios() {
			return _AspNetUsersDao.CarregarUsuarios();
		}
		public AspNetUsers CarregarUsuario(int usuarioID) {
			return _AspNetUsersDao.CarregarUsuario(usuarioID);
		}
        public void CriarNovoUsuarioWorkflow(AspNetUsers objeto) {
			_AspNetUsersDao.CriarNovoUsuarioWorkflow(objeto);
		}
        public void AtualizarAspNetUsers(AspNetUsers objeto)
        {
            _AspNetUsersDao.AtualizarUsuario(objeto);
        }
		public void ExcluirUsuario(int usuarioID) {
			_AspNetUsersDao.ExcluirUsuario(usuarioID);
		}
        public void DesativarUsuario(int usuarioID) {
			_AspNetUsersDao.DesativarUsuario(usuarioID);
		}
		public void NovaSenhaUsuario(int usuarioID) {
			_AspNetUsersDao.NovaSenhaUsuario(usuarioID);
		}

		#endregion
	}
}