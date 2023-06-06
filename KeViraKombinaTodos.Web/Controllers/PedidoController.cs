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
        private ICondicaoPagamentoService _condPgtoService;

        public PedidoController(IPedidoService pedidoService, IProdutoService produtoService, ITransportadoraService transportadoraService, IRedisService cacheService, IClienteService clienteService, ICondicaoPagamentoService condPgtoService)
        {
            _pedidoService = pedidoService ?? throw new ArgumentNullException(nameof(pedidoService));
            _produtoService = produtoService ?? throw new ArgumentException(nameof(produtoService));
            _transportadoraService = transportadoraService ?? throw new ArgumentException(nameof(transportadoraService));
            _cacheService = cacheService ?? throw new ArgumentException(nameof(cacheService));
            _clienteService = clienteService ?? throw new ArgumentException(nameof(clienteService));
            _condPgtoService = condPgtoService ?? throw new ArgumentException(nameof(condPgtoService));
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

        public ActionResult VisualizarEntregas()
        {
            return View(CarregarPedidos());
        }

        public ActionResult CreateCarrinhoItens()
        {
            IList<PedidoItemModel> model = new List<PedidoItemModel>();
            var itensCache = CarregarItensDoCache();

            itensCache.ForEach(d => model.Add(d));

            return View(model);
        }

        [HttpPost]
        public ActionResult CreateCarrinhoItens(int produtoID, string propertyName, float value)
        {
            var status = false;
            var message = "";
            var key = "GerandoPedido_" + User.Identity.GetIdUsuarioLogado();

            try
            {
                var itensCache = CarregarItensDoCache();
                foreach (var item in itensCache.Where(i => i.ProdutoID == produtoID))
                {
                    if (propertyName == "Quantidade")
                    {
                        if (value == 0)
                        {
                            try
                            {
                                RemoverCarrinho(produtoID);
                                status = true;
                            }
                            catch (Exception ex)
                            {
                                message = ex.Message;
                                ModelState.AddModelError(message, "Erro ao atualizar o Item do Pedido");
                            }
                        }
                        else
                            item.Quantidade = value;
                    }
                    else
                        item.Preco = value;
                }
                if (!status)
                {
                    var jsonModel = JsonConvert.SerializeObject(itensCache);

                    _cacheService.SetStrings(key, jsonModel);
                    status = true;
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            var response = new { value = value, status = status, message = message };
            JObject o = JObject.FromObject(response);
            return Content(o.ToString());
        }


        public ActionResult CreateDadosCliente()
        {
            DadosClienteModel model = new DadosClienteModel();

            var result = CarregarModelCache("CreateDadosCliente_" + User.Identity.GetIdUsuarioLogado(), model);

            if(result != null)
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
            CondicaoPagamentoModel model = new CondicaoPagamentoModel();

            model.ListCondPagamento = GetModelCondPagto();

            return View(model);
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

        public ActionResult Details(int pedidoID)
        {
            PedidoModel model = CarregarPedido(pedidoID);

            if (model == null)
                return RedirectToAction("Index");

            model.StatusPedido = model.ListStatus.FirstOrDefault(l => l.Key == model.Status).Value;

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

                return RedirectToAction("Details", new { pedidoID = model.PedidoID });
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

        public ActionResult InserirCarrinho(int id)
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

                if (!itens.Exists(d => d.ProdutoID.GetValueOrDefault() == id))
                {
                    var produtoItem = TransformarProdutoEmItemPedido(_produtoService.CarregarProduto(id));

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
        public ActionResult RemoverCarrinho(int id)
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

                if (itens.Exists(d => d.ProdutoID.GetValueOrDefault() == id))
                {
                    itens.Remove(itens.FirstOrDefault(d => d.ProdutoID.GetValueOrDefault() == id));

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

        #endregion

        #region Methods

        [HttpPost]
        public ActionResult SaveItem(int pedidoID, int produtoID, string propertyName, float value)
        {
            var status = false;
            var message = "";
            PedidoItemModel model = new PedidoItemModel();
            model.PedidoID = pedidoID;
            model.ProdutoID = produtoID;
            if (propertyName == "Quantidade")
            {
                if (value == 0)
                {
                    try
                    {
                        _pedidoService.ExcluirItemPedido(pedidoID, produtoID);
                        status = true;
                    }
                    catch (Exception ex)
                    {
                        message = ex.Message;
                        ModelState.AddModelError(message, "Erro ao atualizar o Item do Pedido");
                    }
                }
                else
                    model.Quantidade = value;
            }
            else
                model.Preco = value;

            if (!status)
            {
                try
                {
                    _pedidoService.AtualizarItemPedido(ConverterTiposObjetosPedidoItemViewModelParaPedidoItem(model));
                    status = true;
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    ModelState.AddModelError(message, "Erro ao atualizar o Item do Pedido");
                }
            }

            var response = new { value = value, status = status, message = message };
            JObject o = JObject.FromObject(response);
            return Content(o.ToString());
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

        private PedidoModel CarregarDadosGeracaoPedido()
        {
            var modelPedido = new PedidoModel();

            try
            {
                var modelDadosCliente = new DadosClienteModel();
                var result = CarregarModelCache("CreateDadosCliente_" + User.Identity.GetIdUsuarioLogado(), modelDadosCliente);
                modelDadosCliente = result as DadosClienteModel;

                if (modelDadosCliente != null)
                    modelPedido.DadosCliente = modelDadosCliente;

                var modelDadosEntrega = new DadosEntregaModel();
                var resultEntrega = CarregarModelCache("CreateDadosEntrega_" + User.Identity.GetIdUsuarioLogado(), modelDadosEntrega);
                modelDadosEntrega = resultEntrega as DadosEntregaModel;

                if (modelDadosEntrega != null)
                    modelPedido.DadosEntrega = modelDadosEntrega;

                var itensCache = CarregarItensDoCache();

                itensCache.ForEach(d => modelPedido.Itens.Add(d));

            }
            catch (Exception ex)
            {
                throw;
            }
            return modelPedido;
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

        private IList<PedidoItemModel> CarregarTodosProdutos()
        {
            IList<PedidoItemModel> itens = new List<PedidoItemModel>();
            IList<Produto> produtos = _produtoService.CarregarProdutos();
            var itensCache = CarregarItensDoCache();

            foreach (var produto in produtos.Where(p => p.Ativo))
            {
                var item = TransformarProdutoEmItemPedido(produto);
                item.NoCarrinho = (itensCache.Any() && itensCache.Exists(d => d.ProdutoID.GetValueOrDefault() == produto.ProdutoID));

                itens.Add(item);
            }
            return itens;
        }

        private PedidoModel ConverterTiposObjetosPedidoParaPedidoViewModel(Pedido Pedido)
        {
            PedidoModel model = new PedidoModel();

            PropertyCopier<Pedido, PedidoModel>.Copy(Pedido, model);

            model.DadosEntrega = new DadosEntregaModel();
            model.DadosEntrega.TransportadoraIDSelected = model.TransportadoraID;
            GetModelTransportadoraGeral().ForEach(d => model.DadosEntrega.ListTransportadora.Add(d.Key, d.Value));

            model.CondPagtoPedido = new CondicaoPagamentoModel();
            model.CondPagtoPedido.CondicaoPagamentoID = model.CondicaoPagamentoID;
            GetModelCondPagto().ForEach(d => model.CondPagtoPedido.ListCondPagamento.Add(d.Key, d.Value));

            model.StatusPedido = model.StatusPedido = model.ListStatus.FirstOrDefault(l => l.Key == model.Status).Value;

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
            int ClienteID = model.ClienteID;

            Cliente Cliente = new Cliente();
            Cliente.ClienteID = model.ClienteID;
            Cliente.CPF = model.DadosCliente != null ? model.DadosCliente.CPF : model.CPF;
            Cliente.Nome = model.DadosCliente != null ? model.DadosCliente.Nome : model.Cliente;
            Cliente.Telefone = model.DadosCliente != null ? model.DadosCliente.Telefone : model.Telefone;
            Cliente.Email = model.DadosCliente != null ? model.DadosCliente.Email : model.Email;
            Cliente.CEP = model.DadosEntrega.CEP != null ? model.DadosEntrega.CEP : model.CEP;
            Cliente.Estado = model.DadosEntrega.Estado != null ? model.DadosEntrega.Estado : model.Estado;
            Cliente.Municipio = model.DadosEntrega.Municipio != null ? model.DadosEntrega.Municipio : model.Municipio;
            Cliente.Bairro = model.DadosEntrega.Bairro != null ? model.DadosEntrega.Bairro : model.Bairro;
            Cliente.Endereco = model.DadosEntrega.Complemento != null ? model.DadosEntrega.Logradouro + ", " + model.DadosEntrega.Numero : model.Endereco;
            Cliente.Complemento = model.DadosEntrega != null ? model.DadosEntrega.Complemento : model.Complemento;

            if (model.DadosCliente != null)
            {
                ClienteID = _clienteService.CriarCliente(Cliente);
            }
            else
                _clienteService.AtualizarCliente(Cliente);

            Pedido Pedido = new Pedido();
            Pedido.PedidoID = model.PedidoID;
            Pedido.ClienteID = ClienteID != 0 ? ClienteID : model.ClienteID;
            Pedido.VendedorID = model.VendedorID;
            Pedido.TransportadoraID = model.DadosEntrega.TransportadoraIDSelected;
            Pedido.CondicaoPagamentoID = model.CondPagtoPedido != null ? model.CondPagtoPedido.CondicaoPagamentoID : model.CondicaoPagamentoID;
            Pedido.Telefone = model.DadosCliente != null ? model.DadosCliente.Telefone : model.Telefone;
            Pedido.Email = model.DadosCliente != null ? model.DadosCliente.Email : model.Email;
            Pedido.CEP = model.DadosEntrega.CEP != null ? model.DadosEntrega.CEP : model.CEP;
            Pedido.Estado = model.DadosEntrega.Estado != null ? model.DadosEntrega.Estado : model.Estado;
            Pedido.Municipio = model.DadosEntrega.Municipio != null ? model.DadosEntrega.Municipio : model.Municipio;
            Pedido.Bairro = model.DadosEntrega.Bairro != null ? model.DadosEntrega.Bairro : model.Bairro;
            Pedido.Endereco = model.DadosEntrega.Logradouro != null ? model.DadosEntrega.Logradouro + ", " + model.DadosEntrega.Numero : model.Endereco;
            Pedido.DataEntrega = model.DadosEntrega.DataEntrega != null ? model.DadosEntrega.DataEntrega : model.DataEntrega;
            Pedido.Observacao = model.DadosEntrega.Observacao != null ? model.DadosEntrega.Observacao : model.Observacao;
            Pedido.Restricao = model.DadosEntrega.Restricao != null ? model.DadosEntrega.Restricao : model.Restricao;
            Pedido.NotaFiscal = model.NotaFiscal;
            Pedido.Status = model.Status;
            Pedido.ValorTotal = model.Itens.Sum(i => i.Preco * i.Quantidade).GetValueOrDefault();


            //PropertyCopier<PedidoModel, Pedido>.Copy(model, Pedido);

            return Pedido;
        }

        private PedidoItemModel ConverterTiposObjetosPedidoItemParaPedidoItemViewModel(ItemPedido item)
        {
            PedidoItemModel itemPedido = new PedidoItemModel();

            PropertyCopier<ItemPedido, PedidoItemModel>.Copy(item, itemPedido);

            return itemPedido;
        }

        private PedidoItemModel TransformarProdutoEmItemPedido(Produto produto)
        {
            var item = new PedidoItemModel();
            item.ProdutoID = produto.ProdutoID;
            item.Preco = produto.Valor;
            item.Quantidade = produto.Quantidade;
            item.Descricao = produto.Descricao;
            item.Codigo = produto.Codigo;

            return item;
        }

        public IDictionary<int, string> GetModelTransportadoraGeral()
        {
            var result = new Dictionary<int, string>();
            var transportadora = (List<Transportadora>)_transportadoraService.CarregarTransportadoras();

            transportadora.ForEach(d => result.Add(d.TransportadoraID, d.Descricao));

            return result;
        }

        public IDictionary<int, string> GetModelCondPagto()
        {
            var result = new Dictionary<int, string>();
            var condPagamentos = (List<CondicaoPagamento>)_condPgtoService.CarregarCondicoesPagamento();

            condPagamentos.ForEach(d => result.Add(d.CondicaoPagamentoID, d.Descricao));

            return result;
        }

        private Object RetornarTipoModel(object model, string result)
        {
            if (model is DadosEntregaModel)
                return JsonConvert.DeserializeObject<DadosEntregaModel>(result);

            if (model is DadosClienteModel)
                return JsonConvert.DeserializeObject<DadosClienteModel>(result);

            return null;
        }

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
        #endregion
    }
}