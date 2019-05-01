using NetflixPrjeq05.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetflixPrjeq05.Service
{
    public class BDService : BaseService
    {
        public BDService(Entities db) : base(db)
        {
        }

        public List<Regle> GetAllReglement()
        {
            return db.Regle.ToList();
        }

        public List<Regle> GetAllReglementForPays(int paysId)
        {
            return db.Regle.Where(x => x.PaysId==paysId).ToList();
        }

        public List<Pays> GetAllPays()
        {
            return db.Pays.ToList();
        }

        public List<Langue> GetAllLangue()
        {
            return db.Langue.ToList();
        }


        public List<Contenu> GetAllContenu()
        {
            return db.Contenu.ToList();
        }
        public Contenu GetContenuByID(int id)
        {
            return (Contenu)db.Contenu.Where(x => x.ContenuId==id);
        }
        public List<OffrePays> GetAllOffreContenu()
        {
            return db.OffrePays.ToList();
        }
        public List<OriginePays> GetAllOrigineContenu()
        {
            return db.OriginePays.ToList();
        }
        public List<ContenuLangue> GetAllContenuLangue()
        {
            return db.ContenuLangue.ToList();
        }

        public List<Contenu> GetContenuBetween(int debut, int fin)
        {
            return db.Contenu.Where(c => c.ContenuId >= debut && c.ContenuId <= fin).ToList();
        }
    
        public List<Contenu> getAllContenuByPays(int id)
        {
            var queryDoublageLangue = from C in GetAllContenu()
                                      join CR in GetAllOffreContenu() on C.ContenuId equals CR.ContenuId
                                      where CR.PaysId == id
                                      select C;

            return queryDoublageLangue.ToList();
        }

        public List<int> GetAllContenuIdsByPays(int id)
        {
            return db.OffrePays.Where(o => o.PaysId == id).Select(o => o.ContenuId).ToList();
        }

        public List<string> getLangueDoublageByContenuId(int id)
        {
            return db.ContenuLangue.Where(x => x.ContenuId == id).Select(y => y.Langue.Nom).ToList();
        }
        public List<string> getOriginePaysByContenuId(int id)
        {
            return db.OriginePays.Where(x => x.ContenuId == id).Select(y => y.Pays.Nom).ToList();
        }

        public List<string> getLangueDoublageByRegleId(int id)
        {
            var queryDoublageLangue = from r in GetAllReglement()
                                      join l in GetAllLangue() on r.DoublageLangueId equals l.LangueId
                                      where r.RegleId == id
                                      select l.Nom;

            return queryDoublageLangue.ToList();
        }

        public List<string> getOriginePaysByRegleId(int id)
        {
            var queryDoublageLangue = from r in GetAllReglement()
                                      join p in GetAllPays() on r.OriginePaysId equals p.PaysId
                                      where r.RegleId == id
                                      select p.Nom;

            return queryDoublageLangue.ToList();
        }
             
        public void RemoveOffre(int id)
        {
            db.OffrePays.Remove(db.OffrePays.Find(id));
            db.SaveChanges();
        }

        //============================================================================AJOUTER============================================================================
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
    }
}