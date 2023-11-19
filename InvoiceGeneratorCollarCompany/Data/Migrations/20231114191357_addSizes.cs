using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InvoiceGenerator.Data.Migrations
{
    /// <inheritdoc />
    public partial class addSizes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Size_SizeId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_SizeId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Size",
                table: "Size");

            migrationBuilder.DropColumn(
                name: "SizeId",
                table: "Products");

            migrationBuilder.RenameTable(
                name: "Size",
                newName: "Sizes");

            migrationBuilder.RenameColumn(
                name: "Number",
                table: "Sizes",
                newName: "ProductId");

            migrationBuilder.AddColumn<int>(
                name: "NameInt",
                table: "Sizes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Namestring",
                table: "Sizes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sizes",
                table: "Sizes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Sizes_ProductId",
                table: "Sizes",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sizes_Products_ProductId",
                table: "Sizes",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sizes_Products_ProductId",
                table: "Sizes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sizes",
                table: "Sizes");

            migrationBuilder.DropIndex(
                name: "IX_Sizes_ProductId",
                table: "Sizes");

            migrationBuilder.DropColumn(
                name: "NameInt",
                table: "Sizes");

            migrationBuilder.DropColumn(
                name: "Namestring",
                table: "Sizes");

            migrationBuilder.RenameTable(
                name: "Sizes",
                newName: "Size");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "Size",
                newName: "Number");

            migrationBuilder.AddColumn<int>(
                name: "SizeId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Size",
                table: "Size",
                column: "Id");

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
    }
}
