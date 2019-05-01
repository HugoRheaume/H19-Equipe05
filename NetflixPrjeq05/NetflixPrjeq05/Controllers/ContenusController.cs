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
        public static List<ContenuVM> m_colContenuCourant;
        public static List<Contenu> m_tousLeContenu;
        public static List<Pays> m_tousLesPays;
        

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
            //Seulement effectué quand le controleur est appellé pour la première fois.
            if (m_tousLeContenu == null)
                m_tousLeContenu = service.GetAllContenu();

            if (m_tousLesPays == null)
                m_tousLesPays = service.GetAllPays();

            ViewBag.Pays = new SelectList(m_tousLesPays, "PaysId", "Nom", currentPaysId);
            //m_colContenuCourant = null;
                   
            //Pagination
            int pageSize = 3;
            int pageNumber = (page ?? 1);

            //if (sortOrder == null && m_sortOrder != null)
            //{
            if (id != null)
            {
                currentPaysId = (int)id;
                m_tousLeContenu = null;
            }

            if (sortOrder != null)
                m_sortOrder = sortOrder;

            var queryContenu = service.getAllContenuByPays(currentPaysId);

            List<Contenu> colContenu = queryContenu.ToList();
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
            //}
            //Sorting ends here            
            return View(colContenuVM.ToPagedList(pageNumber, pageSize));

        }
        //============================================================================AJOUTER============================================================================
        public ActionResult Ajouter(int? id, string sortOrder, int? page)
        {
            //Seulement effectués quand le controleur est appellé pour la première fois.
            if (m_tousLesPays == null)
                m_tousLesPays = service.GetAllPays();

            if (m_tousLeContenu == null)
                m_tousLeContenu = service.GetAllContenu();

            ViewBag.Pays = new SelectList(m_tousLesPays, "PaysId", "Nom", currentPaysId);                      
            //Pagination
            int pageSize = 3;
            int pageNumber = (page ?? 1);

            //BUG: SORT_ORDER N'EST PAS PRIS EN PARAMETRE QUAND NOUS TRIONS DEPUIS AUTREPART QUE LA PAGE 1.  
            // ^ empeche aussi de 
            if (sortOrder != null)
                m_sortOrder = sortOrder;
            ViewBag.NameSortParm = m_sortOrder == "titre_asc" ? "titre_desc" : "titre_asc";
            ViewBag.DateSortParm = m_sortOrder == "date_asc" ? "date_desc" : "date_asc";
            ViewBag.DureeSortParm = m_sortOrder == "duree_asc" ? "duree_desc" : "duree_asc";

            //Empêche de trier une seconde fois la liste quand on veut juste changer de page.
            if (!page.HasValue)
            {
                //Pays a été modifié à partir de menu déroulant ou 

                if (id != null || m_colContenuCourant == null)
                {
                    List<Contenu> queryContenuPays;
                    List<Contenu> colContenu;
                    List<ContenuVM> colContenuVM = new List<ContenuVM>();
                    if (id != null)
                        currentPaysId = id.Value;

                    //queryContenuPays = service.getAllContenuByPays(currentPaysId);
                    queryContenuPays = service.getAllContenuByPays(currentPaysId);
                    List<int> contenuPaysIds = queryContenuPays.Select(c => c.ContenuId).ToList();
                    //List<int> contenuPaysIds2 = service.GetAllContenuIdsByPays(currentPaysId);



                    colContenu = m_tousLeContenu.Where(c => !contenuPaysIds.Contains(c.ContenuId)).ToList();
                    
                    foreach (var item in colContenu)
                    {
                        ContenuVM contenuVM = new ContenuVM(item);
                        //Doublages                                    
                        List<string> queryLangue = service.getLangueDoublageByContenuId(contenuVM.ContenuId);
                        string langues = string.Join(", ", queryLangue.ToList());
                        contenuVM.Doublages = langues;
                        //Origines                                   
                        List<string> queryOrignine = service.getOriginePaysByContenuId(contenuVM.ContenuId);
                        string origines = string.Join(", ", queryOrignine.ToList());
                        contenuVM.Origines = origines;
                        colContenuVM.Add(contenuVM);
                    }
                    m_colContenuCourant = colContenuVM;
                }
               
                

                //List<Contenu> queryContenuPays = service.getAllContenuByPays(currentPaysId);
                //List<Contenu> colContenu =  service.GetAllContenu().Except(queryContenuPays).ToList();
                

                //Sorting matters titre / date sortie / duree


                switch (m_sortOrder)
                {
                    case "titre_desc":
                        m_colContenuCourant = m_colContenuCourant.OrderByDescending(c => c.Titre).ToList();
                        break;
                    case "titre_asc":
                        m_colContenuCourant = m_colContenuCourant.OrderBy(c => c.Titre).ToList();
                        break;
                    case "date_desc":
                        m_colContenuCourant = m_colContenuCourant.OrderByDescending(c => c.DateSortie).ToList();
                        break;
                    case "date_asc":
                        m_colContenuCourant = m_colContenuCourant.OrderBy(c => c.DateSortie).ToList();
                        break;
                    case "duree_desc":
                        m_colContenuCourant = m_colContenuCourant.OrderByDescending(c => c.Duree).ToList();
                        break;
                    case "duree_asc":
                        m_colContenuCourant = m_colContenuCourant.OrderBy(c => c.Duree).ToList();
                        break;

                    default:
                        m_colContenuCourant = m_colContenuCourant.OrderByDescending(c => c.ContenuId).ToList();
                        break;
                }
                
            }
            //Sorting ends here          
            return View(m_colContenuCourant.ToPagedList(pageNumber, pageSize));            
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
        
        public ActionResult AjouterContenu(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContenuVM contenu = m_colContenuCourant.Where(c => c.ContenuId == id.Value).First();
            OffrePays offrePays = new OffrePays();
            offrePays.ContenuId = id.Value;
            offrePays.PaysId = currentPaysId;
            service.AjouterOffre(offrePays);
            //Met a jour liste courante
            m_colContenuCourant.Remove(contenu);
            if (contenu == null)
            {
                return HttpNotFound();
            }
            //return View();
            return RedirectToAction("Ajouter");
        }

       
        public ActionResult AjouterSaison(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }           
            List<Contenu> colEpisodes = service.GetSaisonEpisodes(id.Value);
            foreach (var episode in colEpisodes)
            {
                var contenuTest = m_colContenuCourant.Where(c => c.ContenuId == episode.ContenuId).ToList();
                //Vérification si episode est déja disponible
                if (contenuTest.Count != 0)
                {
                    ContenuVM contenu = m_colContenuCourant.Where(c => c.ContenuId == episode.ContenuId).First();
                    OffrePays offrePays = new OffrePays();
                    offrePays.ContenuId = episode.ContenuId;
                    offrePays.PaysId = currentPaysId;
                    service.AjouterOffre(offrePays);
                    //Met a jour liste courante
                    m_colContenuCourant.Remove(contenu);
                }
            }
            //if (contenu == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(contenu);            
            return RedirectToAction("Ajouter");
        }

      
        public ActionResult AjouterSerie(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<Contenu> colEpisodes = service.GetSerieEpisodes(id.Value);
            foreach (var episode in colEpisodes)
            {
                var contenuTest = m_colContenuCourant.Where(c => c.ContenuId == episode.ContenuId).ToList();

                //Vérification si episode est déja disponible
                if (contenuTest.Count != 0)
                {
                    ContenuVM contenu = m_colContenuCourant.Where(c => c.ContenuId == episode.ContenuId).First();

                    OffrePays offrePays = new OffrePays();
                    offrePays.ContenuId = episode.ContenuId;
                    offrePays.PaysId = currentPaysId;
                    service.AjouterOffre(offrePays);
                    //Met a jour liste courante
                    m_colContenuCourant.Remove(contenu);
                }
            }
            //if (contenu == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(contenu);
            return RedirectToAction("Ajouter");
        }

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
        
        //========================================================================================================================================================
        public ActionResult Delete(int? id)
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

        //========================================================================================================================================================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {           
            int offreId = service.GetAllOffreContenu().Where(o => o.ContenuId == id && o.PaysId == currentPaysId).First().OffrePaysId;
            service.RemoveOffre(offreId);

            if (m_colContenuCourant != null)
            {
                Contenu contenu = m_tousLeContenu.Where(c => c.ContenuId == id).First();
                ContenuVM contenuVM = new ContenuVM(contenu);
                //Doublages                                    
                List<string> queryLangue = service.getLangueDoublageByContenuId(contenuVM.ContenuId);
                string langues = string.Join(", ", queryLangue.ToList());
                contenuVM.Doublages = langues;
                //Origines                                   
                List<string> queryOrignine = service.getOriginePaysByContenuId(contenuVM.ContenuId);
                string origines = string.Join(", ", queryOrignine.ToList());
                contenuVM.Origines = origines;
                m_colContenuCourant.Add(contenuVM);
            }
            return RedirectToAction("Contenu");
        }

        public string GetSerieNom(int id)
        {
            return service.GetSerieNom(id);
        }

        public string GetSaisonNumero(int id)
        {
            return service.GetSaisonNumero(id);
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
