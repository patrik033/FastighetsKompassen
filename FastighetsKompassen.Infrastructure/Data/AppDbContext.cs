
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
                .OnDelete(DeleteBehavior.Cascade); // Undvik kaskad för att förebygga cykler  //kankse restrict?

            modelBuilder.Entity<KommunData>()
                .HasOne(k => k.EducationData)
                .WithOne()
                .HasForeignKey<EducationLevelData>(e => e.KommunDataId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<KommunData>()
                .HasMany(k => k.PoliceEvents)
                .WithOne(p => p.Kommun)
                .HasForeignKey(p => p.KommunDataId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<KommunData>()
                .HasMany(k => k.RealEstateDataList)
                .WithOne(r => r.Kommun)
                .HasForeignKey(r => r.KommunDataId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<KommunData>()
                .HasMany(k => k.SchoolResultsForGrade6)
                .WithOne(s => s.Kommun)
                .HasForeignKey(s => s.KommunId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<KommunData>()
                .HasMany(k => k.SchoolResultsForGrade9)
                .WithOne(s => s.Kommun)
                .HasForeignKey(s => s.KommunId)
                .OnDelete(DeleteBehavior.Cascade);



            modelBuilder.Entity<RealEstateData>()
                .HasOne(r => r.Kommun)
                .WithMany(k => k.RealEstateDataList)
                .HasForeignKey(r => r.KommunDataId)
                .OnDelete(DeleteBehavior.Cascade);


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
