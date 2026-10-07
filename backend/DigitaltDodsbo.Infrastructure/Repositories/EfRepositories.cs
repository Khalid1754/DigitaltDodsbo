using DigitaltDodsbo.Core.Interfaces;
using DigitaltDodsbo.Core.Models;
using DigitaltDodsbo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DigitaltDodsbo.Infrastructure.Repositories;

public class ProviderRepository : IProviderRepository
{
    private readonly AppDbContext _db;
    public ProviderRepository(AppDbContext db) => _db = db;

    public async Task<List<Provider>> GetAllAsync() =>
        await _db.Providers.AsNoTracking().ToListAsync();

    public async Task<Provider?> GetByIdAsync(int id) =>
        await _db.Providers.FindAsync(id);
}

public class EstateCaseRepository : IEstateCaseRepository
{
    private readonly AppDbContext _db;
    public EstateCaseRepository(AppDbContext db) => _db = db;

    public async Task<List<EstateCase>> GetAllAsync() =>
        await _db.EstateCases
            .Include(c => c.Items).ThenInclude(i => i.Provider)
            .AsNoTracking()
            .ToListAsync();

    public async Task<EstateCase?> GetByIdAsync(int id) =>
        await _db.EstateCases
            .Include(c => c.Items).ThenInclude(i => i.Provider)
            .FirstOrDefaultAsync(c => c.Id == id);

    public async Task<EstateCase> CreateAsync(EstateCase estateCase)
    {
        _db.EstateCases.Add(estateCase);
        await _db.SaveChangesAsync();
        return estateCase;
    }

    public async Task UpdateAsync(EstateCase estateCase)
    {
        _db.EstateCases.Update(estateCase);
        await _db.SaveChangesAsync();
    }
}
