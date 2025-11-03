namespace InvestPlatform.Domain.RiskProfile;

public sealed record RiskProfile
{
    public Guid RiskProfileID { get; init; }
    public Guid CustomerID { get; init; }
    public DateTime AssessmentDate { get; init; }
    public string RiskToleranceLevel { get; init; } = string.Empty; // konservativ, balanceret, aggressiv
    public string InvestmentHorizon { get; init; } = string.Empty; // kort, mellem, lang
    public string LiquidityNeeds { get; init; } = string.Empty; // høj, middel, lav
    public string KnowledgeLevel { get; init; } = string.Empty; // begynder, øvet, ekspert
    public string InvestmentExperience { get; init; } = string.Empty; // ingen, begrænset, omfattende
    public string PreferredAssetClasses { get; init; } = string.Empty; // aktier, obligationer, fast ejendom osv.
    public int? RegulatorySuitabilityScore { get; init; }
    public string ProfileStatus { get; set; } = "aktiv"; // aktiv, arkiveret
}