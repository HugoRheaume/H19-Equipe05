﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NetflixPrjeq05.Controllers
{
    public class HomeController : Controller
    {
       

        public ActionResult Index(int regionId)
        {

            return View();
        }

        public ActionResult About()
        {
           
           
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}