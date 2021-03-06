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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class Entities : DbContext
    {
        public Entities()
            : base("name=Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Contenu> Contenu { get; set; }
        public virtual DbSet<Langue> Langue { get; set; }
        public virtual DbSet<OffrePays> OffrePays { get; set; }
        public virtual DbSet<OriginePays> OriginePays { get; set; }
        public virtual DbSet<ContenuLangue> ContenuLangue { get; set; }
        public virtual DbSet<Pays> Pays { get; set; }
        public virtual DbSet<Regle> Regle { get; set; }
        public virtual DbSet<Saison> Saison { get; set; }
        public virtual DbSet<Serie> Serie { get; set; }
        public virtual DbSet<Acteur> Acteur { get; set; }
        public virtual DbSet<ContenuActeur> ContenuActeur { get; set; }
        public virtual DbSet<Vue> Vue { get; set; }
    
        public virtual ObjectResult<usp_GetTop10Acteurs_Result> usp_GetTop10Acteurs(Nullable<int> paysId)
        {
            var paysIdParameter = paysId.HasValue ?
                new ObjectParameter("paysId", paysId) :
                new ObjectParameter("paysId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_GetTop10Acteurs_Result>("usp_GetTop10Acteurs", paysIdParameter);
        }
    }
}
