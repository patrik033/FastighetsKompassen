using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FastighetsKompassen.API.Migrations
{
    /// <inheritdoc />
    public partial class policecascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Location_PoliceEvents_PoliceEventId",
                table: "Location");

            migrationBuilder.AddForeignKey(
                name: "FK_Location_PoliceEvents_PoliceEventId",
                table: "Location",
                column: "PoliceEventId",
                principalTable: "PoliceEvents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Location_PoliceEvents_PoliceEventId",
                table: "Location");

            migrationBuilder.AddForeignKey(
                name: "FK_Location_PoliceEvents_PoliceEventId",
                table: "Location",
                column: "PoliceEventId",
                principalTable: "PoliceEvents",
                principalColumn: "Id");
        }
    }
}
