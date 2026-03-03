namespace Company.Project.Domain.Csv.Interfaces;

public interface ICsvParser
{
    Task<IReadOnlyList<string[]>> ParseAsync(Stream stream, CancellationToken cancellationToken = default);
}
