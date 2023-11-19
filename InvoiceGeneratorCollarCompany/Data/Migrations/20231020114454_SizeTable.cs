using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InvoiceGenerator.Data.Migrations
{
    /// <inheritdoc />
    public partial class SizeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Size",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "SizeId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Size",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Size1 = table.Column<int>(type: "int", nullable: false),
                    Size2 = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Size", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_SizeId",
                table: "Products",
                column: "SizeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Size_SizeId",
                table: "Products",
                column: "SizeId",
                principalTable: "Size",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Size_SizeId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "Size");

            migrationBuilder.DropIndex(
                name: "IX_Products_SizeId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SizeId",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
