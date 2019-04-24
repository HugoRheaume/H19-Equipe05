using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NetflixPrjeq05.Controllers
{
    public class HomeController : Controller
    {
        private DB db = new DB();

        public ActionResult Index(int regionId)
        {
            ViewBag.Region = new List<string>() { "Ca", "Fr", "Eu" };
            var queryContenu = from C in db.Contenu
                        join CR in db.Contenu_Region on C.contenuId equals CR.contenuId
                        where CR.RegionId == regionId
                        orderby C.Date_de_sortie descending
                        select C;

            var queryLangue = from C in db.Contenu
                               join CL in db.Contenu_Langue on C.contenuId equals CL.contenuId
                               join L in db.langue on CL.langueId equals L.langueId
                               where CL.RegionId == regionId
                               orderby C.Date_de_sortie descending
                               select C;

            //List<Contenu> colContenu = db.Contenu.ToList().OrderByDescending(c => c.Date_de_sortie);

            return View(colContenu);
        }

        public ActionResult About()
        {
            List <Contenu> colContenu = db.Contenu.ToList();
           
            return View(colContenu);
        }

        public ActionResult Create()
        {
            List<Contenu> colContenu = db.Contenu.ToList();
            //List<Contenu> colContenuPays = 
            //ViewBag.MultiSelectUtilisateurs = new MultiSelectList(db.Personnes.OfType<Utilisateur>().ToList(), "PersonneId", "NomPrenom");
            //ViewBag.TypeActivites = new SelectList(db.TypeActivites, "TypeActiviteId", "NomActivite");
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}