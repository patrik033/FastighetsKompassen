
using FastighetsKompassen.Shared.Models;
using FastighetsKompassen.Shared.Models.MapData;
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
        public DbSet<Income> Income { get; set; }
        public DbSet<AverageLifeTime> AverageLifeTime { get; set; }
        public DbSet<AverageMiddleAge> AverageMiddleAge { get; set; }

        public DbSet<SchoolResultGradeSix> SchoolResultsGradeSix { get; set; }
        public DbSet<SchoolResultGradeNine> SchoolResultsGradeNine { get; set; }
        public DbSet<EducationLevelData> EducationLevels { get; set; }

        public DbSet<PoliceEvent> PoliceEvents { get; set; }
        public DbSet<PoliceEventSummary> PoliceEventSummary { get; set; }
        public DbSet<RealEstateData> RealEstateData { get; set; }
        public DbSet<RealEstateYearlySummary> RealEstateYearlySummary { get; set; }
        public DbSet<MapFeatures> Features { get; set; }
        public DbSet<MapGeometry> Geometries { get; set; }
        public DbSet<MapProperties> Properties { get; set; }
        public DbSet<MapTags> Tags { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            


            modelBuilder.Entity<Income>(entity =>
            {
                entity.Property(e => e.MiddleValue)
                    .HasPrecision(18, 2); // Precision: 18 siffror totalt, 2 efter decimalpunkten
            });




            // Övriga tabeller
            base.OnModelCreating(modelBuilder);
        }
    }
}
