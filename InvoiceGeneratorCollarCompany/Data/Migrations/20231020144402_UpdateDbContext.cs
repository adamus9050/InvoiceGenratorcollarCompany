using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InvoiceGenerator.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Size_SizeId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Type_TypeId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Type",
                table: "Type");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Size",
                table: "Size");

            migrationBuilder.RenameTable(
                name: "Type",
                newName: "Types");

            migrationBuilder.RenameTable(
                name: "Size",
                newName: "Sizes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Types",
                table: "Types",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sizes",
                table: "Sizes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Sizes_SizeId",
                table: "Products",
                column: "SizeId",
                principalTable: "Sizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Types_TypeId",
                table: "Products",
                column: "TypeId",
                principalTable: "Types",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Sizes_SizeId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Types_TypeId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Types",
                table: "Types");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sizes",
                table: "Sizes");

            migrationBuilder.RenameTable(
                name: "Types",
                newName: "Type");

            migrationBuilder.RenameTable(
                name: "Sizes",
                newName: "Size");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Type",
                table: "Type",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Size",
                table: "Size",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Size_SizeId",
                table: "Products",
                column: "SizeId",
                principalTable: "Size",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Type_TypeId",
                table: "Products",
                column: "TypeId",
                principalTable: "Type",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
