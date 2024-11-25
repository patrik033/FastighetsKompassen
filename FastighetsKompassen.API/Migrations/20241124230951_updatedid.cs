using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FastighetsKompassen.API.Migrations
{
    /// <inheritdoc />
    public partial class updatedid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PoliceEventSummary_Kommuner_KommunDataId",
                table: "PoliceEventSummary");

            migrationBuilder.DropIndex(
                name: "IX_PoliceEventSummary_KommunDataId",
                table: "PoliceEventSummary");

            migrationBuilder.DropColumn(
                name: "KommunDataId",
                table: "PoliceEventSummary");

            migrationBuilder.AddColumn<int>(
                name: "KommunId",
                table: "PoliceEventSummary",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PoliceEventSummary_KommunId",
                table: "PoliceEventSummary",
                column: "KommunId");

            migrationBuilder.AddForeignKey(
                name: "FK_PoliceEventSummary_Kommuner_KommunId",
                table: "PoliceEventSummary",
                column: "KommunId",
                principalTable: "Kommuner",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PoliceEventSummary_Kommuner_KommunId",
                table: "PoliceEventSummary");

            migrationBuilder.DropIndex(
                name: "IX_PoliceEventSummary_KommunId",
                table: "PoliceEventSummary");

            migrationBuilder.DropColumn(
                name: "KommunId",
                table: "PoliceEventSummary");

            migrationBuilder.AddColumn<int>(
                name: "KommunDataId",
                table: "PoliceEventSummary",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PoliceEventSummary_KommunDataId",
                table: "PoliceEventSummary",
                column: "KommunDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_PoliceEventSummary_Kommuner_KommunDataId",
                table: "PoliceEventSummary",
                column: "KommunDataId",
                principalTable: "Kommuner",
                principalColumn: "Id");
        }
    }
}
