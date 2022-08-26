using Benchmark;
using Benchmark.Services;

var service = new SkillService();

Console.WriteLine(Resources.MessageTestStart, DateTime.UtcNow.ToString(Resources.TimestampFormat));
File.AppendAllText(Resources.FilePathReport, string.Format(Environment.NewLine + Environment.NewLine + Resources.MessageTestStart + Environment.NewLine, DateTime.UtcNow.ToString(Resources.TimestampFormat)));

new List<int> { 5, 10, 15, 20 }.ForEach(
    candidateCount =>
    {
        new List<int> {5, 10, 15, 20}.ForEach(
            skillCount =>
            {
                Task.WaitAll(service.CreateSkills(candidateCount, skillCount, 5));
            }
        );
    }
);


File.AppendAllText(Resources.FilePathReport, string.Format(Resources.MessageTestEnd + Environment.NewLine, DateTime.UtcNow.ToString(Resources.TimestampFormat)));