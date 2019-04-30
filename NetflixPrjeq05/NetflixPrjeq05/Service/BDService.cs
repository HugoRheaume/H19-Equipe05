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



    }
}