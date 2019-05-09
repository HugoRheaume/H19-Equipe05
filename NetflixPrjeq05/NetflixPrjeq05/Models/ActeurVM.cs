using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetflixPrjeq05.Models
{
    public class ActeurVM
    {
        public ActeurVM() { }

        public ActeurVM(string nom, string affiche, int totalVues, int score)
        {
            Nom = nom;
            Affiche = "https://image.tmdb.org/t/p/w600_and_h900_bestv2" + affiche;
            TotalVues = totalVues;
            Score = score;
        }

        public int ActeurId { get; set; }
        public string Nom { get; set; }
        public string Affiche { get; set; }
        public int TotalVues { get; set; }
        public int Score { get; set; }
    }
}