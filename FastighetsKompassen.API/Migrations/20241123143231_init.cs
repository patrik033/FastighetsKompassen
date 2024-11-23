using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FastighetsKompassen.API.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kommuner",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kommun = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Kommunnamn = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kommuner", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PriceChangeInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Plus = table.Column<bool>(type: "bit", nullable: false),
                    Minus = table.Column<bool>(type: "bit", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceChangeInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EducationLevelData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PreGymnasial = table.Column<int>(type: "int", nullable: false),
                    Gymnasial = table.Column<int>(type: "int", nullable: false),
                    PostGymnasialUnder3Years = table.Column<int>(type: "int", nullable: false),
                    PostGymnasial3YearsOrMore = table.Column<int>(type: "int", nullable: false),
                    MissingInfo = table.Column<int>(type: "int", nullable: false),
                    KommunDataId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationLevelData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EducationLevelData_Kommuner_KommunDataId",
                        column: x => x.KommunDataId,
                        principalTable: "Kommuner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LifeExpectancyData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Male = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Female = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    KommunDataId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LifeExpectancyData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LifeExpectancyData_Kommuner_KommunDataId",
                        column: x => x.KommunDataId,
                        principalTable: "Kommuner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PoliceEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Datetime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KommunDataId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoliceEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PoliceEvents_Kommuner_KommunDataId",
                        column: x => x.KommunDataId,
                        principalTable: "Kommuner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PoliceEventSummary",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Year = table.Column<int>(type: "int", nullable: false),
                    EventType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventCount = table.Column<int>(type: "int", nullable: false),
                    KommunDataId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoliceEventSummary", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PoliceEventSummary_Kommuner_KommunDataId",
                        column: x => x.KommunDataId,
                        principalTable: "Kommuner",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RealEstateYearlySummary",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Year = table.Column<int>(type: "int", nullable: false),
                    PropertyType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SalesCount = table.Column<int>(type: "int", nullable: false),
                    TotalSalesAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    KommunDataId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RealEstateYearlySummary", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RealEstateYearlySummary_Kommuner_KommunDataId",
                        column: x => x.KommunDataId,
                        principalTable: "Kommuner",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SchoolResultsGradeNine",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EducationLevel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SchoolName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SchoolUnitCode = table.Column<int>(type: "int", nullable: false),
                    SchoolMunicipality = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MunicipalityCode = table.Column<double>(type: "float", nullable: true),
                    HeadOrganizationType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HeadOrganizationName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HeadOrganizationNumber = table.Column<double>(type: "float", nullable: true),
                    SubTest = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TestCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalParticipation = table.Column<double>(type: "float", nullable: true),
                    FemaleParticipation = table.Column<double>(type: "float", nullable: true),
                    MaleParticipation = table.Column<double>(type: "float", nullable: true),
                    TotalGradeAF = table.Column<double>(type: "float", nullable: true),
                    FemaleGradeAF = table.Column<double>(type: "float", nullable: true),
                    MaleGradeAF = table.Column<double>(type: "float", nullable: true),
                    TotalGradeAE = table.Column<double>(type: "float", nullable: true),
                    FemaleGradeAE = table.Column<double>(type: "float", nullable: true),
                    MaleGradeAE = table.Column<double>(type: "float", nullable: true),
                    GradePoints = table.Column<double>(type: "float", nullable: true),
                    FemaleGradePoints = table.Column<double>(type: "float", nullable: true),
                    MaleGradePoints = table.Column<double>(type: "float", nullable: true),
                    KommunId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolResultsGradeNine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolResultsGradeNine_Kommuner_KommunId",
                        column: x => x.KommunId,
                        principalTable: "Kommuner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SchoolResultsGradeSix",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EducationLevel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SchoolName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SchoolUnitCode = table.Column<int>(type: "int", nullable: false),
                    SchoolMunicipality = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MunicipalityCode = table.Column<double>(type: "float", nullable: true),
                    HeadOrganizationType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HeadOrganizationName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HeadOrganizationNumber = table.Column<double>(type: "float", nullable: true),
                    SubTest = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TestCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalParticipation = table.Column<double>(type: "float", nullable: true),
                    FemaleParticipation = table.Column<double>(type: "float", nullable: true),
                    MaleParticipation = table.Column<double>(type: "float", nullable: true),
                    TotalGradeAF = table.Column<double>(type: "float", nullable: true),
                    FemaleGradeAF = table.Column<double>(type: "float", nullable: true),
                    MaleGradeAF = table.Column<double>(type: "float", nullable: true),
                    TotalGradeAE = table.Column<double>(type: "float", nullable: true),
                    FemaleGradeAE = table.Column<double>(type: "float", nullable: true),
                    MaleGradeAE = table.Column<double>(type: "float", nullable: true),
                    GradePoints = table.Column<double>(type: "float", nullable: true),
                    FemaleGradePoints = table.Column<double>(type: "float", nullable: true),
                    MaleGradePoints = table.Column<double>(type: "float", nullable: true),
                    KommunId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolResultsGradeSix", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolResultsGradeSix_Kommuner_KommunId",
                        column: x => x.KommunId,
                        principalTable: "Kommuner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RealEstateData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PropertyType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BuildYear = table.Column<int>(type: "int", nullable: true),
                    OwnershipType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HousingForm = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LivingArea = table.Column<double>(type: "float", nullable: true),
                    LandArea = table.Column<double>(type: "float", nullable: true),
                    County = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Area = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WantedPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true),
                    Fee = table.Column<int>(type: "int", nullable: true),
                    OperatingCost = table.Column<int>(type: "int", nullable: true),
                    Rooms = table.Column<int>(type: "int", nullable: true),
                    Balcony = table.Column<bool>(type: "bit", nullable: true),
                    Association = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Broker = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SoldAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PriceChangeInfoId = table.Column<int>(type: "int", nullable: true),
                    Story = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KommunDataId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RealEstateData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RealEstateData_Kommuner_KommunDataId",
                        column: x => x.KommunDataId,
                        principalTable: "Kommuner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RealEstateData_PriceChangeInfo_PriceChangeInfoId",
                        column: x => x.PriceChangeInfoId,
                        principalTable: "PriceChangeInfo",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ScbValues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IncomeComponent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sex = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Year = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MiddleValue = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    EducationDataId = table.Column<int>(type: "int", nullable: true),
                    KommunDataId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScbValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScbValues_EducationLevelData_EducationDataId",
                        column: x => x.EducationDataId,
                        principalTable: "EducationLevelData",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ScbValues_Kommuner_KommunDataId",
                        column: x => x.KommunDataId,
                        principalTable: "Kommuner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lng = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PoliceEventId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Location_PoliceEvents_PoliceEventId",
                        column: x => x.PoliceEventId,
                        principalTable: "PoliceEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EducationLevelData_KommunDataId",
                table: "EducationLevelData",
                column: "KommunDataId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LifeExpectancyData_KommunDataId",
                table: "LifeExpectancyData",
                column: "KommunDataId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Location_PoliceEventId",
                table: "Location",
                column: "PoliceEventId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PoliceEvents_KommunDataId",
                table: "PoliceEvents",
                column: "KommunDataId");

            migrationBuilder.CreateIndex(
                name: "IX_PoliceEventSummary_KommunDataId",
                table: "PoliceEventSummary",
                column: "KommunDataId");

            migrationBuilder.CreateIndex(
                name: "IX_RealEstateData_KommunDataId",
                table: "RealEstateData",
                column: "KommunDataId");

            migrationBuilder.CreateIndex(
                name: "IX_RealEstateData_PriceChangeInfoId",
                table: "RealEstateData",
                column: "PriceChangeInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_RealEstateYearlySummary_KommunDataId",
                table: "RealEstateYearlySummary",
                column: "KommunDataId");

            migrationBuilder.CreateIndex(
                name: "IX_ScbValues_EducationDataId",
                table: "ScbValues",
                column: "EducationDataId");

            migrationBuilder.CreateIndex(
                name: "IX_ScbValues_KommunDataId",
                table: "ScbValues",
                column: "KommunDataId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SchoolResultsGradeNine_KommunId",
                table: "SchoolResultsGradeNine",
                column: "KommunId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolResultsGradeSix_KommunId",
                table: "SchoolResultsGradeSix",
                column: "KommunId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LifeExpectancyData");

            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.DropTable(
                name: "PoliceEventSummary");

            migrationBuilder.DropTable(
                name: "RealEstateData");

            migrationBuilder.DropTable(
                name: "RealEstateYearlySummary");

            migrationBuilder.DropTable(
                name: "ScbValues");

            migrationBuilder.DropTable(
                name: "SchoolResultsGradeNine");

            migrationBuilder.DropTable(
                name: "SchoolResultsGradeSix");

            migrationBuilder.DropTable(
                name: "PoliceEvents");

            migrationBuilder.DropTable(
                name: "PriceChangeInfo");

            migrationBuilder.DropTable(
                name: "EducationLevelData");

            migrationBuilder.DropTable(
                name: "Kommuner");
        }
    }
}
