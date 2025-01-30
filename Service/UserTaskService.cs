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
using Shared.RequestFeatures;
using System.Dynamic;
using Entities.LinkModels;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Service;

internal sealed class UserTaskService : IUserTaskService
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;
    private readonly IUserTaskLinks _userTaskLinks;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public UserTaskService(IRepositoryManager repository, ILoggerManager logger, 
                            IMapper mapper, IUserTaskLinks userTaskLinks,
                            IHttpContextAccessor httpContextAccessor)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
        _userTaskLinks = userTaskLinks;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<(LinkResponse linkResponse, MetaData metaData)> GetAllUserTasksAsync(LinkParameters linkParameters, bool trackChanges)
    {
        var userId = GetCurrentUserId();

        if (!linkParameters.UserTaskParameters.ValidPriority)
            throw new UserTaskPriorityBadRequestException();

        if (!linkParameters.UserTaskParameters.ValidStatus)
            throw new UserTaskStatusBadRequestException();

        var userTasksWithMetaData = await _repository.UserTask.GetAllUserTasksAsync(userId, linkParameters.UserTaskParameters, trackChanges);

        var userTasksDto = _mapper.Map<IEnumerable<UserTaskDto>>(userTasksWithMetaData);
        var links = _userTaskLinks.TryGenerateLinks(userTasksDto, linkParameters.UserTaskParameters.Fields,
                                                    linkParameters.Context);
        return (linkResponse: links, metaData: userTasksWithMetaData.MetaData);
    }

    public async Task<UserTaskDto> GetUserTaskAsync(Guid id, bool trackChanges)
    {
        var userTask = await GetUserTaskAndCheckIfItExists(id, trackChanges);

        if (userTask.UserId != GetCurrentUserId())
        {
            throw new UnauthorizedAccessException("You do not have access to this task.");
        }

        var userTaskDto = _mapper.Map<UserTaskDto>(userTask);
        return userTaskDto;
    }

    public async Task<UserTaskDto> CreateUserTaskAsync(UserTaskForCreationDto userTask)
    {
        var userTaskEntity = _mapper.Map<UserTask>(userTask);
        userTaskEntity.UserId = GetCurrentUserId();
        userTaskEntity.UpdatedAt = DateTime.Now;
        userTaskEntity.CreatedAt = DateTime.Now;

        _repository.UserTask.CreateUserTask(userTaskEntity);
        await _repository.SaveAsync();

        var userTaskToReturn = _mapper.Map<UserTaskDto>(userTaskEntity);

        return userTaskToReturn;
    }

    public async Task<IEnumerable<UserTaskDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges)
    {
        if (ids is null)
            throw new IdParametersBadRequestException();

        var userTaskEntities = await _repository.UserTask.GetByIdsAsync(ids, trackChanges);

        if (ids.Count() != userTaskEntities.Count())
            throw new CollectionByIdsBadRequestException();

        var userTasksToReturn = _mapper.Map<IEnumerable<UserTaskDto>>(userTaskEntities);

        return userTasksToReturn;
    }

    public async Task DeleteUserTaskAsync(Guid userTaskId, bool trackChanges)
    {
        var userTask = await GetUserTaskAndCheckIfItExists(userTaskId, trackChanges);

        if (userTask.UserId != GetCurrentUserId())
        {
            throw new UnauthorizedAccessException("You do not have access to this task.");
        }

        _repository.UserTask.DeleteUserTask(userTask);
        await _repository.SaveAsync();
    }

    public async Task UpdateUserTaskAsync(Guid userTaskId, UserTaskForUpdateDto userTaskForUpdate, bool trackChanges)
    {
        var userTaskEntity = await GetUserTaskAndCheckIfItExists(userTaskId, trackChanges);

        if (userTaskEntity.UserId != GetCurrentUserId())
        {
            throw new UnauthorizedAccessException("You do not have access to this task.");
        }

        _mapper.Map(userTaskForUpdate, userTaskEntity);
        userTaskEntity.UpdatedAt = DateTime.Now;

        await _repository.SaveAsync();
    }

    public async Task<(UserTaskForUpdateDto userTaskToPatch, UserTask userTaskEntity)> GetUserTaskForPatchAsync(
        Guid userTaskId, bool trackChanges)
    {
        var userTaskEntity = await GetUserTaskAndCheckIfItExists(userTaskId, trackChanges);

        if (userTaskEntity.UserId != GetCurrentUserId())
        {
            throw new UnauthorizedAccessException("You do not have access to this task.");
        }

        var userTaskToPatch = _mapper.Map<UserTaskForUpdateDto>(userTaskEntity);
        return (userTaskToPatch, userTaskEntity);
    }

    public async Task SaveChangesForPatchAsync(UserTaskForUpdateDto userTaskToPatch, UserTask userTaskEntity)
    {
        _mapper.Map(userTaskToPatch, userTaskEntity);
        await _repository.SaveAsync();
    }

    private async Task<UserTask> GetUserTaskAndCheckIfItExists(Guid id, bool trackChanges)
    {
        var userTask = await _repository.UserTask.GetUserTaskAsync(id, trackChanges);
        if (userTask is null)
            throw new UserTaskNotFoundException(id);
        return userTask;
    }

    private string GetCurrentUserId()
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedAccessException("User is not authenticated");
        }
        return userId;
    }
}
