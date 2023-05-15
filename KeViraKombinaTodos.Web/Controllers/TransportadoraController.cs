using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using KeViraKombinaTodos.Core.Models;
using KeViraKombinaTodos.Core.Services;
using KeViraKombinaTodos.Web.Extensions;
using KeViraKombinaTodos.Web.Models;

namespace KeViraKombinaTodos.Web.Controllers {
	[AuthorizeAttribute]
	public class TransportadoraController : Controller {

		#region Inject
		private ITransportadoraService _transportadoraService;
		public TransportadoraController(ITransportadoraService TransportadoraService) {
			_transportadoraService = TransportadoraService ?? throw new ArgumentNullException(nameof(TransportadoraService));
		}
		#endregion

		#region Actions
		public ActionResult Index() {
			IList<TransportadoraModel> model = new List<TransportadoraModel>();

            IList<Transportadora> transportadora = _transportadoraService.CarregarTransportadoras();

            foreach (var item in transportadora)
            {
                TransportadoraModel modelTransportadora = new TransportadoraModel();

                modelTransportadora = ConverterTiposObjetosTransportadoraParaTransportadoraViewModel(item);
                model.Add(modelTransportadora);
            }

            return View(model);
		}
		public ActionResult Details(int transportadoraID) {
			TransportadoraModel model = CarregarTransportadora(transportadoraID);

			if (model == null)
				return RedirectToAction("Index");
			
			return View(model);
		}

		public ActionResult Create() {
			return View(new TransportadoraModel());
		}

		[HttpPost]
		public ActionResult Create(TransportadoraModel model) {
			int transportadoraID = 0;
			try {
                Transportadora transportadora = ConverterTiposObjetosTransportadoraViewModelParaTransportadora(model);

                transportadoraID = _transportadoraService.CriarTransportadora(transportadora);

            } catch {
				return View(model);
			}
			return RedirectToAction("Details", new { transportadoraID = transportadoraID });
		}
		public ActionResult Edit(int transportadoraID) {
			return View(CarregarTransportadora(transportadoraID));
		}

		[HttpPost]
		public ActionResult Edit(TransportadoraModel model) {
			try {
                _transportadoraService.AtualizarTransportadora(ConverterTiposObjetosTransportadoraViewModelParaTransportadora(model));

                return RedirectToAction("Index");
			} catch {
				return View(model);
			}
		}
		public ActionResult Excluir(int transportadoraID) {

            _transportadoraService.ExcluirTransportadora(transportadoraID);

            return RedirectToAction("Index");
		}
		#endregion

		#region Methods

		private TransportadoraModel ConverterTiposObjetosTransportadoraParaTransportadoraViewModel(Transportadora Transportadora) {
			TransportadoraModel model = new TransportadoraModel();

			PropertyCopier<Transportadora, TransportadoraModel>.Copy(Transportadora, model);

			return model;
		}
		private Transportadora ConverterTiposObjetosTransportadoraViewModelParaTransportadora(TransportadoraModel model) {
            Transportadora Transportadora = new Transportadora();
			PropertyCopier<TransportadoraModel, Transportadora>.Copy(model, Transportadora);
			

			return Transportadora;
		}

		private TransportadoraModel CarregarTransportadora(int transportadoraID) {
			TransportadoraModel model = new TransportadoraModel();

			Transportadora Transportadora = _transportadoraService.CarregarTransportadora(transportadoraID);

			if (Transportadora == null) {
				return null;
			}

			model = ConverterTiposObjetosTransportadoraParaTransportadoraViewModel(Transportadora);
			
			return model;
		}
		

		#endregion
	}
}