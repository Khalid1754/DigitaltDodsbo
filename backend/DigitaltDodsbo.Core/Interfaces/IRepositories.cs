using DigitaltDodsbo.Core.Models;

namespace DigitaltDodsbo.Core.Interfaces;

public interface IProviderRepository
{
    Task<List<Provider>> GetAllAsync();
    Task<Provider?> GetByIdAsync(int id);
}

public interface IEstateCaseRepository
{
    Task<List<EstateCase>> GetAllAsync();
    Task<EstateCase?> GetByIdAsync(int id);
    Task<EstateCase> CreateAsync(EstateCase estateCase);
    Task UpdateAsync(EstateCase estateCase);
}

public interface ILetterGenerator
{
    Task<string> GenerateAsync(EstateCase estateCase, Provider provider);
}
