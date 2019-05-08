using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NetflixPrjeq05.Models
{
    public class RegleVM
    {
        public RegleVM()
        {

        }
        public RegleVM(Regle regle)
        {
            RegleId = regle.RegleId;
            DateCreation = regle.DateCreation;
            PaysId = regle.PaysId;
            OriginePaysId = regle.OriginePaysId;
            DoublageLangueId = regle.DoublageLangueId;
            Pourcentage = regle.Pourcentage;
            PourcentageReel = regle.PourcentageReel;
            EstPlusGrand = regle.EstPlusGrand;
            
        }
        public int RegleId { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [DisplayName("Date de sortie")]
        public DateTime? DateCreation { get; set; }

        public int PaysId { get; set; }

        public int? OriginePaysId { get; set; }

        public int? DoublageLangueId { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = @"{0:0.00\%}")]       
        public double?  Pourcentage { get; set; }

        [DisplayName("Pourcentage actuel")]
        [DisplayFormat(DataFormatString = @"{0:0.00\%}")]       
        public double? PourcentageReel { get; set; }

        [DisplayName("Langue de doublage")]
        public string DoublageLangue { get; set; }

        [DisplayName("Pays d'origine")]
        public string OriginePays { get; set; }

        [Required]
        public bool? EstPlusGrand { get; set; }

    }
}