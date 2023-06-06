using System;
using System.Collections.Generic;
using System.Web.Mvc;
using KeViraKombinaTodos.Core.Models;
using KeViraKombinaTodos.Core.Services;
using KeViraKombinaTodos.Web.Models;
using Newtonsoft.Json.Linq;

namespace KeViraKombinaTodos.Web.Controllers
{
    [AuthorizeAttribute]
	public class ProdutoController : Controller {

		#region Inject
		private IProdutoService _produtoService;

        public ProdutoController(IProdutoService produtoService)
        {
            _produtoService = produtoService ?? throw new ArgumentNullException(nameof(produtoService));
        }
        #endregion

        #region Actions
        public ActionResult Index()
        {
			IList<ProdutoModel> model = new List<ProdutoModel>();			
			IList<Produto> produto = _produtoService.CarregarProdutos();

			foreach (var item in produto) {
				ProdutoModel modelProduto = new ProdutoModel();

                modelProduto = ConverterTiposObjetosProdutoParaProdutoViewModel(item);                
				model.Add(modelProduto);
			}

			return View(model);
		}

		public ActionResult Create()
        {
			ProdutoModel model = new ProdutoModel();

			return View(model);
		}

		[HttpPost]
		public ActionResult Create(ProdutoModel model)
        {
			int produtoID = 0;
			try {
                Produto produto = ConverterTiposObjetosProdutoViewModelParaProduto(model);

                produtoID = _produtoService.CriarProduto(produto);

            } catch (Exception ex){
                ModelState.AddModelError(ex.Message, "Erro ao criar produto");
                return View(model);
			}
			return RedirectToAction("Index");
		}

        [HttpPost]
        public ActionResult Edit(int produtoID, string propertyName, string value)
        {
            var status = false;
            var message = "";
            ProdutoModel model = new ProdutoModel();
            model.Ativo = _produtoService.CarregarProduto(produtoID).Ativo;
            model.ProdutoID = produtoID;


            if (propertyName == "Codigo")
            {
                model.Codigo = value;
            }
            else if (propertyName == "Descricao")
            {
                model.Descricao = value;
            }               
            else if (propertyName == "Preco")
            {
                model.Valor = Convert.ToDouble(value);
            }              
            else 
                model.Quantidade = Convert.ToDouble(value);

            try
            {
                _produtoService.AtualizarProduto(ConverterTiposObjetosProdutoViewModelParaProduto(model));
                status = true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                ModelState.AddModelError(message, "Erro ao atualizar produto");
            }

            var response = new { value = value, status = status, message = message };
            JObject o = JObject.FromObject(response);
            return Content(o.ToString());
        }

        public ActionResult EditCheckd(int produtoID, int value)
        {
            var message = "";
            ProdutoModel model = new ProdutoModel();
            model.ProdutoID = produtoID;
            model.Ativo = Convert.ToBoolean(value);

            try
            {
                _produtoService.AtualizarProduto(ConverterTiposObjetosProdutoViewModelParaProduto(model));
            }
            catch (Exception ex)
            {
                message = ex.Message;
                ModelState.AddModelError(message, "Erro ao atualizar status produto");
            }
            return RedirectToAction("Index");
        }

        public ActionResult Excluir(int produtoID, int value = 0)
        {
            var message = "";

            try
            {
                _produtoService.ExcluirProduto(produtoID);
            }
            catch (Exception ex)
            {
                message = ex.Message;
                ModelState.AddModelError(message, "Erro ao excluir produto");
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region Methods

        private ProdutoModel ConverterTiposObjetosProdutoParaProdutoViewModel(Produto produto)
        {
			ProdutoModel model = new ProdutoModel();
			PropertyCopier<Produto, ProdutoModel>.Copy(produto, model);

			return model;
		}
		private Produto ConverterTiposObjetosProdutoViewModelParaProduto(ProdutoModel model)
        {
            Produto Produto = new Produto();
			PropertyCopier<ProdutoModel, Produto>.Copy(model, Produto);

			return Produto;
		}

		private ProdutoModel CarregarProduto(int produtoID)
        {
			ProdutoModel model = new ProdutoModel();
			Produto Produto = _produtoService.CarregarProduto(produtoID);

			if (Produto == null)
				return null;

			model = ConverterTiposObjetosProdutoParaProdutoViewModel(Produto);
			
			return model;
		}


		#endregion
	}
}