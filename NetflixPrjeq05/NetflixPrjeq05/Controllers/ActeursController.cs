using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NetflixPrjeq05.Models;
using NetflixPrjeq05.Service;

namespace NetflixPrjeq05.Controllers
{
    public class ActeursController : Controller
    {       
        private BDService service = new BDService(new Entities());

        // GET: Acteurs
        public ActionResult Index(int? id)
        {
            if (ContenusController.m_tousLesPays == null)
                ContenusController.m_tousLesPays = service.GetAllPays();

            ViewBag.Pays = new SelectList(ContenusController.m_tousLesPays, "PaysId", "Nom", ContenusController.currentPaysId);
            return View(service.GetAllActeurs());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                service.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
