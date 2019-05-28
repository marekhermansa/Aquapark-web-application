using Microsoft.EntityFrameworkCore.Migrations;

namespace AquaparkApplication.Migrations
{
    public partial class SeedSocialClassDiscounts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "SocialClassDiscounts",
                columns: new[] { "Id", "SocialClassName", "Value" },
                values: new object[,]
                {
                    { 1, "Emeryt 50%", 0.50m },
                    { 2, "Student 80%", 0.20m },
                    { 3, "Weteran 25%", 0.75m },
                    { 4, "Dziecko 10%", 0.90m },
                    { 5, "Normalny 100%", 0.00m }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SocialClassDiscounts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "SocialClassDiscounts",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "SocialClassDiscounts",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "SocialClassDiscounts",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "SocialClassDiscounts",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
