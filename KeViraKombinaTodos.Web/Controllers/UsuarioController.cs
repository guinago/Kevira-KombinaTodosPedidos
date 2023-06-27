using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using KeViraKombinaTodos.Core.Models;
using KeViraKombinaTodos.Core.Services;
using KeViraKombinaTodos.Web.Extensions;
using KeViraKombinaTodos.Web.Models;
using Newtonsoft.Json.Linq;

namespace KeViraKombinaTodos.Web.Controllers {
	[AuthorizeAttribute]
	public class UsuarioController : Controller {

		#region Inject
		private IUsuarioService _usuariosService;
		private IPerfilService _perfilService;
        private IPedidoService _pedidoService;
        public UsuarioController(IUsuarioService usuariosService, IPerfilService perfilService, IPedidoService pedidoService)
        {
			_perfilService = perfilService ?? throw new ArgumentException(nameof(perfilService));
			_usuariosService = usuariosService ?? throw new ArgumentException(nameof(usuariosService));
            _pedidoService = pedidoService ?? throw new ArgumentNullException(nameof(pedidoService));
        }
        #endregion

        #region Actions

        public ActionResult Index()
        {
            IList<UsuarioModel> model = new List<UsuarioModel>();
            IList<AspNetUsers> users = _usuariosService.CarregarUsuarios();

            foreach (var item in users)
            {
                UsuarioModel modelUsuario = new UsuarioModel();

                modelUsuario = ConverterTiposObjetosUsuarioParaUsuarioViewModel(item);
                model.Add(modelUsuario);
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, string propertyName, string value)
        {
            var status = false;
            var message = "";
            UsuarioModel model = new UsuarioModel();
            model.UsuarioID = id;
            model.Status = _usuariosService.CarregarUsuario(id).IsEnabled;
            string cgcUsuario = "";

            if (propertyName == "Nome")
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    TempData["error"] = "O campo " + propertyName + " é de preenchimento obrigatório";
                    status = true;
                }
                    
                model.Nome = value;
            }
            else if (propertyName == "CPF")
            {
                if (!string.IsNullOrWhiteSpace(value))
                    cgcUsuario = _usuariosService.CarregarUsuarios().Where(u => u.CGC == value).FirstOrDefault().CGC;

                if (string.IsNullOrWhiteSpace(value))
                {
                    TempData["error"] = "O campo " + propertyName + " é de preenchimento obrigatório";
                    status = true;
                }else if (!string.IsNullOrWhiteSpace(cgcUsuario))
                {
                    TempData["error"] = "Não é permitido mais de um CPF por usuário no sistema";
                    status = true;
                }
                    
                model.CPF = value;
            }
            else if (propertyName == "Telefone")
            {
                model.Telefone =value;
            }
            else if (propertyName == "Email")
            {
                model.Email = value;
            }
            else
                model.PerfilID = Convert.ToInt32(value);

            try
            {
                if (!status)
                {
                    _usuariosService.AtualizarAspNetUsers(ConverterTiposObjetosUsuarioViewModelParaAspNetUsers(model));
                    TempData["success"] = "Usuário atualizado com sucesso!";
                    if (propertyName == "Perfil")
                    {
                        value = GetModelPerfil().Where(a => a.Key == model.PerfilID).FirstOrDefault().Value;
                        status = true;
                    }
                    else
                        status = true;
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                TempData["error"] = (message, "Erro ao atualizar usuário");
            }

            var response = new { value = value, status = status, message = message };
            JObject o = JObject.FromObject(response);
            return Content(o.ToString());
        }

        public ActionResult GetPerfis(int usuarioID)
        {
            string selectedPerfilID = "";
            StringBuilder sb = new StringBuilder();
            int? perfilUsuario = _usuariosService.CarregarUsuario(usuarioID).PerfilID;

            var listPerfil = GetModelPerfil().OrderBy(a => a.Key).ToList();

            foreach (var item in listPerfil)
            {
                sb.Append(string.Format("'{0}':'{1}',", item.Key, item.Value));
            }

            selectedPerfilID = listPerfil.Where(a => a.Key == perfilUsuario).First().Value;

            sb.Append(string.Format("'selected': '{0}'", selectedPerfilID));
            return Content("{" + sb.ToString() + "}");
        }

        public ActionResult EditCheckd(int usuarioID, int value)
        {
            var message = "";
            UsuarioModel model = new UsuarioModel();
            model.UsuarioID = usuarioID;
            model.Status = Convert.ToBoolean(value);

            try
            {
                _usuariosService.AtualizarAspNetUsers(ConverterTiposObjetosUsuarioViewModelParaAspNetUsers(model));
                TempData["success"] = "Usuário atualizado com sucesso!";
            }
            catch (Exception ex)
            {
                message = ex.Message;
                TempData["error"] = (message, "Erro ao atualizar status usuário");
            }
            return RedirectToAction("Index");
        }

        public ActionResult Excluir(int usuID, int value = 0)
        {
            var message = "";
            IList<Pedido> pedidos = _pedidoService.CarregarPedidos(usuID);
            
            if(pedidos.Count() > 0)
            {
                TempData["error"] = "Não é permitido excluir este usuário, o mesmo já possui pedido vinculado";
                return RedirectToAction("Index");
            }

            try
            {
                _usuariosService.ExcluirUsuario(usuID);
                TempData["success"] = "Usuário excluído com sucesso!";
            }
            catch (Exception ex)
            {
                message = ex.Message;
                TempData["error"] = (message, "Erro ao excluir usuário");
            }
            return RedirectToAction("Index");
        }

        #endregion

        #region Methods

        private AspNetUsers ConverterTiposObjetosUsuarioViewModelParaAspNetUsers(UsuarioModel model)
        {
            AspNetUsers user = new AspNetUsers();
            user.Id = model.UsuarioID;
            user.Nome = model.Nome;
            user.CGC = model.CPF;
            user.Email = model.Email;
            user.PhoneNumber = model.Telefone;
            user.IsEnabled = model.Status.GetValueOrDefault();
            user.PerfilID = model.PerfilID.GetValueOrDefault();

            return user;
        }

        private UsuarioModel ConverterTiposObjetosUsuarioParaUsuarioViewModel(AspNetUsers user) {
			UsuarioModel model = new UsuarioModel();
            PerfilModel modelPerfil = new PerfilModel();

            model.UsuarioID = user.Id;
            model.Nome = user.Nome;
            model.CPF = user.CGC;
            model.Telefone = user.PhoneNumber;
            model.Email = user.Email;
            model.Status = user.IsEnabled;
            model.DataCriacao = user.DataCadastro;
            model.PerfilID = user.PerfilID;            
            model.Perfil = GetModelPerfil().Where(p => p.Key == user.PerfilID).FirstOrDefault().Value;

            return model;
		}

        public IDictionary<int, string> GetModelPerfil()
        {
            var result = new Dictionary<int, string>();
            result.Add(0, "Selecione");

            var perfis = (List<Perfil>)_perfilService.CarregarPerfis();

            perfis.ForEach(d => result.Add(d.PerfilID, d.Descricao));

            return result;
        }
		#endregion
	}
}