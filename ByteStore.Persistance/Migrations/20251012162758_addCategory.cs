using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ByteStore.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class addCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Categories_ParentCategoryId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_ParentCategoryId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "ParentCategoryId",
                table: "Categories");

            migrationBuilder.AddColumn<int>(
                name: "CategoryTreeId",
                table: "Products",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CategoryTrees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ParentCategoryId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryTrees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoryTrees_CategoryTrees_ParentCategoryId",
                        column: x => x.ParentCategoryId,
                        principalTable: "CategoryTrees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryTreeId",
                table: "Products",
                column: "CategoryTreeId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryTrees_ParentCategoryId",
                table: "CategoryTrees",
                column: "ParentCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_CategoryTrees_CategoryTreeId",
                table: "Products",
                column: "CategoryTreeId",
                principalTable: "CategoryTrees",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_CategoryTrees_CategoryTreeId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "CategoryTrees");

            migrationBuilder.DropIndex(
                name: "IX_Products_CategoryTreeId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CategoryTreeId",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "ParentCategoryId",
                table: "Categories",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ParentCategoryId",
                table: "Categories",
                column: "ParentCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Categories_ParentCategoryId",
                table: "Categories",
                column: "ParentCategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
