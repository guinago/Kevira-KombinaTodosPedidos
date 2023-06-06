using System;
using System.Collections.Generic;
using System.Web.Mvc;
using KeViraKombinaTodos.Core.Models;
using KeViraKombinaTodos.Core.Services;
using KeViraKombinaTodos.Web.Extensions;
using KeViraKombinaTodos.Web.Models;
using Newtonsoft.Json.Linq;

namespace KeViraKombinaTodos.Web.Controllers {
	[AuthorizeAttribute]
	public class PerfilController : Controller {

		#region Inject
		private IPerfilService _perfilService;
		public PerfilController(IPerfilService perfilService) {
			_perfilService = perfilService ?? throw new ArgumentNullException(nameof(perfilService));
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

				perfilID = _perfilService.CriarPerfil(perfil);

			} catch (Exception ex){
                ModelState.AddModelError(ex.Message, "Erro ao criar Perfil");
                return View(model);
			}
			return RedirectToAction("Index");
		}

        [HttpPost]
        public ActionResult Edit(int perfilID, string propertyName, string value)
        {
            var status = false;
            var message = "";
            var produto = _perfilService.CarregarPerfil(perfilID);
            PerfilModel model = new PerfilModel();
            model.SouComprador = produto.SouComprador;
            model.SouTodoPoderoso = produto.SouTodoPoderoso;
            model.SouTransportador = produto.SouTransportador;
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
                status = true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                ModelState.AddModelError(message, "Erro ao atualizar perfil");
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
            }
            catch (Exception ex)
            {
                message = ex.Message;
                ModelState.AddModelError(message, "Erro ao atualizar perfil");
            }
            return RedirectToAction("Index");
        }

        public ActionResult Excluir(int perfilID, int value = 0)
        {
            var message = "";

            try
            {
                _perfilService.ExcluirPerfil(perfilID);
            }
            catch (Exception ex)
            {
                message = ex.Message;
                ModelState.AddModelError(message, "Erro ao atualizar condição de pagamento");
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
		#endregion
	}
}