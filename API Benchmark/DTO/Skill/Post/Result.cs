using System.Diagnostics.CodeAnalysis;

namespace Benchmark.DTO.Skill.Post;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public class Result
{
    #region Properties
    
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public string? objectID { get; init; }
    
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public string? success { get; init; }
    
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public string? message { get; init; }
    
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public string? errorCode { get; init; }
    
    #endregion
}