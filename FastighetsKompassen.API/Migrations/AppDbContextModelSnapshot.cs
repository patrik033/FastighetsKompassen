﻿// <auto-generated />
using System;
using FastighetsKompassen.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FastighetsKompassen.API.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("FastighetsKompassen.Shared.Models.AverageAgeExpectancy", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Female")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("KommunDataId")
                        .HasColumnType("int");

                    b.Property<decimal>("Male")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Total")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("KommunDataId");

                    b.ToTable("AverageAgeExpectancy");
                });

            modelBuilder.Entity("FastighetsKompassen.Shared.Models.KommunData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Kommun")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Kommunnamn")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Kommuner");
                });

            modelBuilder.Entity("FastighetsKompassen.Shared.Models.LifeTimeExpectedData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("FemaleValue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("KommunDataId")
                        .HasColumnType("int");

                    b.Property<decimal>("MaleValue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("YearSpan")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("KommunDataId")
                        .IsUnique();

                    b.ToTable("LifeTimeExpectedData");
                });

            modelBuilder.Entity("FastighetsKompassen.Shared.Models.PoliceData.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Lat")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Lng")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PoliceEventId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PoliceEventId")
                        .IsUnique()
                        .HasFilter("[PoliceEventId] IS NOT NULL");

                    b.ToTable("Location");
                });

            modelBuilder.Entity("FastighetsKompassen.Shared.Models.PoliceData.PoliceEvent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Body")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Datetime")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("KommunDataId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Summary")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("KommunDataId");

                    b.ToTable("PoliceEvents");
                });

            modelBuilder.Entity("FastighetsKompassen.Shared.Models.PoliceData.PoliceEventSummary", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("EventCount")
                        .HasColumnType("int");

                    b.Property<string>("EventType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("KommunId")
                        .HasColumnType("int");

                    b.Property<int?>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("KommunId");

                    b.ToTable("PoliceEventSummary");
                });

            modelBuilder.Entity("FastighetsKompassen.Shared.Models.RealEstate.PriceChangeInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Minus")
                        .HasColumnType("bit");

                    b.Property<bool>("Plus")
                        .HasColumnType("bit");

                    b.Property<int>("RealEstateDataId")
                        .HasColumnType("int");

                    b.Property<int>("Value")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RealEstateDataId")
                        .IsUnique();

                    b.ToTable("PriceChangeInfo");
                });

            modelBuilder.Entity("FastighetsKompassen.Shared.Models.RealEstate.RealEstateData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Area")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Association")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("Balcony")
                        .HasColumnType("bit");

                    b.Property<string>("Broker")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("BuildYear")
                        .HasColumnType("int");

                    b.Property<string>("County")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Fee")
                        .HasColumnType("int");

                    b.Property<string>("HousingForm")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("KommunDataId")
                        .HasColumnType("int");

                    b.Property<double?>("LandArea")
                        .HasColumnType("float");

                    b.Property<double?>("Latitude")
                        .HasColumnType("float");

                    b.Property<double?>("LivingArea")
                        .HasColumnType("float");

                    b.Property<double?>("Longitude")
                        .HasColumnType("float");

                    b.Property<int?>("OperatingCost")
                        .HasColumnType("int");

                    b.Property<string>("OwnershipType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("PropertyType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Rooms")
                        .HasColumnType("int");

                    b.Property<DateTime?>("SoldAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Story")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Street")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("WantedPrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("KommunDataId");

                    b.ToTable("RealEstateData");
                });

            modelBuilder.Entity("FastighetsKompassen.Shared.Models.RealEstate.RealEstateYearlySummary", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("KommunId")
                        .HasColumnType("int");

                    b.Property<string>("PropertyType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SalesCount")
                        .HasColumnType("int");

                    b.Property<decimal?>("TotalSalesAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("KommunId");

                    b.ToTable("RealEstateYearlySummary");
                });

            modelBuilder.Entity("FastighetsKompassen.Shared.Models.ScbValues", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("IncomeComponent")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("KommunDataId")
                        .HasColumnType("int");

                    b.Property<decimal>("MiddleValue")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Sex")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Year")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("KommunDataId");

                    b.ToTable("ScbValues");
                });

            modelBuilder.Entity("FastighetsKompassen.Shared.Models.SkolData.EducationLevelData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Gymnasial")
                        .HasColumnType("int");

                    b.Property<int>("KommunDataId")
                        .HasColumnType("int");

                    b.Property<int>("MissingInfo")
                        .HasColumnType("int");

                    b.Property<int>("PostGymnasial3YearsOrMore")
                        .HasColumnType("int");

                    b.Property<int>("PostGymnasialUnder3Years")
                        .HasColumnType("int");

                    b.Property<int>("PreGymnasial")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("KommunDataId");

                    b.ToTable("EducationLevelData");
                });

            modelBuilder.Entity("FastighetsKompassen.Shared.Models.SkolData.SchoolResultGradeNine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("EducationLevel")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("FemaleGradeAE")
                        .HasColumnType("float");

                    b.Property<double?>("FemaleGradeAF")
                        .HasColumnType("float");

                    b.Property<double?>("FemaleGradePoints")
                        .HasColumnType("float");

                    b.Property<double?>("FemaleParticipation")
                        .HasColumnType("float");

                    b.Property<double?>("GradePoints")
                        .HasColumnType("float");

                    b.Property<string>("HeadOrganizationName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("HeadOrganizationNumber")
                        .HasColumnType("float");

                    b.Property<string>("HeadOrganizationType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("KommunId")
                        .HasColumnType("int");

                    b.Property<double?>("MaleGradeAE")
                        .HasColumnType("float");

                    b.Property<double?>("MaleGradeAF")
                        .HasColumnType("float");

                    b.Property<double?>("MaleGradePoints")
                        .HasColumnType("float");

                    b.Property<double?>("MaleParticipation")
                        .HasColumnType("float");

                    b.Property<double?>("MunicipalityCode")
                        .HasColumnType("float");

                    b.Property<string>("SchoolMunicipality")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SchoolName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SchoolUnitCode")
                        .HasColumnType("int");

                    b.Property<int>("StartYear")
                        .HasColumnType("int");

                    b.Property<string>("SubTest")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TestCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("TotalGradeAE")
                        .HasColumnType("float");

                    b.Property<double?>("TotalGradeAF")
                        .HasColumnType("float");

                    b.Property<double?>("TotalParticipation")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("KommunId");

                    b.ToTable("SchoolResultsGradeNine");
                });

            modelBuilder.Entity("FastighetsKompassen.Shared.Models.SkolData.SchoolResultGradeSix", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("EducationLevel")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("FemaleGradeAE")
                        .HasColumnType("float");

                    b.Property<double?>("FemaleGradeAF")
                        .HasColumnType("float");

                    b.Property<double?>("FemaleGradePoints")
                        .HasColumnType("float");

                    b.Property<double?>("FemaleParticipation")
                        .HasColumnType("float");

                    b.Property<double?>("GradePoints")
                        .HasColumnType("float");

                    b.Property<string>("HeadOrganizationName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("HeadOrganizationNumber")
                        .HasColumnType("float");

                    b.Property<string>("HeadOrganizationType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("KommunId")
                        .HasColumnType("int");

                    b.Property<double?>("MaleGradeAE")
                        .HasColumnType("float");

                    b.Property<double?>("MaleGradeAF")
                        .HasColumnType("float");

                    b.Property<double?>("MaleGradePoints")
                        .HasColumnType("float");

                    b.Property<double?>("MaleParticipation")
                        .HasColumnType("float");

                    b.Property<double?>("MunicipalityCode")
                        .HasColumnType("float");

                    b.Property<string>("SchoolMunicipality")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SchoolName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SchoolUnitCode")
                        .HasColumnType("int");

                    b.Property<int>("StartYear")
                        .HasColumnType("int");

                    b.Property<string>("SubTest")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TestCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("TotalGradeAE")
                        .HasColumnType("float");

                    b.Property<double?>("TotalGradeAF")
                        .HasColumnType("float");

                    b.Property<double?>("TotalParticipation")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("KommunId");

                    b.ToTable("SchoolResultsGradeSix");
                });

            modelBuilder.Entity("FastighetsKompassen.Shared.Models.AverageAgeExpectancy", b =>
                {
                    b.HasOne("FastighetsKompassen.Shared.Models.KommunData", "Kommun")
                        .WithMany("AverageAge")
                        .HasForeignKey("KommunDataId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Kommun");
                });

            modelBuilder.Entity("FastighetsKompassen.Shared.Models.LifeTimeExpectedData", b =>
                {
                    b.HasOne("FastighetsKompassen.Shared.Models.KommunData", "Kommun")
                        .WithOne("LifeTime")
                        .HasForeignKey("FastighetsKompassen.Shared.Models.LifeTimeExpectedData", "KommunDataId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Kommun");
                });

            modelBuilder.Entity("FastighetsKompassen.Shared.Models.PoliceData.Location", b =>
                {
                    b.HasOne("FastighetsKompassen.Shared.Models.PoliceData.PoliceEvent", "PoliceEvent")
                        .WithOne("Location")
                        .HasForeignKey("FastighetsKompassen.Shared.Models.PoliceData.Location", "PoliceEventId");

                    b.Navigation("PoliceEvent");
                });

            modelBuilder.Entity("FastighetsKompassen.Shared.Models.PoliceData.PoliceEvent", b =>
                {
                    b.HasOne("FastighetsKompassen.Shared.Models.KommunData", "Kommun")
                        .WithMany("PoliceEvents")
                        .HasForeignKey("KommunDataId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Kommun");
                });

            modelBuilder.Entity("FastighetsKompassen.Shared.Models.PoliceData.PoliceEventSummary", b =>
                {
                    b.HasOne("FastighetsKompassen.Shared.Models.KommunData", "Kommun")
                        .WithMany("PoliceEventSummary")
                        .HasForeignKey("KommunId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Kommun");
                });

            modelBuilder.Entity("FastighetsKompassen.Shared.Models.RealEstate.PriceChangeInfo", b =>
                {
                    b.HasOne("FastighetsKompassen.Shared.Models.RealEstate.RealEstateData", "RealEstateData")
                        .WithOne("PriceChangeInfo")
                        .HasForeignKey("FastighetsKompassen.Shared.Models.RealEstate.PriceChangeInfo", "RealEstateDataId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RealEstateData");
                });

            modelBuilder.Entity("FastighetsKompassen.Shared.Models.RealEstate.RealEstateData", b =>
                {
                    b.HasOne("FastighetsKompassen.Shared.Models.KommunData", "Kommun")
                        .WithMany("RealEstateDataList")
                        .HasForeignKey("KommunDataId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Kommun");
                });

            modelBuilder.Entity("FastighetsKompassen.Shared.Models.RealEstate.RealEstateYearlySummary", b =>
                {
                    b.HasOne("FastighetsKompassen.Shared.Models.KommunData", "Kommun")
                        .WithMany("RealEstateYearlySummary")
                        .HasForeignKey("KommunId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Kommun");
                });

            modelBuilder.Entity("FastighetsKompassen.Shared.Models.ScbValues", b =>
                {
                    b.HasOne("FastighetsKompassen.Shared.Models.KommunData", "Kommun")
                        .WithMany("Income")
                        .HasForeignKey("KommunDataId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Kommun");
                });

            modelBuilder.Entity("FastighetsKompassen.Shared.Models.SkolData.EducationLevelData", b =>
                {
                    b.HasOne("FastighetsKompassen.Shared.Models.KommunData", "Kommun")
                        .WithMany("EducationData")
                        .HasForeignKey("KommunDataId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Kommun");
                });

            modelBuilder.Entity("FastighetsKompassen.Shared.Models.SkolData.SchoolResultGradeNine", b =>
                {
                    b.HasOne("FastighetsKompassen.Shared.Models.KommunData", "Kommun")
                        .WithMany("SchoolResultsForGrade9")
                        .HasForeignKey("KommunId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Kommun");
                });

            modelBuilder.Entity("FastighetsKompassen.Shared.Models.SkolData.SchoolResultGradeSix", b =>
                {
                    b.HasOne("FastighetsKompassen.Shared.Models.KommunData", "Kommun")
                        .WithMany("SchoolResultsForGrade6")
                        .HasForeignKey("KommunId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Kommun");
                });

            modelBuilder.Entity("FastighetsKompassen.Shared.Models.KommunData", b =>
                {
                    b.Navigation("AverageAge");

                    b.Navigation("EducationData");

                    b.Navigation("Income");

                    b.Navigation("LifeTime");

                    b.Navigation("PoliceEventSummary");

                    b.Navigation("PoliceEvents");

                    b.Navigation("RealEstateDataList");

                    b.Navigation("RealEstateYearlySummary");

                    b.Navigation("SchoolResultsForGrade6");

                    b.Navigation("SchoolResultsForGrade9");
                });

            modelBuilder.Entity("FastighetsKompassen.Shared.Models.PoliceData.PoliceEvent", b =>
                {
                    b.Navigation("Location");
                });

            modelBuilder.Entity("FastighetsKompassen.Shared.Models.RealEstate.RealEstateData", b =>
                {
                    b.Navigation("PriceChangeInfo");
                });
#pragma warning restore 612, 618
        }
    }
}
