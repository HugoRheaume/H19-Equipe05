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
    
    public partial class ContenuLangue
    {
        public int ContenuLangueId { get; set; }
        public int ContenuId { get; set; }
        public int LangueId { get; set; }
    
        public virtual Contenu Contenu { get; set; }
        public virtual Langue Langue { get; set; }
    }
}
