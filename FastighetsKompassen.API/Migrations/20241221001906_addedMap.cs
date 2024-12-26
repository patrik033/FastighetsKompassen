using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FastighetsKompassen.API.Migrations
{
    /// <inheritdoc />
    public partial class addedMap : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Map",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Map", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MapGeometry",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Coordinates = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapGeometry", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MapTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ref = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameSv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Population = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Boundary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Wikidata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Wikipedia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RefScb = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapTags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MapProperties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OsmId = table.Column<int>(type: "int", nullable: false),
                    TagsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MapProperties_MapTags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "MapTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MapFeatures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GeometryId = table.Column<int>(type: "int", nullable: false),
                    PropertiesId = table.Column<int>(type: "int", nullable: false),
                    MapRootId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapFeatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MapFeatures_MapGeometry_GeometryId",
                        column: x => x.GeometryId,
                        principalTable: "MapGeometry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MapFeatures_MapProperties_PropertiesId",
                        column: x => x.PropertiesId,
                        principalTable: "MapProperties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MapFeatures_Map_MapRootId",
                        column: x => x.MapRootId,
                        principalTable: "Map",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MapFeatures_GeometryId",
                table: "MapFeatures",
                column: "GeometryId");

            migrationBuilder.CreateIndex(
                name: "IX_MapFeatures_MapRootId",
                table: "MapFeatures",
                column: "MapRootId");

            migrationBuilder.CreateIndex(
                name: "IX_MapFeatures_PropertiesId",
                table: "MapFeatures",
                column: "PropertiesId");

            migrationBuilder.CreateIndex(
                name: "IX_MapProperties_TagsId",
                table: "MapProperties",
                column: "TagsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MapFeatures");

            migrationBuilder.DropTable(
                name: "MapGeometry");

            migrationBuilder.DropTable(
                name: "MapProperties");

            migrationBuilder.DropTable(
                name: "Map");

            migrationBuilder.DropTable(
                name: "MapTags");
        }
    }
}
