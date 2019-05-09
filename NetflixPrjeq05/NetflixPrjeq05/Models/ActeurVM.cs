using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetflixPrjeq05.Models
{
    public class ActeurVM
    {
        public ActeurVM() { }

        public ActeurVM(Acteur acteur)
        {
            ActeurId = acteur.ActeurId;
            Nom = acteur.Nom;
            //Path = acteur.pat
        }

        public int ActeurId { get; set; }
        public string Nom { get; set; }
        //public string Path { get; set; }
    }
}