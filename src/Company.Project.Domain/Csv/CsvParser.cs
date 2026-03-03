using Company.Project.Domain.Csv.Interfaces;

namespace Company.Project.Domain.Csv;

public class CsvParser : ICsvParser
{
    public async Task<IReadOnlyList<string[]>> ParseAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        var rows = new List<string[]>();

        using var reader = new StreamReader(stream, leaveOpen: true);
        string? line;
        var isFirst = true;

        while ((line = await reader.ReadLineAsync()) is not null)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(line))
                continue;

            if (isFirst)
            {
                isFirst = false;
                if (line.Contains("AccountId", StringComparison.OrdinalIgnoreCase))
                    continue;
            }

            rows.Add(line.Split(',', StringSplitOptions.TrimEntries));
        }

        if (stream.CanSeek)
            stream.Seek(0, SeekOrigin.Begin);

        return rows;
    }
}
