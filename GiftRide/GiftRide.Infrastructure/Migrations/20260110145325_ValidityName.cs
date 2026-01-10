using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GiftRide.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ValidityName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ValidityDuration",
                table: "Validities",
                newName: "ValidityName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ValidityName",
                table: "Validities",
                newName: "ValidityDuration");
        }
    }
}
