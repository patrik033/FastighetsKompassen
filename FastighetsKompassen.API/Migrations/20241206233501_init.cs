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
                name: "AverageLifeTime",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaleValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FemaleValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    YearSpan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KommunDataId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AverageLifeTime", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AverageLifeTime_Kommuner_KommunDataId",
                        column: x => x.KommunDataId,
                        principalTable: "Kommuner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AverageMiddleAge",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Male = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Female = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    KommunDataId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AverageMiddleAge", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AverageMiddleAge_Kommuner_KommunDataId",
                        column: x => x.KommunDataId,
                        principalTable: "Kommuner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EducationLevels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Year = table.Column<int>(type: "int", nullable: false),
                    PreGymnasial = table.Column<int>(type: "int", nullable: true),
                    Gymnasial = table.Column<int>(type: "int", nullable: true),
                    PostGymnasialUnder3Years = table.Column<int>(type: "int", nullable: true),
                    PostGymnasial3YearsOrMore = table.Column<int>(type: "int", nullable: true),
                    MissingInfo = table.Column<int>(type: "int", nullable: true),
                    KommunDataId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationLevels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EducationLevels_Kommuner_KommunDataId",
                        column: x => x.KommunDataId,
                        principalTable: "Kommuner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Income",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IncomeComponent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sex = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    MiddleValue = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    KommunDataId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Income", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Income_Kommuner_KommunDataId",
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
                    Datetime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    Year = table.Column<int>(type: "int", nullable: true),
                    EventType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventCount = table.Column<int>(type: "int", nullable: true),
                    KommunId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoliceEventSummary", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PoliceEventSummary_Kommuner_KommunId",
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
                    KommunId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RealEstateYearlySummary", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RealEstateYearlySummary_Kommuner_KommunId",
                        column: x => x.KommunId,
                        principalTable: "Kommuner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SchoolResultsGradeNine",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EducationLevel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
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
                    StartYear = table.Column<int>(type: "int", nullable: false),
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
                name: "Location",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lng = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PoliceEventId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Location_PoliceEvents_PoliceEventId",
                        column: x => x.PoliceEventId,
                        principalTable: "PoliceEvents",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PriceChangeInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Plus = table.Column<bool>(type: "bit", nullable: false),
                    Minus = table.Column<bool>(type: "bit", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    RealEstateDataId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceChangeInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PriceChangeInfo_RealEstateData_RealEstateDataId",
                        column: x => x.RealEstateDataId,
                        principalTable: "RealEstateData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AverageLifeTime_KommunDataId",
                table: "AverageLifeTime",
                column: "KommunDataId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AverageMiddleAge_KommunDataId",
                table: "AverageMiddleAge",
                column: "KommunDataId");

            migrationBuilder.CreateIndex(
                name: "IX_EducationLevels_KommunDataId",
                table: "EducationLevels",
                column: "KommunDataId");

            migrationBuilder.CreateIndex(
                name: "IX_Income_KommunDataId",
                table: "Income",
                column: "KommunDataId");

            migrationBuilder.CreateIndex(
                name: "IX_Location_PoliceEventId",
                table: "Location",
                column: "PoliceEventId",
                unique: true,
                filter: "[PoliceEventId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PoliceEvents_KommunDataId",
                table: "PoliceEvents",
                column: "KommunDataId");

            migrationBuilder.CreateIndex(
                name: "IX_PoliceEventSummary_KommunId",
                table: "PoliceEventSummary",
                column: "KommunId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceChangeInfo_RealEstateDataId",
                table: "PriceChangeInfo",
                column: "RealEstateDataId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RealEstateData_KommunDataId",
                table: "RealEstateData",
                column: "KommunDataId");

            migrationBuilder.CreateIndex(
                name: "IX_RealEstateYearlySummary_KommunId",
                table: "RealEstateYearlySummary",
                column: "KommunId");

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
                name: "AverageLifeTime");

            migrationBuilder.DropTable(
                name: "AverageMiddleAge");

            migrationBuilder.DropTable(
                name: "EducationLevels");

            migrationBuilder.DropTable(
                name: "Income");

            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.DropTable(
                name: "PoliceEventSummary");

            migrationBuilder.DropTable(
                name: "PriceChangeInfo");

            migrationBuilder.DropTable(
                name: "RealEstateYearlySummary");

            migrationBuilder.DropTable(
                name: "SchoolResultsGradeNine");

            migrationBuilder.DropTable(
                name: "SchoolResultsGradeSix");

            migrationBuilder.DropTable(
                name: "PoliceEvents");

            migrationBuilder.DropTable(
                name: "RealEstateData");

            migrationBuilder.DropTable(
                name: "Kommuner");
        }
    }
}
