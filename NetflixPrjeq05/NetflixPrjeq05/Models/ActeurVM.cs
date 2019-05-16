using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            Affiche = affiche;
            TotalVues = totalVues;
            Score = score;
            
        }

        public int ActeurId { get; set; }
        public string Nom { get; set; }
        [DisplayName("Image")]
        public string Affiche { get; set; }
        [DisplayName("Nombre de vues")]
        public int TotalVues { get; set; }
        public int Score { get; set; }
    }
}