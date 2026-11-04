using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Assortment.Migrations
{
    /// <inheritdoc />
    public partial class AddCounterpartiesAndFactories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Counterparties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Inn = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RegistrationCountry = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LegalAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    BankAccount = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BankName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Swift = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ContactPerson = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Counterparties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Factories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Specialty = table.Column<int>(type: "int", nullable: false),
                    Ownership = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ContactPerson = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Factories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Factories_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CounterpartyFactories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CounterpartyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FactoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CounterpartyFactories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CounterpartyFactories_Counterparties_CounterpartyId",
                        column: x => x.CounterpartyId,
                        principalTable: "Counterparties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CounterpartyFactories_Factories_FactoryId",
                        column: x => x.FactoryId,
                        principalTable: "Factories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FactoryImporterBeneficiaries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FactoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImporterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BeneficiaryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FactoryImporterBeneficiaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FactoryImporterBeneficiaries_Counterparties_BeneficiaryId",
                        column: x => x.BeneficiaryId,
                        principalTable: "Counterparties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FactoryImporterBeneficiaries_Counterparties_ImporterId",
                        column: x => x.ImporterId,
                        principalTable: "Counterparties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FactoryImporterBeneficiaries_Factories_FactoryId",
                        column: x => x.FactoryId,
                        principalTable: "Factories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Counterparties_Inn",
                table: "Counterparties",
                column: "Inn",
                filter: "[Inn] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Counterparties_IsActive",
                table: "Counterparties",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Counterparties_Name",
                table: "Counterparties",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_CounterpartyFactories_CounterpartyId_FactoryId_Role",
                table: "CounterpartyFactories",
                columns: new[] { "CounterpartyId", "FactoryId", "Role" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CounterpartyFactories_FactoryId_Role_IsActive",
                table: "CounterpartyFactories",
                columns: new[] { "FactoryId", "Role", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_Factories_CityId",
                table: "Factories",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Factories_Code",
                table: "Factories",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Factories_Specialty_IsActive",
                table: "Factories",
                columns: new[] { "Specialty", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_FactoryImporterBeneficiaries_BeneficiaryId_IsActive",
                table: "FactoryImporterBeneficiaries",
                columns: new[] { "BeneficiaryId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_FactoryImporterBeneficiaries_FactoryId_ImporterId",
                table: "FactoryImporterBeneficiaries",
                columns: new[] { "FactoryId", "ImporterId" },
                unique: true,
                filter: "[IsDefault] = 1 AND [IsActive] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_FactoryImporterBeneficiaries_ImporterId",
                table: "FactoryImporterBeneficiaries",
                column: "ImporterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CounterpartyFactories");

            migrationBuilder.DropTable(
                name: "FactoryImporterBeneficiaries");

            migrationBuilder.DropTable(
                name: "Counterparties");

            migrationBuilder.DropTable(
                name: "Factories");
        }
    }
}
