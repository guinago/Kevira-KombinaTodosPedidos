using System;
using System.Collections.Generic;
using System.Web.Mvc;
using KeViraKombinaTodos.Core.Models;
using KeViraKombinaTodos.Core.Services;
using KeViraKombinaTodos.Web.Models;

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
		public ActionResult Details(int produtoID)
        {
			ProdutoModel model = CarregarProduto(produtoID);

			if (model == null)
				return RedirectToAction("Index");

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

            } catch {
				return View(model);
			}
			return RedirectToAction("Details", new { produtoID = produtoID });
		}
		public ActionResult Edit(int produtoID)
        {
			return View(CarregarProduto(produtoID));
		}

		[HttpPost]
		public ActionResult Edit(ProdutoModel model)
        {
			try {
                _produtoService.AtualizarProduto(ConverterTiposObjetosProdutoViewModelParaProduto(model));

                return RedirectToAction("Index");
			} catch
            {
				return View(model);
			}
		}
		public ActionResult Excluir(int produtoID)
        {
            _produtoService.ExcluirProduto(produtoID);

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