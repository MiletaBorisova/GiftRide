using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GiftRide.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChatBotUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "FaqEntries",
                columns: new[] { "Id", "Answer", "Keywords", "Question" },
                values: new object[,]
                {
                    { 5, "За да направите поръчка и да имате достъп до историята на Вашите ваучери и резервации, е необходимо да си създадете профил в GiftRide.", "регистрация, профил, акаунт, вход, login, register", "Нужна ли е регистрация?" },
                    { 6, "След успешна покупка, всички Ваши ваучери се съхраняват в секция 'Моите поръчки' във Вашия профил, откъдето можете да ги разгледате по всяко време.", "къде, намират, ваучерите, поръчките, orders, история, изтегля", "Къде са ми ваучерите?" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "FaqEntries",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "FaqEntries",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}
