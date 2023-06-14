using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using KeViraKombinaTodos.Core.Models;
using KeViraKombinaTodos.Core.Services;
using KeViraKombinaTodos.Web.Models;
using Newtonsoft.Json.Linq;

namespace KeViraKombinaTodos.Web.Controllers {
	[AuthorizeAttribute]
	public class PerfilController : Controller {

		#region Inject
		private IPerfilService _perfilService;
        private IUsuarioService _usuariosService;
        public PerfilController(IPerfilService perfilService, IUsuarioService usuariosService) {
			_perfilService = perfilService ?? throw new ArgumentNullException(nameof(perfilService));
            _usuariosService = usuariosService ?? throw new ArgumentException(nameof(usuariosService));
        }
		#endregion

		#region Actions
		public ActionResult Index() {
			IList<PerfilModel> model = new List<PerfilModel>();

			IList<Perfil> perfils = _perfilService.CarregarPerfis();

			foreach (var item in perfils)
            {
				PerfilModel modelPerfil = new PerfilModel();

				modelPerfil = ConverterTiposObjetosPerfilParaPerfilViewModel(item);
				model.Add(modelPerfil);
			}
			return View(model);
		}
		public ActionResult Create() {
			return View(new PerfilModel());
		}
		[HttpPost]
		public ActionResult Create(PerfilModel model) {
			int perfilID = 0;

            try {
                Perfil perfil = ConverterTiposObjetosPerfilViewModelParaPerfil(model);
                string message = ValidaCadastro(perfil);

                if (!string.IsNullOrWhiteSpace(message))
                {
                    TempData["error"] = message;
                    return View(model);
                }

                perfilID = _perfilService.CriarPerfil(perfil);
                TempData["success"] = "Perfil salvo com sucesso";
			} catch (Exception ex){
                TempData["error"] = (ex.Message, "Erro ao criar Perfil");
                return View(model);
			}
			return RedirectToAction("Index");
		}
        [HttpPost]
        public ActionResult Edit(int perfilID, string propertyName, string value)
        {
            var status = false;
            var message = "";
            var perfil = _perfilService.CarregarPerfil(perfilID);
            PerfilModel model = new PerfilModel();
            model.SouComprador = perfil.SouComprador;
            model.SouTodoPoderoso = perfil.SouTodoPoderoso;
            model.SouTransportador = perfil.SouTransportador;
            model.PerfilID = perfilID;

            if (propertyName == "Codigo")
            {
                model.Codigo = value;
            }
            else
                model.Descricao = value;

            try
            {
                _perfilService.AtualizarPerfil(ConverterTiposObjetosPerfilViewModelParaPerfil(model));
                TempData["success"] = "Perfil atualizado com sucesso.";
                status = true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                TempData["error"] =  (message, "Erro ao atualizar perfil");
            }
            var response = new { value = value, status = status, message = message };
            JObject o = JObject.FromObject(response);
            return Content(o.ToString());
        }
        public ActionResult EditCheckd(int perfilID, string propertyName, int value)
        {
            var message = "";
            PerfilModel model = new PerfilModel();
            model.PerfilID = perfilID;

            if (propertyName == "SouComprador")
            {
                model.SouComprador = Convert.ToBoolean(value);
            }
            else if (propertyName == "SouTransportador")
                model.SouTransportador = Convert.ToBoolean(value);
            else
                model.SouTodoPoderoso = Convert.ToBoolean(value);

            try
            {
                _perfilService.AtualizarPerfil(ConverterTiposObjetosPerfilViewModelParaPerfil(model));
                TempData["success"] = "Perfil atualizado com sucesso.";
            }
            catch (Exception ex)
            {
                message = ex.Message;
                TempData["error"] = (message, "Erro ao atualizar perfil");
            }
            return RedirectToAction("Index");
        }
        public ActionResult Excluir(int perfilID, int value = 0)
        {       
            try
            {
                var usuario = _usuariosService.CarregarUsuarios().Where(u => u.PerfilID == perfilID).FirstOrDefault();

                if (usuario != null)
                {
                    TempData["error"] = "Não é possível excluir este perfil, pois existem usuários cadastra-dos no sistema com este perfil.";
                    return RedirectToAction("Index");
                }

                _perfilService.ExcluirPerfil(perfilID);
                TempData["success"] = "Perfil excluído com sucesso.";
            }
            catch (Exception ex)
            {
                TempData["error"] = ("Erro ao excluir perfil", ex.Message);
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

		#endregion

		#region Methods
		private PerfilModel ConverterTiposObjetosPerfilParaPerfilViewModel(Perfil perfil) {
			PerfilModel model = new PerfilModel();

			PropertyCopier<Perfil, PerfilModel>.Copy(perfil, model);

			return model;
		}
		private Perfil ConverterTiposObjetosPerfilViewModelParaPerfil(PerfilModel model) {
			Perfil perfil = new Perfil();

			PropertyCopier<PerfilModel, Perfil>.Copy(model, perfil);

			return perfil;
		}
		private PerfilModel CarregarPerfil(int perfilID) {
			PerfilModel model = new PerfilModel();

			Perfil perfil = _perfilService.CarregarPerfil(perfilID);

			if (perfil == null) {
				return null;
			}

			model = ConverterTiposObjetosPerfilParaPerfilViewModel(perfil);

			return model;
		}
        private string ValidaCadastro(Perfil perfil)
        {
            string message = "";
            var descricaoPerfil = _perfilService.CarregarPerfis().Where(p => p.Descricao == perfil.Descricao).FirstOrDefault();

            if (descricaoPerfil != null)
                return message = "Não é possível incluir este perfil, pois já existe perfil cadastrado com esta descrição no sistema";

            return message;
        }
        #endregion
    }
}