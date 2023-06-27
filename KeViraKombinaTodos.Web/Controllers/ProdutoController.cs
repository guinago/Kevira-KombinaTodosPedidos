using System;
using System.Collections.Generic;
using System.Linq;
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
        private IPedidoService _pedidoService;
        public ProdutoController(IProdutoService produtoService, IPedidoService pedidoService)
        {
            _produtoService = produtoService ?? throw new ArgumentNullException(nameof(produtoService));
            _pedidoService = pedidoService ?? throw new ArgumentNullException(nameof(pedidoService));
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
                string message = ValidaCadastro(produto);

                if (!string.IsNullOrWhiteSpace(message))
                {
                    TempData["error"] = message;
                    return View(model);
                }

                produtoID = _produtoService.CriarProduto(produto);
                TempData["success"] = "Produto salvo com sucesso";

            } catch (Exception ex){
                TempData["error"] = ("Erro ao criar produto", ex.Message);
                return View(model);
			}
			return RedirectToAction("Index");
		}
        [HttpPost]
        public ActionResult Edit(int produtoID, string propertyName, string value)
        {
            var status = false;
            var message = "";
            string codProduto = "";
            ProdutoModel model = new ProdutoModel();
            try
            {
                model.Ativo = _produtoService.CarregarProduto(produtoID).Ativo;
                model.ProdutoID = produtoID;

                if (propertyName == "Codigo")
                {
                    if (!string.IsNullOrWhiteSpace(value))
                        codProduto = _produtoService.CarregarProdutos().Where(u => u.Codigo == value).FirstOrDefault().Codigo;

                    if (string.IsNullOrWhiteSpace(value))
                    {
                        TempData["error"] = "O campo " + propertyName + " é de preenchimento obrigatório";
                        status = true;
                    }
                    else if (!string.IsNullOrWhiteSpace(codProduto))
                    {
                        TempData["error"] = "Não é possível atualizar o código do produto, pois já existe produto cadastrada no sistema com este código.";
                        status = true;
                    }
                    model.Codigo = value;
                }
                else if (propertyName == "Descricao")
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        TempData["error"] = "O campo " + propertyName + " é de preenchimento obrigatório";
                        status = true;
                    }
                    model.Descricao = value;
                }
                else if (propertyName == "Preco")
                {
                    model.Valor = Convert.ToDouble(value);
                }
                else
                    model.Quantidade = Convert.ToDouble(value);

                if (!status)
                {
                    _produtoService.AtualizarProduto(ConverterTiposObjetosProdutoViewModelParaProduto(model));
                    TempData["success"] = "Produto atualizado com sucesso";
                    status = true;
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                TempData["error"] = ("Erro ao atualizar produto", message);
            }

            var response = new { value = value, status = status, message = message };
            JObject o = JObject.FromObject(response);
            return Content(o.ToString());
        }
        public ActionResult EditCheckd(int produtoID, int value)
        {
            ProdutoModel model = new ProdutoModel();
            model.ProdutoID = produtoID;
            model.Ativo = Convert.ToBoolean(value);

            try
            {
                _produtoService.AtualizarProduto(ConverterTiposObjetosProdutoViewModelParaProduto(model));
                TempData["success"] = "Produto atualizado com sucesso";
            }
            catch (Exception ex)
            {
                TempData["error"] = ("Erro ao atualizar status produto", ex.Message);
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        public ActionResult Excluir(int produtoID, int value = 0)
        {
            try
            {
                var itemPedido = _pedidoService.CarregarItensPedido(0).Where(p => p.ProdutoID == produtoID).FirstOrDefault();

                if (itemPedido != null)
                {
                    TempData["error"] = "Não é possível excluir o produto, pois existem pedidos gerados no sistema com este produto.";
                    return RedirectToAction("Index");
                }

                _produtoService.ExcluirProduto(produtoID);
                TempData["success"] = "Produto excluído com sucesso";
            }
            catch (Exception ex)
            {
                TempData["error"] = ("Erro ao excluir produto", ex.Message);
                return RedirectToAction("Index");
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
        private string ValidaCadastro(Produto produto)
        {
            string message = "";
            var codigoProduto = _produtoService.CarregarProdutos().Where(p => p.Codigo == produto.Codigo).FirstOrDefault();

            if (codigoProduto != null)
                return message = "Não é possível incluir o produto, pois já existe produto cadastrado no sistema com este código.";

            return message;
        }
        #endregion
    }
}