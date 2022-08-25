using System.Diagnostics.CodeAnalysis;

namespace Benchmark.DTO.Skill.Post;

public class Result
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public string? objectID { get; init; }
    
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public string? success { get; init; }
    
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public string? message { get; init; }
}