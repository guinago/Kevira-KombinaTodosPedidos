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
	public class CondicaoPagamentoController : Controller {

		#region Inject
		private ICondicaoPagamentoService _condicaoPagamentoService;
		public CondicaoPagamentoController(ICondicaoPagamentoService CondicaoPagamentoService) {
			_condicaoPagamentoService = CondicaoPagamentoService ?? throw new ArgumentNullException(nameof(CondicaoPagamentoService));
		}
		#endregion

		#region Actions
		public ActionResult Index() {
			IList<CondicaoPagamentoModel> model = new List<CondicaoPagamentoModel>();

			IList<CondicaoPagamento> condicaoPagamento = _condicaoPagamentoService.CarregarCondicoesPagamento();

			foreach (var item in condicaoPagamento) {
				CondicaoPagamentoModel modelCondicaoPagamento = new CondicaoPagamentoModel();

				modelCondicaoPagamento = ConverterTiposObjetosCondicaoPagamentoParaCondicaoPagamentoViewModel(item);
				model.Add(modelCondicaoPagamento);
			}


			return View(model);
		}
		public ActionResult Details(int condicaoPagamentoID) {
			CondicaoPagamentoModel model = CarregarCondicaoPagamento(condicaoPagamentoID);

			if (model == null)
				return RedirectToAction("Index");
			
			return View(model);
		}

		public ActionResult Create() {
			return View(new CondicaoPagamentoModel());
		}

		[HttpPost]
		public ActionResult Create(CondicaoPagamentoModel model) {
			int condicaoPagamentoID = 0;
			try {

                foreach (var item in model.ListCondPgto.Where(l => l.Key.Equals(model.CondPgto)))
                {
                    model.Descricao = item.Value;
                }
                    
                CondicaoPagamento condicaoPagamento = ConverterTiposObjetosCondicaoPagamentoViewModelParaCondicaoPagamento(model);

                condicaoPagamentoID = _condicaoPagamentoService.CriarCondicaoPagamento(condicaoPagamento);

            } catch {
				return View(model);
			}
			return RedirectToAction("Details", new { condicaoPagamentoID = condicaoPagamentoID });
		}
		public ActionResult Edit(int condicaoPagamentoID) {
			return View(CarregarCondicaoPagamento(condicaoPagamentoID));
		}

		[HttpPost]
		public ActionResult Edit(CondicaoPagamentoModel model) {
			try {

                foreach (var item in model.ListCondPgto.Where(l => l.Key.Equals(model.CondPgto)))
                {
                    model.Descricao = item.Value;
                }

                _condicaoPagamentoService.AtualizarCondicaoPagamento(ConverterTiposObjetosCondicaoPagamentoViewModelParaCondicaoPagamento(model));
                return RedirectToAction("Index");
			} catch {
				return View(model);
			}
		}

		public ActionResult Excluir(int condicaoPagamentoID) {

            _condicaoPagamentoService.ExcluirCondicaoPagamento(condicaoPagamentoID);

            return RedirectToAction("Index");
		}
		#endregion

		#region Methods

		private CondicaoPagamentoModel ConverterTiposObjetosCondicaoPagamentoParaCondicaoPagamentoViewModel(CondicaoPagamento CondicaoPagamento) {
			CondicaoPagamentoModel model = new CondicaoPagamentoModel();

            if(CondicaoPagamento.CondPgto == null)
                CondicaoPagamento.CondPgto = model.ListCondPgto.FirstOrDefault(l => l.Value == CondicaoPagamento.Descricao).Key;

            PropertyCopier<CondicaoPagamento, CondicaoPagamentoModel>.Copy(CondicaoPagamento, model);

			return model;
		}
		private CondicaoPagamento ConverterTiposObjetosCondicaoPagamentoViewModelParaCondicaoPagamento(CondicaoPagamentoModel model) {
            CondicaoPagamento condicaoPagamento = new CondicaoPagamento();

			PropertyCopier<CondicaoPagamentoModel, CondicaoPagamento>.Copy(model, condicaoPagamento);			

			return condicaoPagamento;
		}

		private CondicaoPagamentoModel CarregarCondicaoPagamento(int condicaoPagamentoID) {
			CondicaoPagamentoModel model = new CondicaoPagamentoModel();

			CondicaoPagamento CondicaoPagamento = _condicaoPagamentoService.CarregarCondicaoPagamento(condicaoPagamentoID);

			if (CondicaoPagamento == null) {
				return null;
			}

			model = ConverterTiposObjetosCondicaoPagamentoParaCondicaoPagamentoViewModel(CondicaoPagamento);
			
			return model;
		}
		

		#endregion
	}
}