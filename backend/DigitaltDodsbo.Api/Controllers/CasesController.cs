using DigitaltDodsbo.Api.DTOs;
using DigitaltDodsbo.Core.Interfaces;
using DigitaltDodsbo.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace DigitaltDodsbo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CasesController : ControllerBase
{
    private readonly IEstateCaseRepository _cases;
    private readonly IProviderRepository _providers;
    private readonly ILetterGenerator _letterGenerator;

    public CasesController(
        IEstateCaseRepository cases,
        IProviderRepository providers,
        ILetterGenerator letterGenerator)
    {
        _cases = cases;
        _providers = providers;
        _letterGenerator = letterGenerator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var cases = await _cases.GetAllAsync();
        return Ok(cases.Select(ToResponse));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var c = await _cases.GetByIdAsync(id);
        return c is null ? NotFound() : Ok(ToResponse(c));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCaseRequest request)
    {
        var estateCase = new EstateCase
        {
            DeceasedName = request.DeceasedName,
            DeceasedPersonnummer = request.DeceasedPersonnummer,
            DateOfDeath = request.DateOfDeath,
            RelativeName = request.RelativeName,
            RelativeRelation = request.RelativeRelation,
            RelativeContact = request.RelativeContact,
            Items = request.ProviderIds.Select(pid => new CaseItem { ProviderId = pid }).ToList()
        };

        var created = await _cases.CreateAsync(estateCase);

        // Generera brev direkt för varje vald leverantör
        foreach (var item in created.Items)
        {
            var provider = await _providers.GetByIdAsync(item.ProviderId);
            if (provider is null) continue;

            item.GeneratedLetter = await _letterGenerator.GenerateAsync(created, provider);
            item.Status = ItemStatus.LetterReady;
        }
        await _cases.UpdateAsync(created);

        var full = await _cases.GetByIdAsync(created.Id);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, ToResponse(full!));
    }

    [HttpPatch("{caseId}/items/{itemId}/status")]
    public async Task<IActionResult> UpdateItemStatus(int caseId, int itemId, [FromBody] ItemStatus status)
    {
        var estateCase = await _cases.GetByIdAsync(caseId);
        if (estateCase is null) return NotFound();

        var item = estateCase.Items.FirstOrDefault(i => i.Id == itemId);
        if (item is null) return NotFound();

        item.Status = status;
        if (status == ItemStatus.Sent) item.SentAt = DateTime.UtcNow;
        if (status == ItemStatus.Completed) item.CompletedAt = DateTime.UtcNow;

        await _cases.UpdateAsync(estateCase);
        return NoContent();
    }

    private static CaseResponse ToResponse(EstateCase c) => new(
        c.Id,
        c.DeceasedName,
        c.RelativeName,
        c.CreatedAt,
        c.Items.Select(i => new CaseItemResponse(
            i.Id, i.ProviderId, i.Provider?.Name ?? "", i.Status, i.GeneratedLetter
        )).ToList()
    );
}
