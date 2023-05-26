﻿using KeViraKombinaTodos.Core.Services;
using System;
using System.Web.Mvc;

namespace KeViraKombinaTodos.Web.Controllers
{
    public class HomeController : Controller
    {          
        public ActionResult Index()
        {            
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}