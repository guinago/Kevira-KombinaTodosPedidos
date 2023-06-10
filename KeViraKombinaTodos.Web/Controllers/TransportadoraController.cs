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
	public class TransportadoraController : Controller {

		#region Inject
		private ITransportadoraService _transportadoraService;
        private IPedidoService _pedidoService;

        public TransportadoraController(ITransportadoraService TransportadoraService, IPedidoService PedidoService) {
			_transportadoraService = TransportadoraService ?? throw new ArgumentNullException(nameof(TransportadoraService));
            _pedidoService = PedidoService ?? throw new ArgumentNullException(nameof(PedidoService));
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

		public ActionResult Create() {
			return View(new TransportadoraModel());
		}

		[HttpPost]
		public ActionResult Create(TransportadoraModel model) {
			int transportadoraID = 0;

            try {
                Transportadora transportadora = ConverterTiposObjetosTransportadoraViewModelParaTransportadora(model);
                string message = ValidaCadastro(transportadora);

                if (!string.IsNullOrWhiteSpace(message))
                {
                    TempData["error"] = message;
                    return View(model);
                }

                transportadoraID = _transportadoraService.CriarTransportadora(transportadora);
                TempData["success"] = "Transportadora salva com sucesso!";
            } catch (Exception ex){
                TempData["error"] = (ex.Message, "Erro ao criar transportadora");
                return View(model);
			}
			return RedirectToAction("Index");
		}

        [HttpPost]
        public ActionResult Edit(int transportadoraID, string propertyName, string value)
        {
            var status = false;
            var message = "";
            TransportadoraModel model = new TransportadoraModel();
            model.TransportadoraID = transportadoraID;

            if (propertyName == "Codigo")
            {
                model.Codigo = value;
            }
            else
                model.Descricao = value;

            try
            {
                _transportadoraService.AtualizarTransportadora(ConverterTiposObjetosTransportadoraViewModelParaTransportadora(model));
                TempData["success"] = "Transportadora atualizada com sucesso!";
                status = true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                TempData["error"] = (message, "Erro ao atualizar transportadora");
            }

            var response = new { value = value, status = status, message = message };
            JObject o = JObject.FromObject(response);
            return Content(o.ToString());
        }

        public ActionResult Excluir(int transportadoraID, int value = 0)
        {
            try
            {
                var pedido = _pedidoService.CarregarPedidos(0).Where(p => p.TransportadoraID == transportadoraID).FirstOrDefault();

                if (pedido != null)
                {
                    TempData["error"] = "Não é possível excluir a transportadora, pois existem pedidos gerados no sistema com esta transportadora.";
                    return RedirectToAction("Index");
                }

                _transportadoraService.ExcluirTransportadora(transportadoraID);
                TempData["success"] = "Transportadora excluída com sucesso!";
            }
            catch (Exception ex)
            {
                TempData["error"] = ("Erro ao excluir transportadora", ex);
                return RedirectToAction("Index");
            }
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

        private string ValidaCadastro(Transportadora transportadora)
        {
            string message = "";
            var codigoTransportadora = _transportadoraService.CarregarTransportadoras().Where(t => t.Codigo == transportadora.Codigo).FirstOrDefault();

            if (codigoTransportadora != null)
                return message = "Não é possível incluir a transportadora, pois já existe transportadora cadastrada no sistema com este código.";

            return message;
        }
        #endregion
    }
}