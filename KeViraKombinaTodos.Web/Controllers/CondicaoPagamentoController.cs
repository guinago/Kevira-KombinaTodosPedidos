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
        private IPedidoService _pedidoService;
        public CondicaoPagamentoController(ICondicaoPagamentoService CondicaoPagamentoService, IPedidoService PedidoService) {
			_condicaoPagamentoService = CondicaoPagamentoService ?? throw new ArgumentNullException(nameof(CondicaoPagamentoService));
            _pedidoService = PedidoService ?? throw new ArgumentNullException(nameof(PedidoService));
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
                string message = ValidaCadastro(condicaoPagamento);

                if (!string.IsNullOrWhiteSpace(message))
                {
                    TempData["error"] = message;
                    return View(model);
                }

                condicaoPagamentoID = _condicaoPagamentoService.CriarCondicaoPagamento(condicaoPagamento);
                TempData["success"] = "Condição de Pagamento salva com sucesso";
            } catch (Exception ex){
                TempData["error"] = ("Erro ao criar condição de pagamento", ex.Message);
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
            string codDescPagto = "";

            if (propertyName == "Codigo")
            {
                if(!string.IsNullOrWhiteSpace(value))
                    codDescPagto = _condicaoPagamentoService.CarregarCondicoesPagamento().Where(u => u.Codigo == value).FirstOrDefault().Codigo;

                if (string.IsNullOrWhiteSpace(value))
                {
                    TempData["error"] = "O campo " + propertyName + " é de preenchimento obrigatório";
                    status = true;
                }
                else if (!string.IsNullOrWhiteSpace(codDescPagto))
                {
                    TempData["error"] = "Não é possível atualizar o código da condição de pagamento, pois já existe condição de pagamento cadastrada no sistema com este código.";
                    status = true;
                }
                model.Codigo = value;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    TempData["error"] = "O campo " + propertyName + " é de preenchimento obrigatório";
                    status = true;
                }
                model.Descricao = value;
            }
               

            try {
                if (!status)
                {
                    _condicaoPagamentoService.AtualizarCondicaoPagamento(ConverterTiposObjetosCondicaoPagamentoViewModelParaCondicaoPagamento(model));
                    TempData["success"] = "Condição de Pagamento atualizada com sucesso";
                    status = true;
                }              
			} catch (Exception ex){
                message = ex.Message;
                TempData["error"] = ("Erro ao atualizar condição de pagamento", message);
            }

            var response = new { value = value, status = status, message = message };
            JObject o = JObject.FromObject(response);
            return Content(o.ToString());
        }

        public ActionResult Excluir(int condicaoPagamentoID, int value = 0) {
            try
            {
                var pedido = _pedidoService.CarregarPedidos(0).Where(p => p.CondicaoPagamentoID == condicaoPagamentoID).FirstOrDefault();

                if (pedido != null)
                {
                    TempData["error"] = "Não é possível excluir a condição de pagamento, pois existem pedidos gerados no sistema com esta condição de pagamento.";
                    return RedirectToAction("Index");
                }

                _condicaoPagamentoService.ExcluirCondicaoPagamento(condicaoPagamentoID);
                TempData["success"] = "Condição de pagamento excluída com sucesso";
            }
            catch (Exception ex)
            {
                TempData["error"] = ("Erro ao excluir condição de pagamento", ex);
                return RedirectToAction("Index");
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

        private string ValidaCadastro(CondicaoPagamento condicaoPagamento)
        {
            string message = "";
            var codigoCondPagto = _condicaoPagamentoService.CarregarCondicoesPagamento().Where(c => c.Codigo == condicaoPagamento.Codigo).FirstOrDefault();

            if (codigoCondPagto != null)
                return message = "Não é possível incluir a condição de pagamento, pois já existe condição de pagamento cadastrada no sistema com este código.";

            return message;
        }
        #endregion
    }
}