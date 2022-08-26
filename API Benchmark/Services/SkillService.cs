using System.Collections.Immutable;
using System.Text.Json;
using Benchmark.DTO.Skill;
using Benchmark.DTO.Skill.Post;
using Benchmark.Types.Exceptions;

namespace Benchmark.Services;

public class SkillService
{
    #region Properties

    private GoelandApiService GoelandApiService { get; set; } = new GoelandApiService();
    
    private ImmutableList<string>? CandidateGuids => _candidateGuids ??= JsonSerializer.Deserialize<ImmutableList<string>>(Resources.CandidateGuids);
    
    #endregion
    
    #region Backing Fields

    private ImmutableList<string>? _candidateGuids;
    
    #endregion
    
    #region Public Methods
    
    public Task[] CreateSkills(int candidateCount, int skillCount, int requestCount)
    {
        var tasks = new Task[requestCount];
        var candidateGuidsClone = CandidateGuids?.ToList() ?? new List<string>();
        
        Enumerable.Range(0, requestCount)
            .ToList()
            .ForEach(
                i =>
                {
                    var skills = new List<Skill>();
                    var random = new Random();
        
                    Enumerable.Range(1, candidateCount)
                        .ToList()
                        .ForEach(
                            _ =>
                            {
                                var candidateGuid = candidateGuidsClone[random.Next(0, candidateGuidsClone.Count)];

                                skills.AddRange(Skill.NewRandom(skillCount, candidateGuid));

                                candidateGuidsClone.Remove(candidateGuid);
                            }
                        );
                    
                    Console.WriteLine(Resources.MessageCreatingSkills, DateTime.UtcNow.ToString(Resources.TimestampFormat), skillCount, candidateCount);
                    File.AppendAllText(Resources.FilePathReport, string.Format(Resources.MessageCreatingSkills + Environment.NewLine, DateTime.UtcNow.ToString(Resources.TimestampFormat), skillCount, candidateCount));
                    
                    tasks[i] = GoelandApiService.PostAsync(Resources.Endpoint, skills)
                        .ContinueWith(antecedent =>
                        {
                            try
                            {
                                var response = antecedent.Result.Content.ReadAsStringAsync().Result;

                                ReportResults(response);

                                Console.WriteLine(Resources.MessageSkillCreationExecuted, DateTime.UtcNow.ToString(Resources.TimestampFormat), response);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(string.Format(Resources.MessageErrorUnexpectedError, DateTime.UtcNow.ToString(Resources.TimestampFormat), FormatErrorMessage(e.Message)));
                                File.AppendAllText(Resources.FilePathReport, string.Format(Resources.MessageErrorUnexpectedError + Environment.NewLine, DateTime.UtcNow.ToString(Resources.TimestampFormat), FormatErrorMessage(e.Message)));
                            }
                        });
                }
            );

        return tasks;
    }
    
    #endregion
    
    #region Private Methods

    private static void ReportResults(string responseMessage)
    {
        var results = JsonSerializer.Deserialize<List<Result>>(responseMessage);

        if (results == null || !results.Any())
        {
            throw new ApiException(Resources.NoResults);
        }

        if (results[0].errorCode == Resources.ErrorCodeInvalidSessionId)
        {
            throw new ApiException(Resources.MessageErrorInvalidToken);
        }

        var successCount = results.FindAll(result => result.success == "true").Count;
        var failures = results.FindAll(result => result.success == "false");

        File.AppendAllText(Resources.FilePathReport, string.Format(Resources.MessageReportSuccessCount + Environment.NewLine, DateTime.UtcNow.ToString(Resources.TimestampFormat), successCount, results.Count));

        if (!failures.Any()) return;
        
        File.AppendAllText(Resources.FilePathReport, string.Format(Resources.MessageReportErrors + Environment.NewLine, DateTime.UtcNow.ToString(Resources.TimestampFormat)));
        File.AppendAllLines(Resources.FilePathReport, failures.Select(result => string.Format(Resources.MessageFailureResult, DateTime.UtcNow.ToString(Resources.TimestampFormat), result.objectID, FormatErrorMessage(result.message))));
    }

    private static string? FormatErrorMessage(string? errorMessage)
    {
        return string.IsNullOrEmpty(errorMessage) || !errorMessage.Contains(Resources.ErrorCodeUnableToLockRow) ?
            errorMessage?.Replace('\n', '-') :
            Resources.ErrorCodeUnableToLockRow;
    }
    
    #endregion endregion
}