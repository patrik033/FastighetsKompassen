using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FastighetsKompassen.API.Migrations
{
    /// <inheritdoc />
    public partial class updatess : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EducationLevelData_Kommuner_KommunId",
                table: "EducationLevelData");

            migrationBuilder.DropForeignKey(
                name: "FK_Location_PoliceEvents_PoliceEventId",
                table: "Location");

            migrationBuilder.DropForeignKey(
                name: "FK_ScbValues_EducationLevelData_EducationDataId",
                table: "ScbValues");

            migrationBuilder.DropIndex(
                name: "IX_ScbValues_EducationDataId",
                table: "ScbValues");

            migrationBuilder.DropIndex(
                name: "IX_ScbValues_KommunDataId",
                table: "ScbValues");

            migrationBuilder.DropIndex(
                name: "IX_LifeExpectancyData_KommunDataId",
                table: "LifeExpectancyData");

            migrationBuilder.DropIndex(
                name: "IX_EducationLevelData_KommunDataId",
                table: "EducationLevelData");

            migrationBuilder.DropIndex(
                name: "IX_EducationLevelData_KommunId",
                table: "EducationLevelData");

            migrationBuilder.DropColumn(
                name: "EducationDataId",
                table: "ScbValues");

            migrationBuilder.DropColumn(
                name: "KommunId",
                table: "EducationLevelData");

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "LifeExpectancyData",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ScbValues_KommunDataId",
                table: "ScbValues",
                column: "KommunDataId");

            migrationBuilder.CreateIndex(
                name: "IX_LifeExpectancyData_KommunDataId",
                table: "LifeExpectancyData",
                column: "KommunDataId");

            migrationBuilder.CreateIndex(
                name: "IX_EducationLevelData_KommunDataId",
                table: "EducationLevelData",
                column: "KommunDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_Location_PoliceEvents_PoliceEventId",
                table: "Location",
                column: "PoliceEventId",
                principalTable: "PoliceEvents",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Location_PoliceEvents_PoliceEventId",
                table: "Location");

            migrationBuilder.DropIndex(
                name: "IX_ScbValues_KommunDataId",
                table: "ScbValues");

            migrationBuilder.DropIndex(
                name: "IX_LifeExpectancyData_KommunDataId",
                table: "LifeExpectancyData");

            migrationBuilder.DropIndex(
                name: "IX_EducationLevelData_KommunDataId",
                table: "EducationLevelData");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "LifeExpectancyData");

            migrationBuilder.AddColumn<int>(
                name: "EducationDataId",
                table: "ScbValues",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "KommunId",
                table: "EducationLevelData",
                type: "int",
                nullable: true);

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
                name: "IX_LifeExpectancyData_KommunDataId",
                table: "LifeExpectancyData",
                column: "KommunDataId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EducationLevelData_KommunDataId",
                table: "EducationLevelData",
                column: "KommunDataId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EducationLevelData_KommunId",
                table: "EducationLevelData",
                column: "KommunId");

            migrationBuilder.AddForeignKey(
                name: "FK_EducationLevelData_Kommuner_KommunId",
                table: "EducationLevelData",
                column: "KommunId",
                principalTable: "Kommuner",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Location_PoliceEvents_PoliceEventId",
                table: "Location",
                column: "PoliceEventId",
                principalTable: "PoliceEvents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScbValues_EducationLevelData_EducationDataId",
                table: "ScbValues",
                column: "EducationDataId",
                principalTable: "EducationLevelData",
                principalColumn: "Id");
        }
    }
}
