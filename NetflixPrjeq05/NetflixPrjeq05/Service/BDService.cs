using NetflixPrjeq05.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace NetflixPrjeq05.Service
{
    public class BDService : BaseService
    {
        public BDService(Entities db) : base(db)
        {
        }

        //Reglement
        public List<Regle> GetAllReglement()
        {
            return db.Regle.ToList();
        }

        public List<Regle> GetAllReglementForPays(int paysId)
        {
            return db.Regle.Where(x => x.PaysId==paysId).ToList();
        }
       
        //============================================================================CONTENU OPTIMISÉ============================================================================
        public List<ContenuVM> GetContenuDisponible(int paysId)
        {
            List<usp_GetContenuDisponible_Result> t = new List<usp_GetContenuDisponible_Result>();
            List<ContenuVM> contenuVMs = new List<ContenuVM>();
            var queryContenu = db.usp_GetContenuDisponible(paysId);
            foreach (var item in queryContenu)
            {
                contenuVMs.Add(new ContenuVM(item.ContenuId.Value, item.Date_de_sortie, item.Duree.Value, item.Titre_Original, item.Affiche, item.SaisonId, item.Languages, item.Origines));
            }
            return contenuVMs;
        }

        public List<ContenuVM> GetContenuIndisponible(int paysId)
        {
            List<usp_GetContenuIndisponible_Result> t = new List<usp_GetContenuIndisponible_Result>();
            List<ContenuVM> contenuVMs = new List<ContenuVM>();
            var queryContenu = db.usp_GetContenuIndisponible(paysId);
            foreach (var item in queryContenu)
            {
                contenuVMs.Add(new ContenuVM(item.ContenuId, item.Date_de_sortie, item.Duree.Value, item.Titre_Original, item.Affiche, item.SaisonId, item.Languages, item.Origines));
            }
            return contenuVMs;
        }

        public List<Contenu> GetAllContenu()
        {
            return db.Contenu.ToList();
        }
              
        //PAYS
        public List<Pays> GetAllPays()
        {
            return db.Pays.ToList();
        }

       
        //Langue
        public List<Langue> GetAllLangue()
        {
            return db.Langue.ToList();
        }


        //OffreContenu
        public List<OffrePays> GetAllOffreContenu()
        {
            return db.OffrePays.ToList();
        }
        
        //OrigineContenu       
        public List<string> GetOriginePaysByContenuId(int id)
        {
            return db.OriginePays.Where(x => x.ContenuId == id).Select(y => y.Pays.Nom).ToList();
        }
        public List<string> GetOriginePaysByRegleId(int id)
        {
            var queryDoublageLangue = from r in GetAllReglement()
                                      join p in GetAllPays() on r.OriginePaysId equals p.PaysId
                                      where r.RegleId == id
                                      select p.Nom;

            return queryDoublageLangue.ToList();
        }

        //ContenuLangue      
        public List<string> GetLangueDoublageByContenuId(int id)
        {
            return db.ContenuLangue.Where(x => x.ContenuId == id).Select(y => y.Langue.Nom).ToList();
        }

        public List<string> GetLangueDoublageByRegleId(int id)
        {
            var queryDoublageLangue = from r in GetAllReglement()
                                      join l in GetAllLangue() on r.DoublageLangueId equals l.LangueId
                                      where r.RegleId == id
                                      select l.Nom;

            return queryDoublageLangue.ToList();
        }

        //============================================================================AJOUTER============================================================================        
        public void RemoveOffre(int id)
        {
            db.OffrePays.Remove(db.OffrePays.Find(id));
            db.SaveChanges();
        }
   
        public void AjouterOffre(OffrePays offre)
        {
            db.OffrePays.Add(offre);
            db.SaveChanges();
        }

        public string GetSaisonNumero(int id)
        {
            return db.Saison.Where(s => s.SaisonId == id).Select(o => o.NoSaison).First().ToString();
        }

        public string GetSerieNom(int id)
        {
            int serieId = db.Saison.Where(s => s.SaisonId == id).Select(o => o.SerieId).First();
            return db.Serie.Where(s => s.SerieId == serieId).Select(s => s.Nom).First();
        }

        public List<Contenu> GetSaisonEpisodes(int saisonId)
        {
            return db.Contenu.Where(c => c.SaisonId == saisonId).ToList();
        }

        public List<Contenu> GetSerieEpisodes(int saisonId)
        {
            List<Contenu> colEpisodes = new List<Contenu>();
            int serieId = db.Saison.Where(s => s.SaisonId == saisonId).Select(o => o.SerieId).First();
            List<int> saisonIds = db.Saison.Where(s => s.SerieId == serieId).Select(s => s.SaisonId).ToList();
            foreach (int sauceId in saisonIds)
            {
                List<Contenu> colSaisonEpi = GetSaisonEpisodes(sauceId);
                colEpisodes.AddRange(colSaisonEpi);
            }
            return colEpisodes;
        }
        //============================================================================AJOUTER============================================================================
        public void AddRegle(Regle regle)
        {           
            db.Regle.Add(regle);
            db.SaveChanges();           
        }

        public bool RegleExisteDeja(Regle regle)
        {
            //Langue
            if (regle.DoublageLangueId.HasValue && db.Regle.Where(r => r.DoublageLangueId == regle.DoublageLangueId 
                && r.PaysId == regle.PaysId && r.EstPlusGrand == regle.EstPlusGrand && r.RegleId != regle.RegleId).Count() > 0)
                return true;
            //Origine
            if (regle.OriginePaysId.HasValue && db.Regle.Where(r => r.OriginePaysId == regle.OriginePaysId 
                && r.PaysId == regle.PaysId && r.EstPlusGrand == regle.EstPlusGrand && r.RegleId != regle.RegleId).Count() > 0)
                return true;

            return false;
        }

        public Regle GetRegle(int id)
        {
            return db.Regle.Find(id);           
        }
    
        public void ModifyRegle(Regle regle)
        {
            db.Entry(regle).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void DeleteRegle(Regle regle)
        {
            db.Regle.Remove(regle);
            db.SaveChanges();
        }
        //============================================================================ACTEUR============================================================================        
        public List<ActeurVM> GetTop10ActeursPays(int paysId)
        {
            List<usp_GetTop10Acteurs_Result> t = new List<usp_GetTop10Acteurs_Result>();
            List<ActeurVM> acteurVMs = new List<ActeurVM>();
            var queryTop10Acteurs = db.usp_GetTop10Acteurs(paysId);
            int score = 1;
            foreach (var item in queryTop10Acteurs)
            {
                acteurVMs.Add(new ActeurVM(item.Nom, item.Affiche, item.TotalVues.Value, score));
                score++;
            }
            return acteurVMs;
        }       
    }
}