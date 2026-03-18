using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GiftRide.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddContactEntityUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Еmail",
                table: "Contacts",
                newName: "Email");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Contacts",
                newName: "Еmail");
        }
    }
}
