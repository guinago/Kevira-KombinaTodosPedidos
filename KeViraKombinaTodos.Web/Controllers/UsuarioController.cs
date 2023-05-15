using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using KeViraKombinaTodos.Core.Models;
using KeViraKombinaTodos.Core.Services;
using KeViraKombinaTodos.Web.Extensions;
using KeViraKombinaTodos.Web.Models;

namespace KeViraKombinaTodos.Web.Controllers {
	[AuthorizeAttribute]
	public class UsuarioController : Controller {

		#region Inject
		private IUsuarioService _usuariosService;
		private IPerfilService _perfilService;		
		public UsuarioController(IUsuarioService usuariosService, IPerfilService perfilService)
        {
			_perfilService = perfilService ?? throw new ArgumentException(nameof(perfilService));
			_usuariosService = usuariosService ?? throw new ArgumentException(nameof(usuariosService));			
		}
        #endregion

        #region Actions

        public ActionResult Index()
        {
            IList<UsuarioModel> model = new List<UsuarioModel>();
            IList<Usuario> usuarios = _usuariosService.CarregarUsuarios();

            foreach (var item in usuarios)
            {
                UsuarioModel modelUsuario = new UsuarioModel();

                modelUsuario = ConverterTiposObjetosUsuarioParaUsuarioViewModel(item);
                model.Add(modelUsuario);
            }

            return View(model);
        }

        public ActionResult Details(int usuarioID)
        {
            UsuarioModel model = CarregarUsuario(usuarioID);

            if (model == null)
                return RedirectToAction("Index");

            return View(model);
        }

        //      [HttpPost]
        //public ActionResult Create(UsuarioModel model) {
        //	try {

        //              Usuario usuario = ConverterTiposObjetosUsuarioViewModelParaUsuario(model);

        //              usuarioID = _usuariosService.CriarUsuario(usuario);

        //          } catch {
        //		return View(model);
        //	}
        //	return RedirectToAction("Index");
        //}

        public ActionResult Edit(int usuarioID)
        {
            return View(CarregarUsuario(usuarioID));
        }

        [HttpPost]
		public ActionResult Edit(UsuarioModel model) {
			try {

                _usuariosService.AtualizarUsuario(ConverterTiposObjetosUsuarioViewModelParaUsuario(model));
                _usuariosService.AtualizarAspNetUsers(ConverterTiposObjetosUsuarioViewModelParaAspNetUsers(model));

                return RedirectToAction("Index");
			} catch {
				return View(model);
			}
		}
        #endregion

        #region Methods

        private AspNetUsers ConverterTiposObjetosUsuarioViewModelParaAspNetUsers(UsuarioModel model)
        {
            AspNetUsers user = new AspNetUsers();
            user.ID = model.UsuarioID;
            user.Nome = model.Nome;
            user.Email = model.Email;
            user.PhoneNumber = model.Telefone;

            return user;
        }

        private UsuarioModel ConverterTiposObjetosUsuarioParaUsuarioViewModel(Usuario usuario) {
			UsuarioModel model = new UsuarioModel();

			PropertyCopier<Usuario, UsuarioModel>.Copy(usuario, model);

			return model;
		}
		private Usuario ConverterTiposObjetosUsuarioViewModelParaUsuario(UsuarioModel model) {
            Usuario usuario = new Usuario();
			PropertyCopier<UsuarioModel, Usuario>.Copy(model, usuario);

			return usuario;
		}

		private UsuarioModel CarregarUsuario(int usuarioID) {
			UsuarioModel model = new UsuarioModel();

            Usuario usuario = _usuariosService.CarregarUsuario(usuarioID);

			if (usuario == null) {
				return null;
			}

			model = ConverterTiposObjetosUsuarioParaUsuarioViewModel(usuario);			

			return model;
		}


		#endregion
	}
}