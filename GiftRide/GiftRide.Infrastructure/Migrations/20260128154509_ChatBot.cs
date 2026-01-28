using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GiftRide.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChatBot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FaqEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Keywords = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FaqEntries", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "FaqEntries",
                columns: new[] { "Id", "Answer", "Keywords", "Question" },
                values: new object[,]
                {
                    { 1, "Всички наши ваучери са с валидност, избрана от Вас при покупката (3, 6 или 12 месеца). Можете да видите срока на самия ваучер.", "валидност, срок, време, изтича, дата", "Каква е валидността?" },
                    { 2, "Ваучерът се получава дигитално на имейла Ви веднага след успешна поръчка. Можете да го разпечатате или покажете от телефона.", "доставка, куриер, еконт, спиди, пристига, получа", "Как става доставката?" },
                    { 3, "Да, съгласно закона имате право на отказ в рамките на 14 дни, ако ваучерът все още не е използван/резервиран.", "върна, връщане, отказ, рекламация, пари", "Мога ли да върна ваучер?" },
                    { 4, "След като купите ваучер, влезте в списъка с поръчките си и използвайте бутона 'Резервация' срещу съответната поръчка.", "резерв, запаз, час, дата, използ", "Как да резервирам?" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FaqEntries");
        }
    }
}
