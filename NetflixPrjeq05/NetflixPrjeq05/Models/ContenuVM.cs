using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NetflixPrjeq05.Models
{
    public class ContenuVM
    {
        public ContenuVM()
        {
        }

        public ContenuVM(Contenu contenu)
        {                
            ContenuId = contenu.ContenuId;
            DateSortie = contenu.Date_de_sortie == null ? new DateTime(): contenu.Date_de_sortie.Value;           
            Duree = contenu.Duree;          
            Titre = contenu.Titre_Original;
            Affiche = contenu.Affiche;
            SaisonId = contenu.SaisonId;
        }


        public int ContenuId { get; set; }      
        
        public string Titre { get; set; }

        public string Status { get; set; }

        public string Affiche { get; set; }

        public int Budget { get; set; }

        public decimal CoteMoyenne { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [DisplayName("Date de sortie")]
        public DateTime DateSortie { get; set; }

        public string Description { get; set; }

        [DisplayName("Durée")]
        public int? Duree { get; set; }
    
        public int NbCotes { get; set; }
                     
        public string Doublages { get; set; }

        public string Origines { get; set; }
        public int? SaisonId { get; set; }

    }
}