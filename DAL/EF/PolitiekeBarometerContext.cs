using Domain;
using Domain.Dashboards;
using Domain.Elementen;
using Domain.Platformen;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EF
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public Gebruiker Gebruiker { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }
    //[DbConfigurationType(typeof(PolitiekeBarometerConfiguration))]
    public class PolitiekeBarometerContext : IdentityDbContext
    {
        public static PolitiekeBarometerContext Create()
        {
            return new PolitiekeBarometerContext();
        }
        //Dashboards
        public DbSet<Alert> Alerts { get; set; }
        public DbSet<Melding> Meldingen { get; set; }
        public DbSet<DataConfig> DataConfigs { get; set; }
        public DbSet<Dashboard> Dashboards { get; set; }
        public DbSet<Grafiek> Grafieken { get; set; }
        public DbSet<Filter> Filters { get; set; }
        public DbSet<Zone> Zones { get; set; }
        public DbSet<Item> Items { get; set; }
        //Elementen
        public DbSet<Keyword> Keywords { get; set; }
        public DbSet<Persoon> Personen { get; set; }
        public DbSet<Thema> Themas { get; set; }
        public DbSet<Organisatie> Organisaties { get; set; }
        //Platformen
        public DbSet<Gebruiker> Gebruikers { get; set; }
        public DbSet<Platform> Platformen { get; set; }
        public DbSet<Deelplatform> Deelplatformen { get; set; }
        public DbSet<DeelplatformDashboard> DeelplatformDashboards { get; set; }
        //Posts
        public DbSet<Post> Posts { get; set; }

        private readonly bool delaySave;

        public PolitiekeBarometerContext(bool unitOfWorkPresent = false) : base("Politieke_BarometerDB")
        {
            delaySave = unitOfWorkPresent;

            Database.SetInitializer<PolitiekeBarometerContext>(new PolitiekeBarometerInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Thema>().HasMany<Keyword>(kw => kw.Keywords);
            modelBuilder.Entity<Keyword>().HasMany<Thema>(t => t.Themas);

            modelBuilder.Entity<Post>().HasMany<Keyword>(kw => kw.Keywords);
            modelBuilder.Entity<Post>().HasMany<Persoon>(p => p.Personen);
            modelBuilder.Entity<Persoon>().HasMany<Post>(p => p.Posts);
            modelBuilder.Entity<Keyword>().HasMany<Post>(t => t.Posts);

            modelBuilder.Entity<Organisatie>().HasRequired<Deelplatform>(e => e.Deelplatform);
            modelBuilder.Entity<Thema>().HasRequired<Deelplatform>(e => e.Deelplatform);
            modelBuilder.Entity<Persoon>().HasRequired<Deelplatform>(e => e.Deelplatform);

            modelBuilder.Entity<Organisatie>().HasMany<Persoon>(p => p.Personen);

            modelBuilder.Entity<Alert>().HasRequired<DataConfig>(a => a.DataConfig);

            modelBuilder.Entity<DataConfig>().HasRequired<Element>(dc => dc.Element);

            modelBuilder.Entity<DataConfig>().HasMany<Filter>(dc => dc.Filters);

            modelBuilder.Entity<Grafiek>().HasMany<DataConfig>(g => g.Dataconfigs);

            modelBuilder.Entity<Gebruiker>().HasMany<Dashboard>(g => g.Dashboards);

            modelBuilder.Entity<Dashboard>().HasMany<Zone>(db => db.Zones);

            modelBuilder.Entity<DeelplatformDashboard>().HasMany<Item>(dpd => dpd.Items);

            modelBuilder.Entity<DeelplatformDashboard>().HasRequired<Deelplatform>(dpd => dpd.Deelplatform);

            modelBuilder.Entity<Zone>().HasMany<Item>(z => z.Items);

            modelBuilder.Entity<Alert>().HasMany<Melding>(a => a.Meldingen);

            modelBuilder.Entity<Melding>().HasRequired<Alert>(m => m.Alert);

            modelBuilder.Entity<ApplicationUser>().HasRequired<Gebruiker>(g => g.Gebruiker);

            modelBuilder.Entity<ApplicationUser>().HasMany<IdentityUserRole>((ApplicationUser u) => u.Roles);
            base.OnModelCreating(modelBuilder);

        }

        public override int SaveChanges()
        {
            if (delaySave)
            {
                return -1;
            }
            return base.SaveChanges();
        }

        internal int CommitChanges()
        {
            if (delaySave)
            {
                return base.SaveChanges();
            }
            throw new InvalidOperationException("Geen UnitOfWork, gebruik SaveChanges");
        }
    }
}
