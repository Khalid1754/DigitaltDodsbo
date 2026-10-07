using DigitaltDodsbo.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DigitaltDodsbo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProvidersController : ControllerBase
{
    private readonly IProviderRepository _providers;
    public ProvidersController(IProviderRepository providers) => _providers = providers;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var providers = await _providers.GetAllAsync();
        return Ok(providers);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var provider = await _providers.GetByIdAsync(id);
        return provider is null ? NotFound() : Ok(provider);
    }
}
