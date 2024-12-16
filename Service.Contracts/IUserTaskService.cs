using Entities.Models;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts;

public interface IUserTaskService
{
    IEnumerable<UserTaskDto> GetAllUserTasks(bool trackChanges);
    UserTaskDto GetUserTask(Guid userTaskId, bool trackChanges);
    UserTaskDto CreateUserTask(UserTaskForCreationDto userTask);
    IEnumerable<UserTaskDto> GetByIds(IEnumerable<Guid> ids, bool trackChanges);
    void DeleteUserTask(Guid userTaskId, bool trackChanges);
    void UpdateUserTask(Guid userTaskId, UserTaskForUpdateDto userTask, bool trackChanges);
    (UserTaskForUpdateDto userTaskToPatch, UserTask userTaskEntity) GetUserTaskForPatch(
        Guid userTaskId, bool trackChanges);
    void SaveChangesForPatch(UserTaskForUpdateDto userTaskToPatch, UserTask userTaskEntity);
}
