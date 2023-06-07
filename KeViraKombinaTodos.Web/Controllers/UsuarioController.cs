using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using KeViraKombinaTodos.Core.Models;
using KeViraKombinaTodos.Core.Services;
using KeViraKombinaTodos.Web.Extensions;
using KeViraKombinaTodos.Web.Models;
using Newtonsoft.Json.Linq;

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
            IList<AspNetUsers> users = _usuariosService.CarregarUsuarios();

            foreach (var item in users)
            {
                UsuarioModel modelUsuario = new UsuarioModel();

                modelUsuario = ConverterTiposObjetosUsuarioParaUsuarioViewModel(item);
                model.Add(modelUsuario);
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, string propertyName, string value)
        {
            var status = false;
            var message = "";
            UsuarioModel model = new UsuarioModel();
            model.UsuarioID = id;
            model.Status = _usuariosService.CarregarUsuario(id).IsEnabled;

            if (propertyName == "Nome")
            {
                model.Nome = value;
            }
            else if (propertyName == "CPF")
            {
                model.CPF = value;
            }
            else if (propertyName == "Telefone")
            {
                model.Telefone =value;
            }
            else if (propertyName == "Email")
            {
                model.Email = value;
            }
            else
                model.PerfilID = Convert.ToInt32(value);

            try
            {
                _usuariosService.AtualizarAspNetUsers(ConverterTiposObjetosUsuarioViewModelParaAspNetUsers(model));
                if(propertyName == "Perfil")
                {
                    value = GetModelPerfil().Where(a => a.Key == model.PerfilID).FirstOrDefault().Value;
                    status = true;
                }else
                    status = true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                ModelState.AddModelError(message, "Erro ao atualizar usuário");
            }

            var response = new { value = value, status = status, message = message };
            JObject o = JObject.FromObject(response);
            return Content(o.ToString());
        }

        public ActionResult GetPerfis(int usuarioID)
        {
            string selectedPerfilID = "";
            StringBuilder sb = new StringBuilder();
            int? perfilUsuario = _usuariosService.CarregarUsuario(usuarioID).PerfilID;

            var listPerfil = GetModelPerfil().OrderBy(a => a.Key).ToList();

            foreach (var item in listPerfil)
            {
                sb.Append(string.Format("'{0}':'{1}',", item.Key, item.Value));
            }

            selectedPerfilID = listPerfil.Where(a => a.Key == perfilUsuario).First().Value;

            sb.Append(string.Format("'selected': '{0}'", selectedPerfilID));
            return Content("{" + sb.ToString() + "}");
        }

        public ActionResult EditCheckd(int usuarioID, int value)
        {
            var message = "";
            UsuarioModel model = new UsuarioModel();
            model.UsuarioID = usuarioID;
            model.Status = Convert.ToBoolean(value);

            try
            {
                _usuariosService.AtualizarAspNetUsers(ConverterTiposObjetosUsuarioViewModelParaAspNetUsers(model));
            }
            catch (Exception ex)
            {
                message = ex.Message;
                ModelState.AddModelError(message, "Erro ao atualizar status usuário");
            }
            return RedirectToAction("Index");
        }

        public ActionResult Excluir(int usuarioID, int value = 0)
        {
            var message = "";

            try
            {
                _usuariosService.ExcluirUsuario(usuarioID);
            }
            catch (Exception ex)
            {
                message = ex.Message;
                ModelState.AddModelError(message, "Erro ao excluir usuário");
            }
            return RedirectToAction("Index");
        }

        #endregion

        #region Methods

        private AspNetUsers ConverterTiposObjetosUsuarioViewModelParaAspNetUsers(UsuarioModel model)
        {
            AspNetUsers user = new AspNetUsers();
            user.Id = model.UsuarioID;
            user.Nome = model.Nome;
            user.CGC = model.CPF;
            user.Email = model.Email;
            user.PhoneNumber = model.Telefone;
            user.IsEnabled = model.Status.GetValueOrDefault();
            user.PerfilID = model.PerfilID.GetValueOrDefault();

            return user;
        }

        private UsuarioModel ConverterTiposObjetosUsuarioParaUsuarioViewModel(AspNetUsers user) {
			UsuarioModel model = new UsuarioModel();
            PerfilModel modelPerfil = new PerfilModel();

            model.UsuarioID = user.Id;
            model.Nome = user.Nome;
            model.CPF = user.CGC;
            model.Telefone = user.PhoneNumber;
            model.Email = user.Email;
            model.Status = user.IsEnabled;
            model.DataCriacao = user.DataCadastro;
            model.PerfilID = user.PerfilID;            
            //model.ListPerfil = GetModelPerfil();
            model.Perfil = GetModelPerfil().Where(p => p.Key == user.PerfilID).FirstOrDefault().Value;

            return model;
		}

        public IDictionary<int, string> GetModelPerfil()
        {
            var result = new Dictionary<int, string>();
            result.Add(0, "Selecione");

            var perfis = (List<Perfil>)_perfilService.CarregarPerfis();

            perfis.ForEach(d => result.Add(d.PerfilID, d.Descricao));

            return result;
        }
        private Usuario ConverterTiposObjetosUsuarioViewModelParaUsuario(UsuarioModel model) {
            Usuario usuario = new Usuario();
			PropertyCopier<UsuarioModel, Usuario>.Copy(model, usuario);

			return usuario;
		}

		private UsuarioModel CarregarUsuario(int usuarioID) {
			UsuarioModel model = new UsuarioModel();

            //Usuario usuario = _usuariosService.CarregarUsuario(usuarioID);

			//if (usuario == null) {
			//	return null;
			//}

			//model = ConverterTiposObjetosUsuarioParaUsuarioViewModel(usuario);			

			return model;
		}


		#endregion
	}
}