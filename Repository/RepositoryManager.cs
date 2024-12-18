using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository;

public sealed class RepositoryManager : IRepositoryManager
{
    private readonly RepositoryContext _repositoryContext;
    private readonly Lazy<IUserTaskRepository> _userTaskRepository;

    public RepositoryManager(RepositoryContext repositoryContext)
    {
        _repositoryContext = repositoryContext;
        _userTaskRepository = new Lazy<IUserTaskRepository>(() => new UserTaskRepository(repositoryContext));

    }

    public IUserTaskRepository UserTask => _userTaskRepository.Value;

    public async Task SaveAsync() => await _repositoryContext.SaveChangesAsync();
}
