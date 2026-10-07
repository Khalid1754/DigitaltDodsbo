using DigitaltDodsbo.Core.Interfaces;
using DigitaltDodsbo.Core.Models;

namespace DigitaltDodsbo.Infrastructure.Services;

public class TemplateLetterGenerator : ILetterGenerator
{
    public Task<string> GenerateAsync(EstateCase estateCase, Provider provider)
    {
        var requirements = new List<string>();
        if (provider.RequiresDeathCertificate)
            requirements.Add("kopia av dödsfallsintyg/personbevis som visar dödsfallet");
        if (provider.RequiresEstateInventory)
            requirements.Add("bestyrkt kopia av bouppteckning");

        var requirementsText = requirements.Count > 0
            ? $"Bifogat finns: {string.Join(" samt ", requirements)}."
            : "Inga ytterligare dokument bedöms krävas.";

        var letter = $"""
            Till {provider.Name}

            Ärende: Avslut av avtal med anledning av dödsfall

            Jag heter {estateCase.RelativeName} och är {estateCase.RelativeRelation} till
            {estateCase.DeceasedName} (personnummer {estateCase.DeceasedPersonnummer}),
            som avled {estateCase.DateOfDeath:yyyy-MM-dd}.

            Jag kontaktar er för att avsluta avtal/konto kopplat till ovanstående person.

            {requirementsText}

            Kontaktväg: {provider.ContactMethod} ({provider.ContactDetails}).

            Med vänlig hälsning,
            {estateCase.RelativeName}
            {estateCase.RelativeContact}
            """;

        return Task.FromResult(letter);
    }
}
