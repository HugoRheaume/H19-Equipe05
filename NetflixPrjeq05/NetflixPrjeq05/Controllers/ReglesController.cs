using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public class ReglesController : Controller
    {
        private BDService service = new BDService(new Entities());

        //============================================================================INDEX============================================================================
        public ActionResult Index(int? id)
        { 
            if (id != null)
            {
                ContenusController.currentPaysId = id.Value;
                ContenusController.m_colContenuDisponibleCourant = null;
                ContenusController.m_colContenuIndisponibleCourant = null;
            }
            
            int paysId = ContenusController.currentPaysId;
            ViewBag.Pays = new SelectList(service.GetAllPays(), "PaysId", "Nom", paysId);
            List<RegleVM> regleVMs = new List<RegleVM>();
            List<Regle> regles = service.GetAllReglementForPays(paysId);
            foreach (Regle rv in regles )
            {
                RegleVM regleVm = new RegleVM(rv);
                var queryDoublageLangue = service.GetLangueDoublageByRegleId(regleVm.RegleId);
                string langues = string.Join(", ", queryDoublageLangue);
                regleVm.DoublageLangue = langues;
                var queryOriginePays = service.GetOriginePaysByRegleId(regleVm.RegleId);
                string origines = string.Join(", ", queryOriginePays);
                regleVm.OriginePays = origines;
                regleVMs.Add(regleVm);
            }
            return View(regleVMs);
        }

        //============================================================================CREATE============================================================================


        // DOUBLE FONCTIONNE PAS <--------------------------------------------------------------////////////////////////////////////////////////////////////
        public ActionResult CreateOrigine()
        {
            int paysId = ContenusController.currentPaysId;
            ViewBag.DoublageLangueId = new SelectList(service.GetAllLangue(), "LangueId", "Nom");
            ViewBag.OriginePaysId = new SelectList(service.GetAllPays(), "PaysId", "Nom", paysId);
            ViewBag.PaysId = new SelectList(service.GetAllPays(), "PaysId", "Nom", paysId);
            ViewBag.AfficheErreur = false;
            return View();
        }
        public ActionResult CreateLangue()
        {
            int paysId = ContenusController.currentPaysId;
            ViewBag.DoublageLangueId = new SelectList(service.GetAllLangue(), "LangueId", "Nom");
            ViewBag.OriginePaysId = new SelectList(service.GetAllPays(), "PaysId", "Nom", paysId);
            ViewBag.PaysId = new SelectList(service.GetAllPays(), "PaysId", "Nom", paysId);
            ViewBag.AfficheErreur = false;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOrigine([Bind(Include = "RegleId,PaysId,OriginePaysId,DoublageLangueId,Pourcentage,EstPlusGrand")] Regle regle)
        {           
            if (ModelState.IsValid)
            {
                regle.PaysId = ContenusController.currentPaysId;
                regle.DateCreation = DateTime.Now;

                if (!service.RegleExisteDeja(regle))
                {                    
                    service.AddRegle(regle);
                    return RedirectToAction("Index");
                }      
                else
                    ViewBag.AfficheErreur = true;
            }
            List<Pays> allPays = service.GetAllPays();
            ViewBag.DoublageLangueId = new SelectList(service.GetAllLangue(), "LangueId", "Nom", regle.DoublageLangueId);
            ViewBag.OriginePaysId = new SelectList(allPays, "PaysId", "Nom", regle.OriginePaysId);
            ViewBag.PaysId = new SelectList(allPays, "PaysId", "Nom", regle.PaysId);           
            return View(regle);
        }


        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateLangue([Bind(Include = "RegleId,PaysId,OriginePaysId,DoublageLangueId,Pourcentage,EstPlusGrand")] Regle regle)
        {
            if (ModelState.IsValid)
            {
                regle.PaysId = ContenusController.currentPaysId;
                regle.DateCreation = DateTime.Now;

                if (!service.RegleExisteDeja(regle))
                {
                    service.AddRegle(regle);
                    return RedirectToAction("Index");
                }
                else
                    ViewBag.AfficheErreur = true;
            }
            List<Pays> allPays = service.GetAllPays();
            ViewBag.DoublageLangueId = new SelectList(service.GetAllLangue(), "LangueId", "Nom", regle.DoublageLangueId);
            ViewBag.OriginePaysId = new SelectList(allPays, "PaysId", "Nom", regle.OriginePaysId);
            ViewBag.PaysId = new SelectList(allPays, "PaysId", "Nom", regle.PaysId);            
            return View(regle);
        }


        //============================================================================EDIT============================================================================
        public ActionResult EditOrigine(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Regle regle = service.GetRegle(id.Value);
                      
            if (regle == null)
            {
                return HttpNotFound();
            }
            int paysId = ContenusController.currentPaysId;
            
            ViewBag.OriginePaysId = new SelectList(service.GetAllPays(), "PaysId", "Nom", regle.OriginePaysId);           
            return View(regle);
        }

        public ActionResult EditLangue(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Regle regle = service.GetRegle(id.Value);

            if (regle == null)
            {
                return HttpNotFound();
            }
            int paysId = ContenusController.currentPaysId;
            ViewBag.DoublageLangueId = new SelectList(service.GetAllLangue(), "LangueId", "Nom", regle.DoublageLangueId);
            return View(regle);
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditLangue([Bind(Include = "RegleId,PaysId,OriginePaysId,DoublageLangueId,Pourcentage,EstPlusGrand")] Regle regle)
        {
            int paysId = ContenusController.currentPaysId;
            if (ModelState.IsValid)
            {
                regle.PaysId = ContenusController.currentPaysId;
                regle.DateCreation = DateTime.Now;
                service.ModifyRegle(regle);
                return RedirectToAction("Index");
            }
            ViewBag.DoublageLangueId = new SelectList(service.GetAllLangue(), "LangueId", "Nom", regle.DoublageLangueId);
            return View(regle);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditOrigine([Bind(Include = "RegleId,PaysId,OriginePaysId,DoublageLangueId,Pourcentage,EstPlusGrand")] Regle regle)
        {
            int paysId = ContenusController.currentPaysId;
            if (ModelState.IsValid)
            {
                regle.PaysId = ContenusController.currentPaysId;
                regle.DateCreation = DateTime.Now;
                service.ModifyRegle(regle);
                return RedirectToAction("Index");
            }
            ViewBag.OriginePaysId = new SelectList(service.GetAllPays(), "PaysId", "Nom", regle.OriginePaysId);
            return View(regle);
        }
        //============================================================================DELETE============================================================================
        public ActionResult DeleteOrigine(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Regle regle = service.GetRegle(id.Value);
            if (regle == null)
            {
                return HttpNotFound();
            }
            service.DeleteRegle(regle);
            return RedirectToAction("Index");
        }
        public ActionResult DeleteLangue(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Regle regle = service.GetRegle(id.Value);           
            if (regle == null)
            {
                return HttpNotFound();
            }
            service.DeleteRegle(regle);
            return RedirectToAction("Index");
        }
        

        //// GET: Regles/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Regle regle = db.Regle.Find(id);
        //    if (regle == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(regle);
        //}

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
