using KeViraKombinaTodos.Core.Models;
using KeViraKombinaTodos.Core.Services;
using KeViraKombinaTodos.Web.Extensions;
using KeViraKombinaTodos.Web.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace KeViraKombinaTodos.Web.Controllers
{
    public class HomeController : Controller
    {
        #region Inject
        private IPerfilService _perfilService;

        public HomeController(IPerfilService perfilService)
        {
            _perfilService = perfilService ?? throw new ArgumentNullException(nameof(perfilService));
        }
        #endregion
        public ActionResult Index()
        {
            IList<PerfilModel> model = new List<PerfilModel>();
            //PerfilModel model = new PerfilModel();
            Perfil perfil = _perfilService.CarregarPerfil(User.Identity.GetPerfilID());
            if(perfil != null)
            {
                PerfilModel modelPerfil = new PerfilModel();

                modelPerfil = ConverterTiposObjetosPerfilParaPerfilViewModel(perfil);
                model.Add(modelPerfil);
            }
            return View(model);
        }
        private PerfilModel ConverterTiposObjetosPerfilParaPerfilViewModel(Perfil perfil)
        {
            PerfilModel model = new PerfilModel();

            PropertyCopier<Perfil, PerfilModel>.Copy(perfil, model);

            return model;
        }
    }
}