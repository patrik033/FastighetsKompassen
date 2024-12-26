using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FastighetsKompassen.API.Migrations
{
    /// <inheritdoc />
    public partial class updetessdas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MapFeatures_MapGeometry_GeometryId",
                table: "MapFeatures");

            migrationBuilder.DropForeignKey(
                name: "FK_MapFeatures_MapProperties_PropertiesId",
                table: "MapFeatures");

            migrationBuilder.DropForeignKey(
                name: "FK_MapFeatures_Map_MapRootId",
                table: "MapFeatures");

            migrationBuilder.DropForeignKey(
                name: "FK_MapProperties_MapTags_TagsId",
                table: "MapProperties");

            migrationBuilder.DropTable(
                name: "Map");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MapTags",
                table: "MapTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MapProperties",
                table: "MapProperties");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MapGeometry",
                table: "MapGeometry");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MapFeatures",
                table: "MapFeatures");

            migrationBuilder.DropIndex(
                name: "IX_MapFeatures_MapRootId",
                table: "MapFeatures");

            migrationBuilder.DropColumn(
                name: "MapRootId",
                table: "MapFeatures");

            migrationBuilder.RenameTable(
                name: "MapTags",
                newName: "Tags");

            migrationBuilder.RenameTable(
                name: "MapProperties",
                newName: "Properties");

            migrationBuilder.RenameTable(
                name: "MapGeometry",
                newName: "Geometries");

            migrationBuilder.RenameTable(
                name: "MapFeatures",
                newName: "Features");

            migrationBuilder.RenameIndex(
                name: "IX_MapProperties_TagsId",
                table: "Properties",
                newName: "IX_Properties_TagsId");

            migrationBuilder.RenameIndex(
                name: "IX_MapFeatures_PropertiesId",
                table: "Features",
                newName: "IX_Features_PropertiesId");

            migrationBuilder.RenameIndex(
                name: "IX_MapFeatures_GeometryId",
                table: "Features",
                newName: "IX_Features_GeometryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tags",
                table: "Tags",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Properties",
                table: "Properties",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Geometries",
                table: "Geometries",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Features",
                table: "Features",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Features_Geometries_GeometryId",
                table: "Features",
                column: "GeometryId",
                principalTable: "Geometries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Features_Properties_PropertiesId",
                table: "Features",
                column: "PropertiesId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_Tags_TagsId",
                table: "Properties",
                column: "TagsId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Features_Geometries_GeometryId",
                table: "Features");

            migrationBuilder.DropForeignKey(
                name: "FK_Features_Properties_PropertiesId",
                table: "Features");

            migrationBuilder.DropForeignKey(
                name: "FK_Properties_Tags_TagsId",
                table: "Properties");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tags",
                table: "Tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Properties",
                table: "Properties");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Geometries",
                table: "Geometries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Features",
                table: "Features");

            migrationBuilder.RenameTable(
                name: "Tags",
                newName: "MapTags");

            migrationBuilder.RenameTable(
                name: "Properties",
                newName: "MapProperties");

            migrationBuilder.RenameTable(
                name: "Geometries",
                newName: "MapGeometry");

            migrationBuilder.RenameTable(
                name: "Features",
                newName: "MapFeatures");

            migrationBuilder.RenameIndex(
                name: "IX_Properties_TagsId",
                table: "MapProperties",
                newName: "IX_MapProperties_TagsId");

            migrationBuilder.RenameIndex(
                name: "IX_Features_PropertiesId",
                table: "MapFeatures",
                newName: "IX_MapFeatures_PropertiesId");

            migrationBuilder.RenameIndex(
                name: "IX_Features_GeometryId",
                table: "MapFeatures",
                newName: "IX_MapFeatures_GeometryId");

            migrationBuilder.AddColumn<int>(
                name: "MapRootId",
                table: "MapFeatures",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MapTags",
                table: "MapTags",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MapProperties",
                table: "MapProperties",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MapGeometry",
                table: "MapGeometry",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MapFeatures",
                table: "MapFeatures",
                column: "Id");

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

            migrationBuilder.CreateIndex(
                name: "IX_MapFeatures_MapRootId",
                table: "MapFeatures",
                column: "MapRootId");

            migrationBuilder.AddForeignKey(
                name: "FK_MapFeatures_MapGeometry_GeometryId",
                table: "MapFeatures",
                column: "GeometryId",
                principalTable: "MapGeometry",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MapFeatures_MapProperties_PropertiesId",
                table: "MapFeatures",
                column: "PropertiesId",
                principalTable: "MapProperties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MapFeatures_Map_MapRootId",
                table: "MapFeatures",
                column: "MapRootId",
                principalTable: "Map",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MapProperties_MapTags_TagsId",
                table: "MapProperties",
                column: "TagsId",
                principalTable: "MapTags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
