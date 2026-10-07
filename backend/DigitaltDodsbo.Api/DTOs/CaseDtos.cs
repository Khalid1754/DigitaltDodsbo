using DigitaltDodsbo.Core.Models;

namespace DigitaltDodsbo.Api.DTOs;

public record CreateCaseRequest(
    string DeceasedName,
    string DeceasedPersonnummer,
    DateOnly DateOfDeath,
    string RelativeName,
    string RelativeRelation,
    string RelativeContact,
    List<int> ProviderIds
);

public record CaseItemResponse(
    int Id,
    int ProviderId,
    string ProviderName,
    ItemStatus Status,
    string? GeneratedLetter
);

public record CaseResponse(
    int Id,
    string DeceasedName,
    string RelativeName,
    DateTime CreatedAt,
    List<CaseItemResponse> Items
);
