using Service.Contracts;
using Contracts;
using Entities.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;
using Shared.DataTransferObjects;
using AutoMapper;

namespace Service;

internal sealed class UserTaskService : IUserTaskService
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public UserTaskService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    public IEnumerable<UserTaskDto> GetAllUserTasks(bool trackChanges)
    {

        var userTasks = _repository.UserTask.GetAllUserTasks(trackChanges);

        var userTasksDto = _mapper.Map<IEnumerable<UserTaskDto>>(userTasks);
        return userTasksDto;
    }

    public UserTaskDto GetUserTask(Guid id, bool trackChanges)
    {
        var userTask = _repository.UserTask.GetUserTask(id, trackChanges);
        if (userTask is null)
            throw new UserTaskNotFoundException(id);

        var userTaskDto = _mapper.Map<UserTaskDto>(userTask);
        return userTaskDto;
    }
}
