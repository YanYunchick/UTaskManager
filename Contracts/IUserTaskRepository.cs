using Entities.Models;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Contracts;

public interface IUserTaskRepository
{
    Task<PagedList<UserTask>> GetAllUserTasksAsync(UserTaskParameters userTaskParameters, bool trackChanges);
    Task<UserTask> GetUserTaskAsync(Guid userTaskId, bool trackChanges);
    void CreateUserTask(UserTask userTask);
    Task<IEnumerable<UserTask>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
    void DeleteUserTask(UserTask userTask);
}
