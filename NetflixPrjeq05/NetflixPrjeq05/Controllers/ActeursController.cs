using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NetflixPrjeq05.Models;

namespace NetflixPrjeq05.Controllers
{
    public class ActeursController : Controller
    {
        private Entities db = new Entities();

        // GET: Acteurs
        public ActionResult Index()
        {
            ViewBag.Pays = new SelectList(ContenusController.m_tousLesPays, "PaysId", "Nom", ContenusController.currentPaysId);
            return View(db.Acteur.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
