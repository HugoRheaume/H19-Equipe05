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
    public class ContenusController : Controller
    {
        private Entities db = new Entities();

        // GET: Contenus
        public ActionResult Index(int paysId)
        {
            ViewBag.Region = new List<string>() { "Ca", "Fr", "Eu" };
            var queryContenu = from C in db.Contenu
                               join CR in db.OffrePays on C.ContenuId equals CR.ContenuId
                               where CR.PaysId == paysId
                               orderby C.Date_de_sortie descending
                               select C;

            List<Contenu> colContenu = queryContenu.ToList();
            return View(colContenu);

        }

        public ActionResult Contenu()
        {
            //int paysId = 2;           
            ViewBag.Pays = new SelectList(db.Pays.ToList(), "PaysId", "Nom");
            //ViewBag.Pays = new List<string>() { "Ca", "Fr", "Eu" };
            //var queryContenu = from C in db.Contenu
            //                   join CR in db.ContenuPays on C.ContenuId equals CR.ContenuId
            //                   where CR.PaysId == paysId
            //                   orderby C.Date_de_sortie descending
            //                   select C;

            List<Contenu> colContenu = db.Contenu.ToList();//queryContenu.ToList();
            List<ContenuVM> colContenuVM = new List<ContenuVM>();
            foreach (var item in colContenu)
            {
                ContenuVM contenuVM = new ContenuVM(item);
                //Doublages              
                var queryLangue = from C in db.Contenu
                                  join CL in db.ContenuLangue on C.ContenuId equals CL.ContenuId
                                  join L in db.Langue on CL.LangueId equals L.LangueId
                                  where C.ContenuId == contenuVM.ContenuId
                                  select L.Nom;
                string langues = string.Join(", ",queryLangue.ToList());
                contenuVM.Doublages = langues;
                //Origines             
                var queryOrigines = from C in db.Contenu
                                  join OP in db.OriginePays on C.ContenuId equals OP.ContenuId
                                  join L in db.Pays on OP.PaysId equals L.PaysId
                                  where C.ContenuId == contenuVM.ContenuId
                                  select L.Nom;
                string origines = string.Join(", ", queryOrigines.ToList());
                contenuVM.Origines = origines;
                colContenuVM.Add(contenuVM);
            }
           
            //List<Contenu> colContenu = db.Contenu.ToList().OrderByDescending(c => c.Date_de_sortie);


            return View(colContenuVM); //colContenu

        }

        // GET: Contenus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contenu contenu = db.Contenu.Find(id);
            if (contenu == null)
            {
                return HttpNotFound();
            }
            return View(contenu);
        }

        // GET: Contenus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Contenus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ContenuId,Description,Affiche,Cote_moyenne,Nombre_de_Cote,Status,Budget,Titre_Original,Date_de_sortie,Duree")] Contenu contenu)
        {
            if (ModelState.IsValid)
            {
                db.Contenu.Add(contenu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(contenu);
        }

        // GET: Contenus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contenu contenu = db.Contenu.Find(id);
            if (contenu == null)
            {
                return HttpNotFound();
            }
            return View(contenu);
        }

        // POST: Contenus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ContenuId,Description,Affiche,Cote_moyenne,Nombre_de_Cote,Status,Budget,Titre_Original,Date_de_sortie,Duree")] Contenu contenu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contenu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(contenu);
        }

        // GET: Contenus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contenu contenu = db.Contenu.Find(id);
            if (contenu == null)
            {
                return HttpNotFound();
            }
            return View(contenu);
        }

        // POST: Contenus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Contenu contenu = db.Contenu.Find(id);
            db.Contenu.Remove(contenu);
            db.SaveChanges();
            return RedirectToAction("Index");
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
