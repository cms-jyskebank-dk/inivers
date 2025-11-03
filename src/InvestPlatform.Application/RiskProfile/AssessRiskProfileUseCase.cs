using InvestPlatform.Domain.RiskProfile;

namespace InvestPlatform.Application.RiskProfile;

public class AssessRiskProfileUseCase
{
    private readonly IRiskProfileRepository _repository;
    public AssessRiskProfileUseCase(IRiskProfileRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> AssessAsync(InvestPlatform.Domain.RiskProfile.RiskProfile profile)
    {
        var newProfile = profile with
        {
            RiskProfileID = Guid.NewGuid(),
            AssessmentDate = DateTime.UtcNow,
            ProfileStatus = "aktiv"
        };
        await _repository.AddAsync(newProfile);
        return newProfile.RiskProfileID;
    }
}
