using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DigitaltDodsbo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EstateCases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DeceasedName = table.Column<string>(type: "TEXT", nullable: false),
                    DeceasedPersonnummer = table.Column<string>(type: "TEXT", nullable: false),
                    DateOfDeath = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    RelativeName = table.Column<string>(type: "TEXT", nullable: false),
                    RelativeRelation = table.Column<string>(type: "TEXT", nullable: false),
                    RelativeContact = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstateCases", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Providers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    RequiresDeathCertificate = table.Column<bool>(type: "INTEGER", nullable: false),
                    RequiresEstateInventory = table.Column<bool>(type: "INTEGER", nullable: false),
                    ContactMethod = table.Column<string>(type: "TEXT", nullable: false),
                    ContactDetails = table.Column<string>(type: "TEXT", nullable: false),
                    ProcessNotes = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Providers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CaseItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EstateCaseId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProviderId = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    GeneratedLetter = table.Column<string>(type: "TEXT", nullable: true),
                    SentAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaseItems_EstateCases_EstateCaseId",
                        column: x => x.EstateCaseId,
                        principalTable: "EstateCases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CaseItems_Providers_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "Providers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Providers",
                columns: new[] { "Id", "Category", "ContactDetails", "ContactMethod", "Name", "ProcessNotes", "RequiresDeathCertificate", "RequiresEstateInventory" },
                values: new object[,]
                {
                    { 1, "Telekom", "90 200", "Brev eller kundtjänst", "Telia", "Skicka kopia av dödsfallsintyg.", true, false },
                    { 2, "Streaming", "support.spotify.com", "Supportformulär", "Spotify", "Ingen särskild dödsboprocess finns.", false, false },
                    { 3, "Telekom/Bredband", "90 222", "Kundtjänst telefon", "Com Hem / Tele2", "Kräver personbevis som visar dödsfall.", true, false },
                    { 4, "Betalning/Kredit", "klarna.com/se/kundservice", "Kundtjänst + brev", "Klarna", "Eventuella skulder hanteras av dödsboet.", true, true },
                    { 5, "Bank", "swedbank.se/dodsfall", "Bokat möte på kontor", "Swedbank", "Konton spärras vid dödsfall. Bouppteckning krävs.", true, true },
                    { 6, "Försäkring", "0771-43 00 00", "Telefon + blankett", "If Försäkring", "Kan ge rätt till utbetalning.", true, false },
                    { 7, "Sociala medier", "facebook.com/help", "Onlineformulär", "Facebook/Meta", "Kan minnesbemärkas eller raderas.", true, false },
                    { 8, "Streaming", "netflix.com/support", "Kundtjänst chatt", "Netflix", "Ingen formell dödsboprocess.", false, false }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CaseItems_EstateCaseId",
                table: "CaseItems",
                column: "EstateCaseId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseItems_ProviderId",
                table: "CaseItems",
                column: "ProviderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CaseItems");

            migrationBuilder.DropTable(
                name: "EstateCases");

            migrationBuilder.DropTable(
                name: "Providers");
        }
    }
}
