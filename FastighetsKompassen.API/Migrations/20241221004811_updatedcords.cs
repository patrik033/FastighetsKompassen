using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FastighetsKompassen.API.Migrations
{
    /// <inheritdoc />
    public partial class updatedcords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Coordinates",
                table: "Geometries");

            migrationBuilder.CreateTable(
                name: "Coordinate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    GeometryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coordinate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Coordinate_Geometries_GeometryId",
                        column: x => x.GeometryId,
                        principalTable: "Geometries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Coordinate_GeometryId",
                table: "Coordinate",
                column: "GeometryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Coordinate");

            migrationBuilder.AddColumn<string>(
                name: "Coordinates",
                table: "Geometries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
