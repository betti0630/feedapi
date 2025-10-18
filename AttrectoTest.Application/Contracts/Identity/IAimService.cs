
namespace AttrectoTest.Application.Contracts.Identity;

public interface IAimService
{
    Task<string> GetAimDataAsync(string id, CancellationToken ct = default);
}
