using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FastighetsKompassen.API.Migrations
{
    /// <inheritdoc />
    public partial class addedLifeTimeExpectancy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LifeExpectancyData");

            migrationBuilder.CreateTable(
                name: "AverageAgeExpectancy",
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
                    table.PrimaryKey("PK_AverageAgeExpectancy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AverageAgeExpectancy_Kommuner_KommunDataId",
                        column: x => x.KommunDataId,
                        principalTable: "Kommuner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LifeTimeExpectedData",
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
                    table.PrimaryKey("PK_LifeTimeExpectedData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LifeTimeExpectedData_Kommuner_KommunDataId",
                        column: x => x.KommunDataId,
                        principalTable: "Kommuner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AverageAgeExpectancy_KommunDataId",
                table: "AverageAgeExpectancy",
                column: "KommunDataId");

            migrationBuilder.CreateIndex(
                name: "IX_LifeTimeExpectedData_KommunDataId",
                table: "LifeTimeExpectedData",
                column: "KommunDataId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AverageAgeExpectancy");

            migrationBuilder.DropTable(
                name: "LifeTimeExpectedData");

            migrationBuilder.CreateTable(
                name: "LifeExpectancyData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KommunDataId = table.Column<int>(type: "int", nullable: false),
                    Female = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Male = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_LifeExpectancyData_KommunDataId",
                table: "LifeExpectancyData",
                column: "KommunDataId");
        }
    }
}
