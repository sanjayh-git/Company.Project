using Company.Project.Application.Accounts.Queries;
using Company.Project.Data.Entities;
using Company.Project.Data.Repositories.Interfaces;
using MediatR;

namespace Company.Project.Application.Accounts.Handlers;

public class GetAllAccountsQueryHandler : IRequestHandler<GetAllAccountsQuery, IReadOnlyList<Account>>
{
    private readonly IAccountRepository _repository;

    public GetAllAccountsQueryHandler(IAccountRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<Account>> Handle(GetAllAccountsQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetAllAsync(cancellationToken);
    }
}
