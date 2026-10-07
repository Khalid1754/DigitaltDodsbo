using DigitaltDodsbo.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DigitaltDodsbo.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Provider> Providers => Set<Provider>();
    public DbSet<EstateCase> EstateCases => Set<EstateCase>();
    public DbSet<CaseItem> CaseItems => Set<CaseItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CaseItem>()
            .HasOne(ci => ci.EstateCase)
            .WithMany(ec => ec.Items)
            .HasForeignKey(ci => ci.EstateCaseId);

        modelBuilder.Entity<CaseItem>()
            .HasOne(ci => ci.Provider)
            .WithMany()
            .HasForeignKey(ci => ci.ProviderId);

        // Seed-data: de vanligaste svenska tjänsterna
        modelBuilder.Entity<Provider>().HasData(
            new Provider { Id = 1, Name = "Telia", Category = "Telekom", RequiresDeathCertificate = true, RequiresEstateInventory = false, ContactMethod = "Brev eller kundtjänst", ContactDetails = "90 200", ProcessNotes = "Skicka kopia av dödsfallsintyg." },
            new Provider { Id = 2, Name = "Spotify", Category = "Streaming", RequiresDeathCertificate = false, RequiresEstateInventory = false, ContactMethod = "Supportformulär", ContactDetails = "support.spotify.com", ProcessNotes = "Ingen särskild dödsboprocess finns." },
            new Provider { Id = 3, Name = "Com Hem / Tele2", Category = "Telekom/Bredband", RequiresDeathCertificate = true, RequiresEstateInventory = false, ContactMethod = "Kundtjänst telefon", ContactDetails = "90 222", ProcessNotes = "Kräver personbevis som visar dödsfall." },
            new Provider { Id = 4, Name = "Klarna", Category = "Betalning/Kredit", RequiresDeathCertificate = true, RequiresEstateInventory = true, ContactMethod = "Kundtjänst + brev", ContactDetails = "klarna.com/se/kundservice", ProcessNotes = "Eventuella skulder hanteras av dödsboet." },
            new Provider { Id = 5, Name = "Swedbank", Category = "Bank", RequiresDeathCertificate = true, RequiresEstateInventory = true, ContactMethod = "Bokat möte på kontor", ContactDetails = "swedbank.se/dodsfall", ProcessNotes = "Konton spärras vid dödsfall. Bouppteckning krävs." },
            new Provider { Id = 6, Name = "If Försäkring", Category = "Försäkring", RequiresDeathCertificate = true, RequiresEstateInventory = false, ContactMethod = "Telefon + blankett", ContactDetails = "0771-43 00 00", ProcessNotes = "Kan ge rätt till utbetalning." },
            new Provider { Id = 7, Name = "Facebook/Meta", Category = "Sociala medier", RequiresDeathCertificate = true, RequiresEstateInventory = false, ContactMethod = "Onlineformulär", ContactDetails = "facebook.com/help", ProcessNotes = "Kan minnesbemärkas eller raderas." },
            new Provider { Id = 8, Name = "Netflix", Category = "Streaming", RequiresDeathCertificate = false, RequiresEstateInventory = false, ContactMethod = "Kundtjänst chatt", ContactDetails = "netflix.com/support", ProcessNotes = "Ingen formell dödsboprocess." }
        );
    }
}
