using Service.Contracts;
using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service;

internal sealed class UserTaskService : IUserTaskService
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;

    public UserTaskService(IRepositoryManager repository, ILoggerManager logger)
    {
        _repository = repository;
        _logger = logger;
    }
}
