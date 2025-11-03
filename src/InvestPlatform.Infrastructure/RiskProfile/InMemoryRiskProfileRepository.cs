using InvestPlatform.Application.RiskProfile;
using InvestPlatform.Domain.RiskProfile;

namespace InvestPlatform.Infrastructure.RiskProfile;

public class InMemoryRiskProfileRepository : IRiskProfileRepository
{
    private readonly Dictionary<Guid, InvestPlatform.Domain.RiskProfile.RiskProfile> _profiles = new();

    public Task<InvestPlatform.Domain.RiskProfile.RiskProfile?> GetByIdAsync(Guid riskProfileId)
        => Task.FromResult(_profiles.TryGetValue(riskProfileId, out var profile) ? profile : null);

    public Task AddAsync(InvestPlatform.Domain.RiskProfile.RiskProfile riskProfile)
    {
        _profiles[riskProfile.RiskProfileID] = riskProfile;
        return Task.CompletedTask;
    }

    public Task UpdateAsync(InvestPlatform.Domain.RiskProfile.RiskProfile riskProfile)
    {
        _profiles[riskProfile.RiskProfileID] = riskProfile;
        return Task.CompletedTask;
    }
}
