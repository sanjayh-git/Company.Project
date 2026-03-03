using Company.Project.Data.Entities;
using MediatR;

namespace Company.Project.Application.Accounts.Queries;

public record GetAllAccountsQuery() : IRequest<IReadOnlyList<Account>>;
