using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using KeViraKombinaTodos.Core.Models;
using KeViraKombinaTodos.Core.Services;
using KeViraKombinaTodos.Web.Extensions;
using KeViraKombinaTodos.Web.Models;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KeViraKombinaTodos.Web.Controllers
{
    [AuthorizeAttribute]
    public class PedidoController : Controller
    {

        #region Inject
        private IPedidoService _pedidoService;
        private IProdutoService _produtoService;
        private ITransportadoraService _transportadoraService;
        private IRedisService _cacheService;
        private IClienteService _clienteService;

        public PedidoController(IPedidoService pedidoService, IProdutoService produtoService, ITransportadoraService transportadoraService, IRedisService cacheService, IClienteService clienteService)
        {
            _pedidoService = pedidoService ?? throw new ArgumentNullException(nameof(pedidoService));
            _produtoService = produtoService ?? throw new ArgumentException(nameof(produtoService));
            _transportadoraService = transportadoraService ?? throw new ArgumentException(nameof(transportadoraService));
            _cacheService = cacheService ?? throw new ArgumentException(nameof(cacheService));
            _clienteService = clienteService ?? throw new ArgumentException(nameof(clienteService));
        }
        #endregion

        #region Actions
        public ActionResult Index()
        {
            var model = new PedidoModel();
            model.Itens = CarregarTodosProdutos();

            return View(model);
        }

        public ActionResult VisualizarPedidos()
        {
            return View(CarregarPedidos());
        }

        public ActionResult InserirCarrinho(int produtoID)
        {
            try
            {
                List<PedidoItemModel> itens = new List<PedidoItemModel>();
                var key = "GerandoPedido_" + User.Identity.GetIdUsuarioLogado();

                if (_cacheService.IsKeyExists(key))
                {
                    var result = _cacheService.GetStrings(key);
                    if (!string.IsNullOrEmpty(result))
                    {
                        var itensCache = JsonConvert.DeserializeObject<List<PedidoItemModel>>(result);

                        itensCache.ForEach(d => itens.Add(d));
                    }
                }

                if (!itens.Exists(d => d.ProdutoID.GetValueOrDefault() == produtoID))
                {
                    var produtoItem = TransformarProdutoEmItemPedido(_produtoService.CarregarProduto(produtoID));

                    itens.Add(produtoItem);

                    var jsonModel = JsonConvert.SerializeObject(itens);

                    _cacheService.SetStrings(key, jsonModel);
                }
            }
            catch (Exception)
            {

                //naudinha
            }
            return RedirectToAction("Index");
        }
        public ActionResult RemoverCarrinho(int produtoID)
        {
            try
            {
                List<PedidoItemModel> itens = new List<PedidoItemModel>();
                var key = "GerandoPedido_" + User.Identity.GetIdUsuarioLogado();

                if (_cacheService.IsKeyExists(key))
                {
                    var result = _cacheService.GetStrings(key);
                    if (!string.IsNullOrEmpty(result))
                    {
                        var itensCache = JsonConvert.DeserializeObject<List<PedidoItemModel>>(result);

                        itensCache.ForEach(d => itens.Add(d));
                    }
                }

                if (itens.Exists(d => d.ProdutoID.GetValueOrDefault() == produtoID))
                {
                    itens.Remove(itens.FirstOrDefault(d => d.ProdutoID.GetValueOrDefault() == produtoID));

                    var jsonModel = JsonConvert.SerializeObject(itens);

                    _cacheService.SetStrings(key, jsonModel);
                }
            }
            catch
            {
                //nauda                
            }

            return RedirectToAction("Index");
        }
        public ActionResult CreateDadosCliente()
        {
            DadosClienteModel model = new DadosClienteModel();

            var result = CarregarModelCache("CreateDadosCliente_" + User.Identity.GetIdUsuarioLogado(), model);

            model = result as DadosClienteModel;           

            return View(model);
        }

        [HttpPost]
        public ActionResult CreateDadosCliente(DadosClienteModel model)
        {
            try
            {
                var key = "CreateDadosCliente_" + User.Identity.GetIdUsuarioLogado();

                var jsonModel = JsonConvert.SerializeObject(model);

                _cacheService.SetStrings(key, jsonModel);
            }
            catch
            {
                return View(model);
            }
            return RedirectToAction("CreateDadosEntrega");
        }

        public ActionResult CreateDadosEntrega()
        {
            DadosEntregaModel model = new DadosEntregaModel();

            var result =  CarregarModelCache("CreateDadosEntrega_" + User.Identity.GetIdUsuarioLogado(), model);

            if(result != null)
                model = result as DadosEntregaModel;    

            model.ListTransportadora = GetModelTransportadoraGeral();

            return View(model);
        }

        [HttpPost]
        public ActionResult CreateDadosEntrega(DadosEntregaModel model)
        {

            try
            {
                var key = "CreateDadosEntrega_" + User.Identity.GetIdUsuarioLogado();

                var jsonModel = JsonConvert.SerializeObject(model);

                _cacheService.SetStrings(key, jsonModel);
            }
            catch
            {
                return View(model);
            }

            return RedirectToAction("CreateCondPagtoPedido");
        }

        public ActionResult CreateCondPagtoPedido()
        {
            return View(new CondicaoPagamentoModel());
        }

        [HttpPost]
        public ActionResult CreateCondPagtoPedido(CondicaoPagamentoModel model)
        {
            PedidoModel modelPedido = CarregarDadosGeracaoPedido();
            var IDPedido = 0;
            modelPedido.CondPagtoPedido = model;

            try
            {
                IDPedido = _pedidoService.CriarPedido(ConverterTiposObjetosPedidoViewModelParaPedido(modelPedido));
                modelPedido.PedidoID = IDPedido;
                modelPedido.Itens.ForEach(d => d.PedidoID = IDPedido);

                foreach (var item in modelPedido.Itens)
                {
                    _pedidoService.CriarItemPedido(ConverterTiposObjetosPedidoItemViewModelParaPedidoItem(item));
                }
                LimparCacheUsuarioAposPedido();
            }
            catch (Exception ex)
            {
                // cuspir erro
                return RedirectToAction("CreateCondPagtoPedido");
            }

            return RedirectToAction("Details", new { pedidoID = IDPedido });
        }

        // Este método é para a listagem de Pedidos
        //[HttpPost]
        //public ActionResult ListarPedidos()
        //{
        //    IList<PedidoModel> model = new List<PedidoModel>();

        //    IList<Pedido> pedidos = _pedidoService.CarregarPedidos();

        //    foreach (var item in pedidos)
        //    {
        //        PedidoModel modelPedido = new PedidoModel();

        //        modelPedido = ConverterTiposObjetosPedidoParaPedidoViewModel(item);
        //        model.Add(modelPedido);
        //    }
        //    return View(model);
        //}

        public ActionResult Details(int pedidoID)
        {
            PedidoModel model = CarregarPedido(pedidoID);

            if (model == null)
                return RedirectToAction("Index");

            return View(model);
        }

        public ActionResult Edit(int pedidoID)
        {
            return View(CarregarPedido(pedidoID));
        }

        [HttpPost]
        public ActionResult Edit(PedidoModel model)
        {
            try
            {
                _pedidoService.AtualizarPedido(ConverterTiposObjetosPedidoViewModelParaPedido(model));

                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }

        public ActionResult Excluir(int pedidoID)
        {

            return RedirectToAction("Index");
        }

        #endregion

        #region Methods

        private void LimparCacheUsuarioAposPedido()
        {
            try
            {
                 _cacheService.SetStrings("CreateDadosCliente_" + User.Identity.GetIdUsuarioLogado(), string.Empty);
                _cacheService.SetStrings("CreateDadosEntrega_" + User.Identity.GetIdUsuarioLogado(), string.Empty);
                _cacheService.SetStrings("GerandoPedido_" + User.Identity.GetIdUsuarioLogado(), string.Empty);   
            }
            catch (Exception)
            {

                throw;
            }
        }

        private object CarregarModelCache(string key, object model)
        {
            try
            {
                if (_cacheService.IsKeyExists(key))
                {
                    var result = _cacheService.GetStrings(key);

                    if (!string.IsNullOrEmpty(result))
                        return RetornarTipoModel(model, result);
                }
                else
                {
                    model = null;
                }
            }
            catch
            {
                model = null;
            }

            return model;
        }

        private Object RetornarTipoModel(object model, string result)
        {
            if (model is DadosEntregaModel)
                return JsonConvert.DeserializeObject<DadosEntregaModel>(result);

            if(model is DadosClienteModel)
                return JsonConvert.DeserializeObject<DadosClienteModel>(result);

            return null;
        }
        private PedidoModel CarregarDadosGeracaoPedido()
        {
            var modelPedido = new PedidoModel();

            try
            {
                var modelDadosCliente = new DadosClienteModel();
                var result = CarregarModelCache("CreateDadosCliente_" + User.Identity.GetIdUsuarioLogado(), modelDadosCliente);
                modelDadosCliente = result as DadosClienteModel;

                modelPedido.DadosCliente = modelDadosCliente;

                var modelDadosEntrega = new DadosEntregaModel();
                var resultEntrega = CarregarModelCache("CreateDadosEntrega_" + User.Identity.GetIdUsuarioLogado(), modelDadosEntrega);
                modelDadosEntrega = resultEntrega as DadosEntregaModel;

                modelPedido.DadosEntrega = modelDadosEntrega;

                var itensCache = CarregarItensDoCache();

                itensCache.ForEach(d=> modelPedido.Itens.Add(d));

            }
            catch (Exception)
            {

                throw;
            }

            return modelPedido;
        }

        private PedidoItemModel TransformarProdutoEmItemPedido(Produto produto)
        {
            var item = new PedidoItemModel();
            item.ProdutoID = produto.ProdutoID;
            item.Preco = produto.Valor;
            item.Quantidade = produto.Quantidade;
            item.Descricao = produto.Descricao;

            return item;
        }
        private IList<PedidoItemModel> CarregarTodosProdutos()
        {
            IList<PedidoItemModel> itens = new List<PedidoItemModel>();
            IList<Produto> produtos = _produtoService.CarregarProdutos();
            var itensCache = CarregarItensDoCache();

            foreach (var produto in produtos)
            {
                var item = TransformarProdutoEmItemPedido(produto);
                item.NoCarrinho = (itensCache.Any() && itensCache.Exists(d => d.ProdutoID.GetValueOrDefault() == produto.ProdutoID));

                itens.Add(item);
            }

            return itens;
        }
        private List<PedidoItemModel> CarregarItensDoCache()
        {
            List<PedidoItemModel> itens = new List<PedidoItemModel>();

            try
            {
                var key = "GerandoPedido_" + User.Identity.GetIdUsuarioLogado();

                if (_cacheService.IsKeyExists(key))
                {
                    var result = _cacheService.GetStrings(key);
                    if (!string.IsNullOrEmpty(result))
                    {
                        var itensCache = JsonConvert.DeserializeObject<List<PedidoItemModel>>(result);

                        itensCache.ForEach(d => itens.Add(d));
                    }
                }
            }
            catch (Exception ex)
            {
                // fica mudo por enquanto
            }

            return itens;
        }

        private PedidoModel ConverterTiposObjetosPedidoParaPedidoViewModel(Pedido Pedido)
        {
            PedidoModel model = new PedidoModel();

            PropertyCopier<Pedido, PedidoModel>.Copy(Pedido, model);

            return model;
        }

        private ItemPedido ConverterTiposObjetosPedidoItemViewModelParaPedidoItem(PedidoItemModel item)
        {
            ItemPedido itemPedido = new ItemPedido();

            PropertyCopier<PedidoItemModel, ItemPedido>.Copy(item, itemPedido);

            return itemPedido;
        }

        private Pedido ConverterTiposObjetosPedidoViewModelParaPedido(PedidoModel model)
        {
            model.VendedorID = User.Identity.GetIdUsuarioLogado();

            Cliente Cliente = new Cliente();
            Cliente.CPF = model.DadosCliente.CPF;
            Cliente.Nome = model.DadosCliente.Nome;
            Cliente.Telefone = model.DadosCliente.Telefone;
            Cliente.Email = model.DadosCliente.Email;
            Cliente.CEP = model.DadosEntrega.CEP;
            Cliente.Estado = model.DadosEntrega.Estado;
            Cliente.Municipio = model.DadosEntrega.Municipio;
            Cliente.Bairro = model.DadosEntrega.Bairro;
            Cliente.Endereco = model.DadosEntrega.Logradouro + model.DadosEntrega.Numero;
            Cliente.Complemento = model.DadosEntrega.Complemento;

            var ClienteID = _clienteService.CriarCliente(Cliente);

            Pedido Pedido = new Pedido();
            Pedido.ClienteID = ClienteID;
            Pedido.VendedorID = model.VendedorID;
            Pedido.TransportadoraID = model.DadosEntrega.TransportadoraIDSelected;
            Pedido.CondicaoPagamentoID = model.CondPagtoPedido.CondicaoPagamentoID;
            Pedido.Telefone = model.DadosCliente.Telefone;
            Pedido.Email = model.DadosCliente.Email;
            Pedido.CEP = model.DadosEntrega.CEP;
            Pedido.Estado = model.DadosEntrega.Estado;
            Pedido.Municipio = model.DadosEntrega.Municipio;
            Pedido.Bairro = model.DadosEntrega.Bairro;
            Pedido.Endereco = model.DadosEntrega.Logradouro + model.DadosEntrega.Numero;
            Pedido.DataEntrega = model.DadosEntrega.DataEntrega;
            Pedido.Observacao = model.DadosEntrega.Observacao;
            Pedido.Restricao = model.DadosEntrega.Restricao;
            Pedido.NotaFiscal = model.NotaFiscal;
            Pedido.Status = model.Status;
            Pedido.ValorTotal = model.Itens.Sum(i => i.Preco * i.Quantidade).GetValueOrDefault();


            //PropertyCopier<PedidoModel, Pedido>.Copy(model, Pedido);

            return Pedido;
        }

        private IList<PedidoModel> CarregarPedidos()
        {
            IList<PedidoModel> model = new List<PedidoModel>();

            IList<Pedido> pedidos = _pedidoService.CarregarPedidos(User.Identity.GetIdUsuarioLogado());

            foreach (var item in pedidos)
            {
                PedidoModel modelPedido = new PedidoModel();

                modelPedido = ConverterTiposObjetosPedidoParaPedidoViewModel(item);
                model.Add(modelPedido);
            }
            return model;
        }

        private PedidoModel CarregarPedido(int pedidoID)
        {
            PedidoModel model = new PedidoModel();
            Pedido pedido = _pedidoService.CarregarPedido(pedidoID);
            IList<ItemPedido> itensPedido = _pedidoService.CarregarItensPedido(pedidoID);

            if (pedido == null)
                return null;

            model = ConverterTiposObjetosPedidoParaPedidoViewModel(pedido);
            foreach (var item in itensPedido)
            {
                model.Itens.Add(ConverterTiposObjetosPedidoItemParaPedidoItemViewModel(item));
            }

            return model;

        }

        private PedidoItemModel ConverterTiposObjetosPedidoItemParaPedidoItemViewModel(ItemPedido item)
        {
            PedidoItemModel itemPedido = new PedidoItemModel();

            PropertyCopier<ItemPedido, PedidoItemModel>.Copy(item, itemPedido);

            return itemPedido;
        }

        public IDictionary<int, string> GetModelTransportadoraGeral()
        {
            var result = new Dictionary<int, string>();
            var transportadora = (List<Transportadora>)_transportadoraService.CarregarTransportadoras();

            transportadora.ForEach(d => result.Add(d.TransportadoraID, d.Descricao));

            return result;
        }
        #endregion
    }
}