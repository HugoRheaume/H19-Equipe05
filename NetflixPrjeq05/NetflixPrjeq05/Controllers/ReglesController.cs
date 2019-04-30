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
    public class ReglesController : Controller
    {
        private BDService service = new BDService(new Entities());

        // GET: Regles
        public ActionResult Index(int? id)
        {
            if (id != null)
                ContenusController.currentPaysId = id.Value;

            int paysId = ContenusController.currentPaysId;
            ViewBag.Pays = new SelectList(service.GetAllPays(), "PaysId", "Nom", paysId);
            List<RegleVM> regleVMs = new List<RegleVM>();
            List<Regle> regles = service.GetAllReglementForPays(paysId);
            foreach (Regle rv in regles )
            {
                RegleVM regleVm = new RegleVM(rv);
                var queryDoublageLangue = from r in service.GetAllReglement()
                                          join l in service.GetAllLangue() on r.DoublageLangueId equals l.LangueId
                                  where r.RegleId ==  regleVm.RegleId
                                  select l.Nom;
                string langues = string.Join(", ", queryDoublageLangue.ToList());
                regleVm.DoublageLangue = langues;
                var queryOriginePays = from r in service.GetAllReglement()
                                       join p in service.GetAllPays() on r.OriginePaysId equals p.PaysId
                                       where r.RegleId == regleVm.RegleId
                                       select p.Nom;
                string origines = string.Join(", ", queryOriginePays.ToList());
                regleVm.OriginePays = origines;
                regleVMs.Add(regleVm);
            }
            return View(regleVMs);
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

        //// GET: Regles/Create
        //public ActionResult Create()
        //{
        //    ViewBag.DoublageLangueId = new SelectList(db.Langue, "LangueId", "Code_ISO639");
        //    ViewBag.OriginePaysId = new SelectList(db.Pays, "PaysId", "Code_ISO3166");
        //    ViewBag.PaysId = new SelectList(db.Pays, "PaysId", "Code_ISO3166");
        //    return View();
        //}

        //// POST: Regles/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "RegleId,PaysId,OriginePaysId,DoublageLangueId,Pourcentage,EstPlusGrand")] Regle regle)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Regle.Add(regle);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.DoublageLangueId = new SelectList(db.Langue, "LangueId", "Code_ISO639", regle.DoublageLangueId);
        //    ViewBag.OriginePaysId = new SelectList(db.Pays, "PaysId", "Code_ISO3166", regle.OriginePaysId);
        //    ViewBag.PaysId = new SelectList(db.Pays, "PaysId", "Code_ISO3166", regle.PaysId);
        //    return View(regle);
        //}

        //// GET: Regles/Edit/5
        //public ActionResult Edit(int? id)
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
        //    ViewBag.DoublageLangueId = new SelectList(db.Langue, "LangueId", "Code_ISO639", regle.DoublageLangueId);
        //    ViewBag.OriginePaysId = new SelectList(db.Pays, "PaysId", "Code_ISO3166", regle.OriginePaysId);
        //    ViewBag.PaysId = new SelectList(db.Pays, "PaysId", "Code_ISO3166", regle.PaysId);
        //    return View(regle);
        //}

        //// POST: Regles/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "RegleId,PaysId,OriginePaysId,DoublageLangueId,Pourcentage,EstPlusGrand")] Regle regle)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(regle).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.DoublageLangueId = new SelectList(db.Langue, "LangueId", "Code_ISO639", regle.DoublageLangueId);
        //    ViewBag.OriginePaysId = new SelectList(db.Pays, "PaysId", "Code_ISO3166", regle.OriginePaysId);
        //    ViewBag.PaysId = new SelectList(db.Pays, "PaysId", "Code_ISO3166", regle.PaysId);
        //    return View(regle);
        //}

        //// GET: Regles/Delete/5
        //public ActionResult Delete(int? id)
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

        //// POST: Regles/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Regle regle = db.Regle.Find(id);
        //    db.Regle.Remove(regle);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
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
