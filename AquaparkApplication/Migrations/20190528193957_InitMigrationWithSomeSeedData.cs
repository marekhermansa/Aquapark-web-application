using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AquaparkApplication.Migrations
{
    public partial class InitMigrationWithSomeSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PeriodicDiscounts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StartTime = table.Column<DateTime>(nullable: false),
                    FinishTime = table.Column<DateTime>(nullable: false),
                    Value = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeriodicDiscounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SocialClassDiscounts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Value = table.Column<decimal>(nullable: false),
                    SocialClassName = table.Column<string>(maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialClassDiscounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(maxLength: 30, nullable: true),
                    Password = table.Column<string>(maxLength: 40, nullable: true),
                    UserGuid = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 30, nullable: true),
                    Surname = table.Column<string>(maxLength: 30, nullable: true),
                    IsAdmin = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsersData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(maxLength: 30, nullable: true),
                    Name = table.Column<string>(maxLength: 30, nullable: true),
                    Surname = table.Column<string>(maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Zones",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 30, nullable: true),
                    MaxAmountOfPeople = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateOfOrder = table.Column<DateTime>(nullable: false),
                    UserDataId = table.Column<int>(nullable: true),
                    UserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_UsersData_UserDataId",
                        column: x => x.UserDataId,
                        principalTable: "UsersData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Attractions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 30, nullable: true),
                    MaxAmountOfPeople = table.Column<int>(nullable: false),
                    ZoneId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attractions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attractions_Zones_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "Zones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    ZoneId = table.Column<int>(nullable: true),
                    PeriodicDiscountId = table.Column<int>(nullable: true),
                    StartHour = table.Column<double>(nullable: false),
                    EndHour = table.Column<double>(nullable: false),
                    Days = table.Column<int>(nullable: false),
                    Months = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_PeriodicDiscounts_PeriodicDiscountId",
                        column: x => x.PeriodicDiscountId,
                        principalTable: "PeriodicDiscounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tickets_Zones_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "Zones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Positions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TicketId = table.Column<int>(nullable: true),
                    PeriodicDiscountId = table.Column<int>(nullable: true),
                    SocialClassDiscountId = table.Column<int>(nullable: true),
                    CanBeUsed = table.Column<bool>(nullable: false),
                    OrderId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Positions_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Positions_PeriodicDiscounts_PeriodicDiscountId",
                        column: x => x.PeriodicDiscountId,
                        principalTable: "PeriodicDiscounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Positions_SocialClassDiscounts_SocialClassDiscountId",
                        column: x => x.SocialClassDiscountId,
                        principalTable: "SocialClassDiscounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Positions_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AttractionHistories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AttractionId = table.Column<int>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    FinishTime = table.Column<DateTime>(nullable: true),
                    PositionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttractionHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttractionHistories_Attractions_AttractionId",
                        column: x => x.AttractionId,
                        principalTable: "Attractions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttractionHistories_Positions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "Positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ZoneHistories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ZoneId = table.Column<int>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    FinishTime = table.Column<DateTime>(nullable: true),
                    PositionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZoneHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZoneHistories_Positions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "Positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ZoneHistories_Zones_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "Zones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Zones",
                columns: new[] { "Id", "MaxAmountOfPeople", "Name" },
                values: new object[] { 1, 35, "Strefa saun" });

            migrationBuilder.InsertData(
                table: "Zones",
                columns: new[] { "Id", "MaxAmountOfPeople", "Name" },
                values: new object[] { 2, 100, "Strefa basenów" });

            migrationBuilder.InsertData(
                table: "Zones",
                columns: new[] { "Id", "MaxAmountOfPeople", "Name" },
                values: new object[] { 3, 20, "Strefa spa" });

            migrationBuilder.InsertData(
                table: "Attractions",
                columns: new[] { "Id", "MaxAmountOfPeople", "Name", "ZoneId" },
                values: new object[,]
                {
                    { 1, 5, "Sauna 1", 1 },
                    { 2, 5, "Sauna 2", 1 },
                    { 3, 5, "Sauna 3", 1 },
                    { 4, 7, "Sauna 4", 1 },
                    { 5, 3, "Sauna 5", 1 },
                    { 6, 2, "Sauna 6", 1 },
                    { 7, 8, "Sauna 7", 1 },
                    { 8, 25, "Basen 1", 2 },
                    { 9, 25, "Basen 2", 2 },
                    { 10, 30, "Basen 3", 2 },
                    { 11, 20, "Basen 4", 2 },
                    { 12, 5, "Spa 1", 3 },
                    { 13, 5, "Spa 2", 3 },
                    { 14, 10, "Spa 3", 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttractionHistories_AttractionId",
                table: "AttractionHistories",
                column: "AttractionId");

            migrationBuilder.CreateIndex(
                name: "IX_AttractionHistories_PositionId",
                table: "AttractionHistories",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Attractions_ZoneId",
                table: "Attractions",
                column: "ZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserDataId",
                table: "Orders",
                column: "UserDataId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_OrderId",
                table: "Positions",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_PeriodicDiscountId",
                table: "Positions",
                column: "PeriodicDiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_SocialClassDiscountId",
                table: "Positions",
                column: "SocialClassDiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_TicketId",
                table: "Positions",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_PeriodicDiscountId",
                table: "Tickets",
                column: "PeriodicDiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ZoneId",
                table: "Tickets",
                column: "ZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_ZoneHistories_PositionId",
                table: "ZoneHistories",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_ZoneHistories_ZoneId",
                table: "ZoneHistories",
                column: "ZoneId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttractionHistories");

            migrationBuilder.DropTable(
                name: "ZoneHistories");

            migrationBuilder.DropTable(
                name: "Attractions");

            migrationBuilder.DropTable(
                name: "Positions");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "SocialClassDiscounts");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "UsersData");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "PeriodicDiscounts");

            migrationBuilder.DropTable(
                name: "Zones");
        }
    }
}
