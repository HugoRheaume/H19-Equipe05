//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NetflixPrjeq05.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Regle
    {
        public int RegleId { get; set; }
        public int PaysId { get; set; }
        public Nullable<int> OriginePaysId { get; set; }
        public Nullable<int> DoublageLangueId { get; set; }
        public Nullable<double> Pourcentage { get; set; }
        public Nullable<bool> EstPlusGrand { get; set; }
        public Nullable<System.DateTime> DateCreation { get; set; }
        public Nullable<double> PourcentageReel { get; set; }
    
        public virtual Langue Langue { get; set; }
        public virtual Pays Pays { get; set; }
        public virtual Pays Pays1 { get; set; }
    }
}
