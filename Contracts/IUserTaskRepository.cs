using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Contracts;

public interface IUserTaskRepository
{
    IEnumerable<UserTask> GetAllUserTasks(bool trackChanges);
    UserTask GetUserTask(Guid userTaskId, bool trackChanges);
    void CreateUserTask(UserTask userTask);
    IEnumerable<UserTask> GetByIds(IEnumerable<Guid> ids, bool trackChanges);
    void DeleteUserTask(UserTask userTask);
}
