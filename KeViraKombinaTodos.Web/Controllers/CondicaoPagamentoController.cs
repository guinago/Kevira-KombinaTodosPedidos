using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using KeViraKombinaTodos.Core.Models;
using KeViraKombinaTodos.Core.Services;
using KeViraKombinaTodos.Web.Extensions;
using KeViraKombinaTodos.Web.Models;
using Newtonsoft.Json.Linq;

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

		public ActionResult Create() {
			return View(new CondicaoPagamentoModel());
		}

		[HttpPost]
		public ActionResult Create(CondicaoPagamentoModel model) {
			int condicaoPagamentoID = 0;
			try {                 
                CondicaoPagamento condicaoPagamento = ConverterTiposObjetosCondicaoPagamentoViewModelParaCondicaoPagamento(model);

                condicaoPagamentoID = _condicaoPagamentoService.CriarCondicaoPagamento(condicaoPagamento);

            } catch (Exception ex){
                ModelState.AddModelError(ex.Message, "Erro ao criar condição de pagamento");
                return View(model);
			}
			return RedirectToAction("Index");
		}

		[HttpPost]
		public ActionResult Edit(int condicaoPagamentoID, string propertyName, string value) {
            var status = false;
            var message = "";
            CondicaoPagamentoModel model = new CondicaoPagamentoModel();
            model.CondicaoPagamentoID = condicaoPagamentoID;

            if (propertyName == "Codigo")
            {
                model.Codigo = value;
            }
            else
                model.Descricao = value;

            try {
                _condicaoPagamentoService.AtualizarCondicaoPagamento(ConverterTiposObjetosCondicaoPagamentoViewModelParaCondicaoPagamento(model));
                status = true;
			} catch (Exception ex){
                message = ex.Message;
                ModelState.AddModelError(message, "Erro ao atualizar condição de pagamento");
            }

            var response = new { value = value, status = status, message = message };
            JObject o = JObject.FromObject(response);
            return Content(o.ToString());
        }

        public ActionResult Excluir(int condicaoPagamentoID, int value = 0) {
            var message = "";

            try
            {
                _condicaoPagamentoService.ExcluirCondicaoPagamento(condicaoPagamentoID);
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

		private CondicaoPagamentoModel ConverterTiposObjetosCondicaoPagamentoParaCondicaoPagamentoViewModel(CondicaoPagamento CondicaoPagamento) {
			CondicaoPagamentoModel model = new CondicaoPagamentoModel();

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