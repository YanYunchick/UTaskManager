using AutoMapper;
using Contracts;
using Entities.ConfigurationModels;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Service.Contracts;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service;

public sealed class ServiceManager : IServiceManager
{
    private readonly Lazy<IUserTaskService> _userTaskService;
    private readonly Lazy<IAuthenticationService> _authenticationService;
    public ServiceManager(IRepositoryManager repositoryManager, 
                            ILoggerManager logger, 
                            IMapper mapper, 
                            IUserTaskLinks userTaskLinks, 
                            UserManager<User> userManager,
                            IOptions<JwtConfiguration> configuration)
    {
        _userTaskService = new Lazy<IUserTaskService>(() => 
            new UserTaskService(repositoryManager, logger, mapper, userTaskLinks));
        _authenticationService = new Lazy<IAuthenticationService>(() =>
            new AuthenticationService(logger, mapper, userManager, configuration));
    }

    public IUserTaskService UserTaskService => _userTaskService.Value;
    public IAuthenticationService AuthenticationService => _authenticationService.Value;
}
