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
using Microsoft.Reporting.WebForms;
using System.Web.UI.WebControls;

namespace NetflixPrjeq05.Controllers
{
    public class ReglesController : Controller
    {
        private BDService service = new BDService(new Entities());
        private ReportViewer reportViewer = new ReportViewer()
        {
            ProcessingMode = ProcessingMode.Remote,
            SizeToReportContent = true,
            Width = Unit.Percentage(100),
            Height = Unit.Percentage(100),
        };

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
                var queryDoublageLangue = service.getLangueDoublageByRegleId(regleVm.RegleId);
                string langues = string.Join(", ", queryDoublageLangue);
                regleVm.DoublageLangue = langues;
                var queryOriginePays = service.getOriginePaysByRegleId(regleVm.RegleId);
                string origines = string.Join(", ", queryOriginePays);
                regleVm.OriginePays = origines;
                regleVMs.Add(regleVm);
            }
            return View(regleVMs);
        }

        // GET: Regles/Details/5
        public ActionResult Report()
        {
            reportViewer.ServerReport.ReportPath = "/Eq05Rapport/RapportReglement";
            reportViewer.ServerReport.ReportServerUrl = new Uri("http://ed4sql2/ReportServer/");
            var startDate = DateTime.Now;
            var parameters = new List<ReportParameter>
            { new ReportParameter("Date", startDate.ToString())};
            reportViewer.ServerReport.SetParameters(parameters);
            ViewBag.ReportViewer = reportViewer;
            return View();

        }
        //protected override void OnPreRender(EventArgs e)
        //{
        //    base.OnPreRender(e);
        //    DatePickers.Value = string.Join(",", (new List(GetDateParameters()).ToArray()));
        ////}
        //private IEnumerable GetDateParameters()
        //{
        //    // I'm assuming report view control id as reportViewer
        //    foreach (ReportParameterInfo info in reportViewer.ServerReport.GetParameters())
        //    {
        //        if (info.DataType == ParameterDataType.DateTime)
        //        {
        //            yield return string.Format("[{0}]", info.Prompt);
        //        }
        //    }
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
