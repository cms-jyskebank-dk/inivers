using InvestPlatform.Domain.RiskProfile;

namespace InvestPlatform.Application.RiskProfile;

public interface IRiskProfileRepository
{
    Task<InvestPlatform.Domain.RiskProfile.RiskProfile?> GetByIdAsync(Guid riskProfileId);
    Task AddAsync(InvestPlatform.Domain.RiskProfile.RiskProfile riskProfile);
    Task UpdateAsync(InvestPlatform.Domain.RiskProfile.RiskProfile riskProfile);
}
