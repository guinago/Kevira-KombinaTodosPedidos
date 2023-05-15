using System;
using System.Collections.Generic;
using System.Web.Mvc;
using KeViraKombinaTodos.Core.Models;
using KeViraKombinaTodos.Core.Services;
using KeViraKombinaTodos.Web.Extensions;
using KeViraKombinaTodos.Web.Models;

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
		public ActionResult Details(int perfilID) {
			PerfilModel model = CarregarPerfil(perfilID);

			if (model == null)
				return RedirectToAction("Index");
			
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

			} catch {
				return View(model);
			}
			return RedirectToAction("Details", new { perfilID = perfilID });
		}
		public ActionResult Edit(int perfilID) {
			return View(CarregarPerfil(perfilID));
		}

		[HttpPost]
		public ActionResult Edit(PerfilModel model) {
			try {
				_perfilService.AtualizarPerfil(ConverterTiposObjetosPerfilViewModelParaPerfil(model));

				return RedirectToAction("Index");
			} catch {
				return View(model);
			}
		}		

		public ActionResult Excluir(int perfilID) {

            _perfilService.ExcluirPerfil(perfilID);

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