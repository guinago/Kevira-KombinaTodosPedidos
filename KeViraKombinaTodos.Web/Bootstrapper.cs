using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc3;
using KeViraKombinaTodos.Core.Services;
using KeViraKombinaTodos.Web.Controllers;
using KeViraKombinaTodos.Impl.Services;
using KeViraKombinaTodos.Core.DAO;
using KeViraKombinaTodos.Impl.DAO;

namespace KeViraKombinaTodos.Web
{
    public static class Bootstrapper
    {
        public static void Initialise()
        {
            var container = BuildUnityContainer();            

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();
            //services
            #region
            //container.RegisterType<ApplicationSignInManager, ApplicationSignInManager>();
            //container.RegisterType<ApplicationUserManager, ApplicationUserManager>();


            container.RegisterType<IPedidoService, PedidoService>();
            container.RegisterType<IUsuarioService, UsuariosService>();
            container.RegisterType<IPerfilService, PerfilService>();
            container.RegisterType<ITransportadoraService, TransportadoraService>();
            container.RegisterType<IProdutoService, ProdutoService>();
            container.RegisterType<ICondicaoPagamentoService, CondicaoPagamentoService>();
            container.RegisterType<IRedisService, RedisService>();
            container.RegisterType<IClienteService, ClienteService>();

            #endregion

            // DAO
            #region
            container.RegisterType<IPedidoDao, PedidoDao>();
            container.RegisterType<IAspNetUsersDao, AspNetUsersDao>();
            container.RegisterType<ICondicaoPagamentoDao, CondicaoPagamentoDao>();
            container.RegisterType<IPerfilDao, PerfilDao>();
            container.RegisterType<IProdutoDao, ProdutoDao>();
            container.RegisterType<ITransportadoraDao, TransportadoraDao>();
            container.RegisterType<IClienteDao, ClienteDao>();
            container.RegisterType<IItemPedidoDao, ItemPedidoDao>();
            #endregion

            //controllers
            #region
            container.RegisterType<IController, HomeController>("Store");
            container.RegisterType<IController, AccountController>("Store");

            #endregion


            return container;
        }
    }
}