using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shop.Database.Migrations
{
    public partial class Stripe224 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stripe_Products_ProductId",
                table: "Stripe");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Stripe",
                table: "Stripe");

            migrationBuilder.RenameTable(
                name: "Stripe",
                newName: "Stripes");

            migrationBuilder.RenameIndex(
                name: "IX_Stripe_ProductId",
                table: "Stripes",
                newName: "IX_Stripes_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Stripes",
                table: "Stripes",
                column: "StripeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stripes_Products_ProductId",
                table: "Stripes",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stripes_Products_ProductId",
                table: "Stripes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Stripes",
                table: "Stripes");

            migrationBuilder.RenameTable(
                name: "Stripes",
                newName: "Stripe");

            migrationBuilder.RenameIndex(
                name: "IX_Stripes_ProductId",
                table: "Stripe",
                newName: "IX_Stripe_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Stripe",
                table: "Stripe",
                column: "StripeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stripe_Products_ProductId",
                table: "Stripe",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
