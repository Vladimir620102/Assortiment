using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Assortment.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Seasons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Kind = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Translate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ExternalCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LabelName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ExpireDate = table.Column<DateOnly>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seasons", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Seasons",
                columns: new[] { "Id", "CreatedAt", "ExpireDate", "ExternalCode", "IsActive", "Kind", "LabelName", "Name", "Translate", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("382d486a-e64b-40bd-a8e5-04d21c1616fb"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "1", true, 2, "ВЕСНА-ЛЕТО", "Весна-Лето", "Spring-Summer", null },
                    { new Guid("417fceb4-59c6-4868-89fb-039031e6b781"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "2", true, 3, "ОСЕНЬ-ЗИМА", "Осень-Зима", "Autumn-Winter", null },
                    { new Guid("599cc2a3-8adb-4458-9d7e-d0ed2b593c0f"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "0", true, 1, "ДЕМИ", "Демисезон", "Demi-season", null },
                    { new Guid("7a230551-5f5b-47b0-a36d-1bbad8010d85"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "3", true, 4, "КРУГЛОСЕЗОН", "Круглосезонная", "All seasons", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Seasons_ExternalCode",
                table: "Seasons",
                column: "ExternalCode",
                unique: true,
                filter: "[ExternalCode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Seasons_Kind_IsActive",
                table: "Seasons",
                columns: new[] { "Kind", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_Seasons_Kind_Name",
                table: "Seasons",
                columns: new[] { "Kind", "Name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Seasons");
        }
    }
}
