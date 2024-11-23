
using FastighetsKompassen.Shared.Models;
using FastighetsKompassen.Shared.Models.PoliceData;
using FastighetsKompassen.Shared.Models.RealEstate;
using FastighetsKompassen.Shared.Models.SkolData;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastighetsKompassen.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<KommunData> Kommuner { get; set; }
        public DbSet<PoliceEvent> PoliceEvents { get; set; }
        public DbSet<RealEstateData> RealEstateData { get; set; }
        public DbSet<SchoolResultGradeSix> SchoolResultsGradeSix { get; set; }
        public DbSet<SchoolResultGradeNine> SchoolResultsGradeNine { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<KommunData>()
            .HasOne(k => k.LifeExpectancy) // KommunData har en LifeExpectancy
            .WithOne(l => l.Kommun) // LifeExpectancyData har en Kommun
            .HasForeignKey<LifeExpectancyData>(l => l.KommunDataId) // Främmande nyckel i LifeExpectancyData
            .OnDelete(DeleteBehavior.Cascade); // Cascaderande borttagning

            modelBuilder.Entity<KommunData>()
            .HasOne(k => k.Income)
            .WithOne(i => i.Kommun)
            .HasForeignKey<ScbValues>(i => i.KommunDataId)
            .OnDelete(DeleteBehavior.Restrict); // Undvik kaskad för att förebygga cykler


            //    // Konfiguration för SchoolResultsForGrade6
            //modelBuilder.Entity<SchoolResultGradeSix>()
            //    .HasOne<KommunData>() // Ingen navigation tillbaka från SchoolResult till KommunData
            //    .WithMany(k => k.SchoolResultsForGrade6)
            //    .HasForeignKey("KommunDataId") // Främmande nyckel
            //    .OnDelete(DeleteBehavior.Cascade); // Cascaderande borttagning

            ////    // Konfiguration för SchoolResultsForGrade9
            //modelBuilder.Entity<SchoolResultGradeSix>()
            //    .HasOne<KommunData>()
            //    .WithMany(k => k.SchoolResultsForGrade9)
            //    .HasForeignKey("KommunDataId")
            //    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ScbValues>(entity =>
            {
                entity.Property(e => e.MiddleValue)
                    .HasPrecision(18, 2); // Precision: 18 siffror totalt, 2 efter decimalpunkten
            });




            // Övriga tabeller
            base.OnModelCreating(modelBuilder);
        }
    }
}
