using Entities.Models;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts;

public interface IUserTaskService
{
    Task<(IEnumerable<ExpandoObject> userTasks, MetaData metaData)> GetAllUserTasksAsync(UserTaskParameters userTaskParameters, bool trackChanges);
    Task<UserTaskDto> GetUserTaskAsync(Guid userTaskId, bool trackChanges);
    Task<UserTaskDto> CreateUserTaskAsync(UserTaskForCreationDto userTask);
    Task<IEnumerable<UserTaskDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
    Task DeleteUserTaskAsync(Guid userTaskId, bool trackChanges);
    Task UpdateUserTaskAsync(Guid userTaskId, UserTaskForUpdateDto userTask, bool trackChanges);
    Task<(UserTaskForUpdateDto userTaskToPatch, UserTask userTaskEntity)> GetUserTaskForPatchAsync(
        Guid userTaskId, bool trackChanges);
    Task SaveChangesForPatchAsync(UserTaskForUpdateDto userTaskToPatch, UserTask userTaskEntity);
}
