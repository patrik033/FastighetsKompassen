using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FastighetsKompassen.API.Migrations
{
    /// <inheritdoc />
    public partial class deletesas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RealEstateYearlySummary_Kommuner_KommunDataId",
                table: "RealEstateYearlySummary");

            migrationBuilder.DropIndex(
                name: "IX_RealEstateYearlySummary_KommunDataId",
                table: "RealEstateYearlySummary");

            migrationBuilder.DropColumn(
                name: "KommunDataId",
                table: "RealEstateYearlySummary");

            migrationBuilder.AddColumn<int>(
                name: "KommunId",
                table: "RealEstateYearlySummary",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RealEstateYearlySummary_KommunId",
                table: "RealEstateYearlySummary",
                column: "KommunId");

            migrationBuilder.AddForeignKey(
                name: "FK_RealEstateYearlySummary_Kommuner_KommunId",
                table: "RealEstateYearlySummary",
                column: "KommunId",
                principalTable: "Kommuner",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RealEstateYearlySummary_Kommuner_KommunId",
                table: "RealEstateYearlySummary");

            migrationBuilder.DropIndex(
                name: "IX_RealEstateYearlySummary_KommunId",
                table: "RealEstateYearlySummary");

            migrationBuilder.DropColumn(
                name: "KommunId",
                table: "RealEstateYearlySummary");

            migrationBuilder.AddColumn<int>(
                name: "KommunDataId",
                table: "RealEstateYearlySummary",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RealEstateYearlySummary_KommunDataId",
                table: "RealEstateYearlySummary",
                column: "KommunDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_RealEstateYearlySummary_Kommuner_KommunDataId",
                table: "RealEstateYearlySummary",
                column: "KommunDataId",
                principalTable: "Kommuner",
                principalColumn: "Id");
        }
    }
}
