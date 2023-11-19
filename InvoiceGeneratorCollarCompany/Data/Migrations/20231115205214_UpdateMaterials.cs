using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InvoiceGenerator.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMaterials : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Colour",
                table: "Materials",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageColour",
                table: "Materials",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Colour",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "ImageColour",
                table: "Materials");
        }
    }
}
