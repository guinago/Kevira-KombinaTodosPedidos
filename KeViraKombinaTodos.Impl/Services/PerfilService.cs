using KeViraKombinaTodos.Core;
using KeViraKombinaTodos.Core.DAO;
using KeViraKombinaTodos.Core.Models;
using KeViraKombinaTodos.Core.Services;
using System;
using System.Collections;
using System.Collections.Generic;

namespace KeViraKombinaTodos.Impl.Services {
	[Component]
	public class PerfilService : IPerfilService {

		#region Private Read-Only Fields

		private readonly IPerfilDao _perfilDao;

		#endregion

		#region Public Constructors

		public PerfilService(IPerfilDao perfilDao) {
			_perfilDao = perfilDao ?? throw new ArgumentNullException(nameof(perfilDao));
		}

		#endregion

		#region IPerfilService Members

		public int CriarPerfil(Perfil perfil) {
			return _perfilDao.CriarPerfil(perfil);
		}

		public IList<Perfil> CarregarPerfis() {
			return _perfilDao.CarregarPerfis();
		}

		public Perfil CarregarPerfil(int perfilID) {
			return _perfilDao.CarregarPerfil(perfilID);
		}

		public void AtualizarPerfil(Perfil perfil) {
			_perfilDao.AtualizarPerfil(perfil);
		}
		public void ExcluirPerfil(int perfilID) {
			_perfilDao.ExcluirPerfil(perfilID);
		}
		#endregion

		#region Methods private

		#endregion
	}
}