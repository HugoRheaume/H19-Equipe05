﻿using System;
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
        public static int currentPaysId = 1;

        //========================================================================================================================================================
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
        //============================================================================CONTENU============================================================================
        public ActionResult Contenu(int? id, string sortOrder)
        {      
            
            ViewBag.Pays = new SelectList(db.Pays.ToList(), "PaysId", "Nom", currentPaysId);
            if (id != null)
                currentPaysId = (int)id;

            var queryContenu = from C in db.Contenu
                               join CR in db.OffrePays on C.ContenuId equals CR.ContenuId
                               where CR.PaysId == currentPaysId                              
                               select C;

            List<Contenu> colContenu = queryContenu.ToList();
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

            //Sorting matters titre / date sortie / duree
            ViewBag.NameSortParm = sortOrder == "titre_asc" ? "titre_desc" : "titre_asc";           
            ViewBag.DateSortParm = sortOrder == "date_asc" ? "date_desc" : "date_asc";
            ViewBag.DureeSortParm = sortOrder == "duree_asc" ? "duree_desc" : "duree_asc";

            switch (sortOrder)
            {
                case "titre_desc":
                    colContenuVM = colContenuVM.OrderByDescending(c => c.Titre).ToList();
                    break;
                case "titre_asc":
                    colContenuVM = colContenuVM.OrderBy(c => c.Titre).ToList();
                    break;
                case "date_desc":
                    colContenuVM = colContenuVM.OrderByDescending(c => c.DateSortie).ToList();
                    break;
                case "date_asc":
                    colContenuVM = colContenuVM.OrderBy(c => c.DateSortie).ToList();
                    break;
                case "duree_desc":
                    colContenuVM = colContenuVM.OrderByDescending(c => c.Duree).ToList();
                    break;
                case "duree_asc":
                    colContenuVM = colContenuVM.OrderBy(c => c.Duree).ToList();
                    break;

                default:
                    colContenuVM = colContenuVM.OrderByDescending(c => c.ContenuId).ToList();
                    break;
            }
            //Sorting ends here

            return View(colContenuVM);

        }           
        //============================================================================INFORMATION============================================================================
        public ActionResult Details(int? id)
        {
            ViewBag.Pays = new SelectList(db.Pays.ToList(), "PaysId", "Nom", currentPaysId);
            if (id != null)
                currentPaysId = (int)id;



            return View();
        }
       
        //============================================================================AJOUTER============================================================================
        public ActionResult Create()
        {
            return View();
        }
        
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

        //========================================================================================================================================================
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

        //========================================================================================================================================================
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

        //========================================================================================================================================================
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
