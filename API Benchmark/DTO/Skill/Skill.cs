using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace Benchmark.DTO.Skill;

public class Skill
{
    #region Properties

    private static ImmutableDictionary<string, ImmutableList<string>>? SkillCodesByType => _skillCodesByType ??= JsonSerializer.Deserialize<ImmutableDictionary<string, ImmutableList<string>>>(Resources.Skills);

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public string? candidateGUID { get; init; }
    
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public string? type { get; init; }
    
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public string? skillCode { get; init; }
    
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public bool? verified { get; init; }

    #endregion
    
    #region Backing Fields

    private static ImmutableDictionary<string, ImmutableList<string>>? _skillCodesByType;
    
    #endregion
    
    #region static Methods

    public static IEnumerable<Skill> NewRandom(int count, string candidateGuid)
    {
        var skillCodesByTypeClone = SkillCodesByType?.ToDictionary(
            entry => entry.Key, 
            entry => entry.Value.ToList()
        );
        
        var random = new Random();
        
        return Enumerable.Range(1, count)
            .ToList()
            .Select(_ =>
            {
                var skillType = skillCodesByTypeClone?.Keys.ToList()[random.Next(0, skillCodesByTypeClone.Keys.Count)];
                var skillCodes = skillType == null ? null : skillCodesByTypeClone?[skillType];
                var skillCode = skillCodes?[random.Next(0, skillCodes.Count)];
                
                var skill = new Skill
                {
                    candidateGUID = candidateGuid,
                    type = skillType,
                    skillCode = skillCode,
                    verified = true
                };

                if (skillCode != null) skillCodes?.Remove(skillCode);
                if ((skillCodes == null || skillCodes.Count == 0) && skillType != null) skillCodesByTypeClone?.Remove(skillType);

                return skill;
            })
            .ToList();
    }
    
    #endregion
}