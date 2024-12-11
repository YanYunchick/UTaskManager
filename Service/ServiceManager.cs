﻿using Contracts;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service;

public sealed class ServiceManager : IServiceManager
{
    private readonly Lazy<IUserTaskService> _userTaskService;

    public ServiceManager(IRepositoryManager repositoryManager, ILoggerManager logger)
    {
        _userTaskService = new Lazy<IUserTaskService>(() => new UserTaskService(repositoryManager, logger));
    }

    public IUserTaskService UserTaskService => _userTaskService.Value;
}
