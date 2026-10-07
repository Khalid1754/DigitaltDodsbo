namespace DigitaltDodsbo.Core.Models;

public enum ItemStatus
{
    Pending,      // brev ej genererat/skickat
    LetterReady,  // brev genererat, väntar på utskick
    Sent,         // skickat till leverantör
    Completed,    // bekräftat avslutat
    NeedsAction   // kräver uppmärksamhet (t.ex. svar saknas för länge)
}

public class EstateCase
{
    public int Id { get; set; }

    public string DeceasedName { get; set; } = "";
    public string DeceasedPersonnummer { get; set; } = "";
    public DateOnly DateOfDeath { get; set; }

    public string RelativeName { get; set; } = "";
    public string RelativeRelation { get; set; } = "";
    public string RelativeContact { get; set; } = "";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<CaseItem> Items { get; set; } = new();
}

public class CaseItem
{
    public int Id { get; set; }

    public int EstateCaseId { get; set; }
    public EstateCase? EstateCase { get; set; }

    public int ProviderId { get; set; }
    public Provider? Provider { get; set; }

    public ItemStatus Status { get; set; } = ItemStatus.Pending;
    public string? GeneratedLetter { get; set; }
    public DateTime? SentAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}
