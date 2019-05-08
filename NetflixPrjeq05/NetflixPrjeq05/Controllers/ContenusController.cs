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
        public static List<ContenuVM> m_colContenuIndisponibleCourant;
        public static List<ContenuVM> m_colContenuDisponibleCourant;
        public static List<Contenu> m_tousLeContenu;
        public static List<Pays> m_tousLesPays;
        public static List<string> m_listMessages;
        public static string m_searchContenuDispo = "";
        public static string m_searchContenuIndispo = "";       


        //============================================================================CONTENU============================================================================
        public ActionResult Contenu(int? id, string sortOrder, int? page, string searchTitle)
        {
            //Seulement effectué quand le controleur est appellé pour la première fois.
            if (m_tousLeContenu == null)
                m_tousLeContenu = service.GetAllContenu();

            if (m_tousLesPays == null)
                m_tousLesPays = service.GetAllPays();

            if(searchTitle != null)
                m_searchContenuDispo = searchTitle;

            m_searchContenuIndispo = "";
            List<ContenuVM> colContenuVM = m_colContenuDisponibleCourant == null ? new List<ContenuVM>() : m_colContenuDisponibleCourant;
            ViewBag.Pays = new SelectList(m_tousLesPays, "PaysId", "Nom", currentPaysId);           
                              
            //Pagination
            int pageSize = 3;
            int pageNumber = (page ?? 1);

            if (sortOrder != null)
                m_sortOrder = sortOrder;
            else if (m_colContenuDisponibleCourant != null)
                m_sortOrder = null;

            if (sortOrder != null)
                m_sortOrder = sortOrder;
            else if (m_colContenuDisponibleCourant != null)
                m_sortOrder = null;

            //Sorting matters titre / date sortie / duree
            ViewBag.NameSortParm = m_sortOrder == "titre_asc" ? "titre_desc" : "titre_asc";
            ViewBag.DateSortParm = m_sortOrder == "date_asc" ? "date_desc" : "date_asc";
            ViewBag.DureeSortParm = m_sortOrder == "duree_asc" ? "duree_desc" : "duree_asc";

            if (!page.HasValue)
            {   //Peut etre optimisé en recodant le fonctionnement de la liste courante
                if ((id.HasValue && id!=currentPaysId) || m_colContenuDisponibleCourant == null)
                {
                    if (id.HasValue)
                    {
                        currentPaysId = id.Value;
                        m_colContenuIndisponibleCourant = null;
                    }
                        
                    var queryContenu = service.GetAllContenuByPays(currentPaysId);
                    List<Contenu> colContenu = queryContenu.ToList();
                    colContenuVM.Clear();

                    foreach (var item in colContenu)
                    {
                        ContenuVM contenuVM = new ContenuVM(item);
                        //Doublages              
                        var queryLangue = service.GetLangueDoublageByContenuId(contenuVM.ContenuId);
                        string langues = string.Join(", ", queryLangue.ToList());
                        contenuVM.Doublages = langues;
                        //Origines             
                        var queryOrigines = service.GetOriginePaysByContenuId(contenuVM.ContenuId);
                        string origines = string.Join(", ", queryOrigines.ToList());
                        contenuVM.Origines = origines;
                        colContenuVM.Add(contenuVM);
                    }
                     m_colContenuDisponibleCourant = colContenuVM;
                }               
            }

            if (m_searchContenuDispo != null && m_searchContenuDispo != string.Empty)
                colContenuVM = colContenuVM.Where(x => x.Titre.ToLower().Contains(m_searchContenuDispo.ToLower())).ToList();

            //Sorting matters titre / date sortie / duree
            colContenuVM = GetSortedContenuVM(colContenuVM);
            
            //Sorting ends here     
            ViewBag.CurrentSearch = m_searchContenuDispo;
            ViewBag.MessagesErreur = m_listMessages;
            m_listMessages = null;
            return View(colContenuVM.ToPagedList(pageNumber, pageSize));
        }
        //============================================================================AJOUTER============================================================================
        public ActionResult Ajouter(int? id, string sortOrder, int? page, string searchTitle)
        {
            //Seulement effectués quand le controleur est appellé pour la première fois.
            if (m_tousLesPays == null)
                m_tousLesPays = service.GetAllPays();

            if (m_tousLeContenu == null)
                m_tousLeContenu = service.GetAllContenu();

            if (searchTitle != null)
                m_searchContenuIndispo = searchTitle;
        
            m_searchContenuDispo = "";
            ViewBag.Pays = new SelectList(m_tousLesPays, "PaysId", "Nom", currentPaysId);
            List<ContenuVM> colContenuVM = m_colContenuIndisponibleCourant == null ? new List<ContenuVM>() : m_colContenuIndisponibleCourant;      
           
            //Pagination
            int pageSize = 3;
            int pageNumber = (page ?? 1);
           
            if (sortOrder != null)
                m_sortOrder = sortOrder;
            else if (m_colContenuIndisponibleCourant != null)
                m_sortOrder = null;

            ViewBag.NameSortParm = m_sortOrder == "titre_asc" ? "titre_desc" : "titre_asc";
            ViewBag.DateSortParm = m_sortOrder == "date_asc" ? "date_desc" : "date_asc";
            ViewBag.DureeSortParm = m_sortOrder == "duree_asc" ? "duree_desc" : "duree_asc";

            //Empêche de trier une seconde fois la liste quand on veut juste changer de page.
            if (!page.HasValue)
            {
                //Pays a été modifié à partir de menu déroulant ou            
                if ((id.HasValue && id != currentPaysId) || m_colContenuIndisponibleCourant == null)
                {
                    if (id.HasValue)
                    {
                        currentPaysId = id.Value;
                        m_colContenuDisponibleCourant = null;
                    }
                        
                    List<Contenu> queryContenuPays;
                    List<Contenu> colContenu;
                                                        
                    queryContenuPays = service.GetAllContenuByPays(currentPaysId);
                    List<int> contenuPaysIds = queryContenuPays.Select(c => c.ContenuId).ToList();                   
                    colContenu = m_tousLeContenu.Where(c => !contenuPaysIds.Contains(c.ContenuId)).ToList();
                    colContenuVM.Clear();

                    foreach (var item in colContenu)
                    {
                        ContenuVM contenuVM = new ContenuVM(item);
                        //Doublages                                    
                        List<string> queryLangue = service.GetLangueDoublageByContenuId(contenuVM.ContenuId);
                        string langues = string.Join(", ", queryLangue.ToList());
                        contenuVM.Doublages = langues;
                        //Origines                                   
                        List<string> queryOrignine = service.GetOriginePaysByContenuId(contenuVM.ContenuId);
                        string origines = string.Join(", ", queryOrignine.ToList());
                        contenuVM.Origines = origines;
                        colContenuVM.Add(contenuVM);
                    }

                    m_colContenuIndisponibleCourant = colContenuVM;
                }             
            }

            if (m_searchContenuIndispo != null && m_searchContenuIndispo != string.Empty)
                colContenuVM = colContenuVM.Where(x => x.Titre.ToLower().Contains(m_searchContenuIndispo.ToLower())).ToList();

            //Sorting matters titre / date sortie / duree
            colContenuVM = GetSortedContenuVM(colContenuVM);

            //Sorting ends here            
            ViewBag.MessagesErreur = m_listMessages;
            ViewBag.CurrentSearch = m_searchContenuIndispo;
            m_listMessages = null;
            return View(colContenuVM.ToPagedList(pageNumber, pageSize));            
        }       
        //============================================================================AJOUTER============================================================================    
        #region Ajouter
        public ActionResult AjouterContenu(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }                         
            ContenuVM contenu = m_colContenuIndisponibleCourant.Where(c => c.ContenuId == id.Value).First();
            OffrePays offrePays = new OffrePays();
            offrePays.ContenuId = id.Value;
            offrePays.PaysId = currentPaysId;
            service.AjouterOffre(offrePays);
            //Mettre à jour les listes courantes 
            if (m_colContenuIndisponibleCourant != null)
                m_colContenuIndisponibleCourant.Remove(m_colContenuIndisponibleCourant.Where(c => c.ContenuId == id).First());
            if (m_colContenuDisponibleCourant != null)
                m_colContenuDisponibleCourant.Add(contenu);
                       
            //Administration des messages d'erreur pour pourcentages
            m_listMessages = GetMessagesPourcentages();
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
                var contenuTest = m_colContenuIndisponibleCourant.Where(c => c.ContenuId == episode.ContenuId).ToList();
                //Vérification si episode est déja disponible
                if (contenuTest.Count != 0)
                {
                    ContenuVM contenu = m_colContenuIndisponibleCourant.Where(c => c.ContenuId == episode.ContenuId).First();
                    OffrePays offrePays = new OffrePays();
                    offrePays.ContenuId = episode.ContenuId;
                    offrePays.PaysId = currentPaysId;
                    service.AjouterOffre(offrePays);
                    //Mettre à jour les listes courantes 
                    if (m_colContenuIndisponibleCourant != null)
                        m_colContenuIndisponibleCourant.Remove(m_colContenuIndisponibleCourant.Where(c => c.ContenuId == episode.ContenuId).First());
                    if (m_colContenuDisponibleCourant != null)
                        m_colContenuDisponibleCourant.Add(contenu);
                }
            }
            //Administration des messages d'erreur pour pourcentages

            m_listMessages = GetMessagesPourcentages();
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
                var contenuTest = m_colContenuIndisponibleCourant.Where(c => c.ContenuId == episode.ContenuId).ToList();

                //Vérification si episode est déja disponible
                if (contenuTest.Count != 0)
                {
                    ContenuVM contenu = m_colContenuIndisponibleCourant.Where(c => c.ContenuId == episode.ContenuId).First();

                    OffrePays offrePays = new OffrePays();
                    offrePays.ContenuId = episode.ContenuId;
                    offrePays.PaysId = currentPaysId;
                    service.AjouterOffre(offrePays);
                    //Mettre à jour les listes courantes 
                    if (m_colContenuIndisponibleCourant != null)
                        m_colContenuIndisponibleCourant.Remove(m_colContenuIndisponibleCourant.Where(c => c.ContenuId == episode.ContenuId).First());
                    if (m_colContenuDisponibleCourant != null)
                        m_colContenuDisponibleCourant.Add(contenu);
                }
            }
            //Administration des messages d'erreur pour pourcentages
            m_listMessages = GetMessagesPourcentages();
            return RedirectToAction("Ajouter");
        }

        #endregion

        //============================================================================DELETE====================================================================
        #region Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int offreId = service.GetAllOffreContenu().Where(o => o.ContenuId == id && o.PaysId == currentPaysId).First().OffrePaysId;
            service.RemoveOffre(offreId);
            //Mettre à jour les listes courantes           
            ContenuVM contenuVM = m_colContenuDisponibleCourant.Where(c => c.ContenuId == id.Value).First();
                      
            if (m_colContenuIndisponibleCourant != null)
                m_colContenuIndisponibleCourant.Add(contenuVM);
            if (m_colContenuDisponibleCourant != null)
                m_colContenuDisponibleCourant.Remove(m_colContenuDisponibleCourant.Where(c => c.ContenuId == id).First());

            //Administration des messages d'erreur pour pourcentages
            m_listMessages = GetMessagesPourcentages();           
            return RedirectToAction("Contenu");
        }

        public ActionResult DeleteSaison(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<Contenu> colEpisodes = service.GetSaisonEpisodes(id.Value);
            foreach (var episode in colEpisodes)
            {
                var contenuTest = m_colContenuDisponibleCourant.Where(c => c.ContenuId == episode.ContenuId).ToList();
                //Vérification si episode est déja disponible
                if (contenuTest.Count != 0)
                {
                    ContenuVM contenuVM = m_colContenuDisponibleCourant.Where(c => c.ContenuId == episode.ContenuId).First();
                    int offreId = service.GetAllOffreContenu().Where(o => o.ContenuId == episode.ContenuId && o.PaysId == currentPaysId).First().OffrePaysId;
                    service.RemoveOffre(offreId);
                    //Met a jour listes courantes

                    if (m_colContenuIndisponibleCourant != null)
                        m_colContenuIndisponibleCourant.Add(contenuVM);
                    if (m_colContenuDisponibleCourant != null)
                        m_colContenuDisponibleCourant.Remove(m_colContenuDisponibleCourant.Where(c => c.ContenuId == episode.ContenuId).First());
                }
            }
            //Administration des messages d'erreur pour pourcentages
            m_listMessages = GetMessagesPourcentages();
            return RedirectToAction("Contenu");
        }

        public ActionResult DeleteSerie(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<Contenu> colEpisodes = service.GetSerieEpisodes(id.Value);
            foreach (var episode in colEpisodes)
            {
                var contenuTest = m_colContenuDisponibleCourant.Where(c => c.ContenuId == episode.ContenuId).ToList();
                //Vérification si episode est déja disponible
                if (contenuTest.Count != 0)
                {
                    ContenuVM contenuVM = m_colContenuDisponibleCourant.Where(c => c.ContenuId == episode.ContenuId).First();
                    int offreId = service.GetAllOffreContenu().Where(o => o.ContenuId == episode.ContenuId && o.PaysId == currentPaysId).First().OffrePaysId;
                    service.RemoveOffre(offreId);
                    //Met a jour listes courantes

                    if (m_colContenuIndisponibleCourant != null)
                        m_colContenuIndisponibleCourant.Add(contenuVM);
                    if (m_colContenuDisponibleCourant != null)
                        m_colContenuDisponibleCourant.Remove(m_colContenuDisponibleCourant.Where(c => c.ContenuId == episode.ContenuId).First());
                }
            }
            //Administration des messages d'erreur pour pourcentages
            m_listMessages = GetMessagesPourcentages();
            return RedirectToAction("Contenu");
        }
        #endregion
        //========================================================================================================================================================       
        public List<string> GetMessagesPourcentages()
        {
            List<Regle> regles = service.GetAllReglementForPays(currentPaysId);
            List<string> messages = new List<string>();
            foreach (var regle in regles)
            {
                if (regle.EstPlusGrand.Value && !(regle.PourcentageReel >= regle.Pourcentage))
                {
                    messages.Add("Le pourcentage " + (regle.OriginePaysId.HasValue ?
                        "de contenu provenant du pays " + m_tousLesPays[regle.OriginePaysId.Value].Nom
                        : (" de contenu doublé en " + service.GetAllLangue().Where(r => r.LangueId == regle.DoublageLangueId.Value).First().Nom))
                        + " doit être supérieur ou égal à " + regle.Pourcentage + "%"
                        + "\n Pourcentage actuel: " + Math.Round(regle.PourcentageReel.Value, 2) + "%");
                }

                if (!regle.EstPlusGrand.Value && !(regle.PourcentageReel <= regle.Pourcentage))
                {
                    messages.Add("Le pourcentage " + (regle.OriginePaysId.HasValue ?
                        "de contenu provenant du pays " + m_tousLesPays[regle.OriginePaysId.Value].Nom
                        : (" de contenu doublé en " + service.GetAllLangue().Where(r => r.LangueId == regle.DoublageLangueId.Value).First().Nom))
                        + " doit être inférieur ou égal à " + regle.Pourcentage + "%"
                        + "\n Pourcentage actuel: " + Math.Round(regle.PourcentageReel.Value, 2) + "%");
                }
            }
            return messages;
        }

        public List<ContenuVM> GetSortedContenuVM(List<ContenuVM> colContenuVM)
        {
            switch (m_sortOrder)
            {
                case "titre_desc":
                    return  colContenuVM.OrderByDescending(c => c.Titre).ToList();
                case "titre_asc":
                    return colContenuVM.OrderBy(c => c.Titre).ToList();
                case "date_desc":
                    return colContenuVM.OrderByDescending(c => c.DateSortie).ToList();
                case "date_asc":
                    return colContenuVM.OrderBy(c => c.DateSortie).ToList();
                case "duree_desc":
                    return colContenuVM.OrderByDescending(c => c.Duree).ToList();
                case "duree_asc":
                    return colContenuVM.OrderBy(c => c.Duree).ToList();
                default:
                    return colContenuVM.OrderByDescending(c => c.ContenuId).ToList();                   
            }
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
