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

    public UserTaskDto CreateUserTask(UserTaskForCreationDto userTask)
    {
        var userTaskEntity = _mapper.Map<UserTask>(userTask);
        userTaskEntity.UpdatedAt = DateTime.Now;
        userTaskEntity.CreatedAt = DateTime.Now;

        _repository.UserTask.CreateUserTask(userTaskEntity);
        _repository.Save();

        var userTaskToReturn = _mapper.Map<UserTaskDto>(userTaskEntity);

        return userTaskToReturn;
    }

    public IEnumerable<UserTaskDto> GetByIds(IEnumerable<Guid> ids, bool trackChanges)
    {
        if (ids is null)
            throw new IdParametersBadRequestException();

        var userTaskEntities = _repository.UserTask.GetByIds(ids, trackChanges);
        if (ids.Count() != userTaskEntities.Count())
            throw new CollectionByIdsBadRequestException();

        var userTasksToReturn = _mapper.Map<IEnumerable<UserTaskDto>>(userTaskEntities);

        return userTasksToReturn;
    }

    public void DeleteUserTask(Guid userTaskId, bool trackChanges)
    {
        var userTask = _repository.UserTask.GetUserTask(userTaskId, trackChanges);
        if (userTask is null)
            throw new UserTaskNotFoundException(userTaskId);

        _repository.UserTask.DeleteUserTask(userTask);
        _repository.Save();
    }

    public void UpdateUserTask(Guid userTaskId, UserTaskForUpdateDto userTaskForUpdate, bool trackChanges)
    {
        var userTaskEntity = _repository.UserTask.GetUserTask(userTaskId, trackChanges);
        if (userTaskEntity is null)
            throw new UserTaskNotFoundException(userTaskId);

        _mapper.Map(userTaskForUpdate, userTaskEntity);
        userTaskEntity.UpdatedAt = DateTime.Now;

        _repository.Save();
    }

    public (UserTaskForUpdateDto userTaskToPatch, UserTask userTaskEntity) GetUserTaskForPatch(
        Guid userTaskId, bool trackChanges)
    {
        var userTaskEntity = _repository.UserTask.GetUserTask(userTaskId, trackChanges);
        if (userTaskEntity is null)
            throw new UserTaskNotFoundException(userTaskId);

        var userTaskToPatch = _mapper.Map<UserTaskForUpdateDto>(userTaskEntity);
        return (userTaskToPatch, userTaskEntity);
    }

    public void SaveChangesForPatch(UserTaskForUpdateDto userTaskToPatch, UserTask userTaskEntity)
    {
        _mapper.Map(userTaskToPatch, userTaskEntity);
        _repository.Save();
    }
}
