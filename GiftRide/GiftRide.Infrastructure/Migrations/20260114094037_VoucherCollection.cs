using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GiftRide.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class VoucherCollection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderId1",
                table: "Vouchers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vouchers_OrderId1",
                table: "Vouchers",
                column: "OrderId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Vouchers_Orders_OrderId1",
                table: "Vouchers",
                column: "OrderId1",
                principalTable: "Orders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vouchers_Orders_OrderId1",
                table: "Vouchers");

            migrationBuilder.DropIndex(
                name: "IX_Vouchers_OrderId1",
                table: "Vouchers");

            migrationBuilder.DropColumn(
                name: "OrderId1",
                table: "Vouchers");
        }
    }
}
