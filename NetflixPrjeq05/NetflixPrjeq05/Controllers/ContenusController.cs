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
using PagedList;


namespace NetflixPrjeq05.Controllers
{
    public class ContenusController : Controller
    {
        private BDService service = new BDService(new Entities());
        public static int currentPaysId = 1;
        public static string m_sortOrder;

        //========================================================================================================================================================
        public ActionResult Index(int paysId)
        {
            ViewBag.Region = new List<string>() { "Ca", "Fr", "Eu" };
            var queryContenu = service.getAllContenuByPays(paysId);

            List<Contenu> colContenu = queryContenu.ToList();
            return View(colContenu);

        }
        //============================================================================CONTENU============================================================================
        public ActionResult Contenu(int? id, string sortOrder, int? page)
        {              
            ViewBag.Pays = new SelectList(service.GetAllPays(), "PaysId", "Nom", currentPaysId);
            if (id != null)
                currentPaysId = (int)id;

            if(sortOrder != null)
                m_sortOrder = sortOrder;

            var queryContenu = service.getAllContenuByPays(currentPaysId);

            List<Contenu> colContenu = queryContenu.ToList();
            List<ContenuVM> colContenuVM = new List<ContenuVM>();
            foreach (var item in colContenu)
            {
                ContenuVM contenuVM = new ContenuVM(item);
                //Doublages              
                var queryLangue = service.getLangueDoublageByContenuId(contenuVM.ContenuId);
                string langues = string.Join(", ",queryLangue.ToList());
                contenuVM.Doublages = langues;
                //Origines             
                var queryOrigines = service.getOriginePaysByContenuId(contenuVM.ContenuId);
                string origines = string.Join(", ", queryOrigines.ToList());
                contenuVM.Origines = origines;
                colContenuVM.Add(contenuVM);
            }

            //Sorting matters titre / date sortie / duree
            ViewBag.NameSortParm = m_sortOrder == "titre_asc" ? "titre_desc" : "titre_asc";           
            ViewBag.DateSortParm = m_sortOrder == "date_asc" ? "date_desc" : "date_asc";
            ViewBag.DureeSortParm = m_sortOrder == "duree_asc" ? "duree_desc" : "duree_asc";

            switch (m_sortOrder)
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
            //Pagination
            int pageSize = 2;
            int pageNumber = (page ?? 1);
            return View(colContenuVM.ToPagedList(pageNumber, pageSize));

        }           
        //============================================================================INFORMATION============================================================================
        public ActionResult Details(int? id)
        {
            ViewBag.Pays = new SelectList(service.GetAllPays(), "PaysId", "Nom", currentPaysId);
            if (id != null)
                currentPaysId = (int)id;



            return View();
        }
       
        //============================================================================AJOUTER============================================================================
        public ActionResult Create()
        {
            return View();
        }
        
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "ContenuId,Description,Affiche,Cote_moyenne,Nombre_de_Cote,Status,Budget,Titre_Original,Date_de_sortie,Duree")] Contenu contenu)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Contenu.Add(contenu);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(contenu);
        //}

        //========================================================================================================================================================
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contenu contenu = service.GetContenuByID(id.Value);
            if (contenu == null)
            {
                return HttpNotFound();
            }
            return View(contenu);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "ContenuId,Description,Affiche,Cote_moyenne,Nombre_de_Cote,Status,Budget,Titre_Original,Date_de_sortie,Duree")] Contenu contenu)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(contenu).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(contenu);
        //}
        public ActionResult Ajouter(int? id)
        {
            ViewBag.Pays = new SelectList(service.GetAllPays(), "PaysId", "Nom", currentPaysId);
            if (id != null)
                currentPaysId = (int)id;

            List<Contenu> queryContenuPays = service.getAllContenuByPays(currentPaysId);
            List<Contenu> colContenu = service.GetAllContenu().Except(queryContenuPays).ToList();
           
            List<ContenuVM> colContenuVM = new List<ContenuVM>();
            foreach (var item in colContenu)
            {
                ContenuVM contenuVM = new ContenuVM(item);
                //Doublages              
                var queryLangue = service.getLangueDoublageByContenuId(contenuVM.ContenuId);
                string langues = string.Join(", ", queryLangue.ToList());
                contenuVM.Doublages = langues;
                //Origines             
                var queryOrigines = service.getOriginePaysByContenuId(contenuVM.ContenuId);
                string origines = string.Join(", ", queryOrigines.ToList());
                contenuVM.Origines = origines;
                colContenuVM.Add(contenuVM);
            }
            return View(colContenuVM);
        }
        //========================================================================================================================================================
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }          
            Contenu contenu = service.GetAllContenu().ElementAt((int)id);            
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
            int offreId = service.GetAllOffreContenu().Where(o => o.ContenuId == id && o.PaysId == currentPaysId).Single().OffrePaysId;
            service.RemoveOffre(offreId);
            return RedirectToAction("Contenu");
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
