using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FastighetsKompassen.API.Migrations
{
    /// <inheritdoc />
    public partial class addedcscadeforpricechangeinfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RealEstateData_PriceChangeInfo_PriceChangeInfoId",
                table: "RealEstateData");

            migrationBuilder.DropIndex(
                name: "IX_RealEstateData_PriceChangeInfoId",
                table: "RealEstateData");

            migrationBuilder.DropColumn(
                name: "PriceChangeInfoId",
                table: "RealEstateData");

            migrationBuilder.AddColumn<int>(
                name: "RealEstateDataId",
                table: "PriceChangeInfo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PriceChangeInfo_RealEstateDataId",
                table: "PriceChangeInfo",
                column: "RealEstateDataId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PriceChangeInfo_RealEstateData_RealEstateDataId",
                table: "PriceChangeInfo",
                column: "RealEstateDataId",
                principalTable: "RealEstateData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PriceChangeInfo_RealEstateData_RealEstateDataId",
                table: "PriceChangeInfo");

            migrationBuilder.DropIndex(
                name: "IX_PriceChangeInfo_RealEstateDataId",
                table: "PriceChangeInfo");

            migrationBuilder.DropColumn(
                name: "RealEstateDataId",
                table: "PriceChangeInfo");

            migrationBuilder.AddColumn<int>(
                name: "PriceChangeInfoId",
                table: "RealEstateData",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RealEstateData_PriceChangeInfoId",
                table: "RealEstateData",
                column: "PriceChangeInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_RealEstateData_PriceChangeInfo_PriceChangeInfoId",
                table: "RealEstateData",
                column: "PriceChangeInfoId",
                principalTable: "PriceChangeInfo",
                principalColumn: "Id");
        }
    }
}
