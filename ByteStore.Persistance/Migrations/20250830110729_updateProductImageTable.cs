using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ByteStore.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class updateProductImageTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "ProductImages");

            migrationBuilder.AddColumn<string>(
                name: "FileId",
                table: "ProductImages",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileId",
                table: "ProductImages");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "ProductImages",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }
    }
}
