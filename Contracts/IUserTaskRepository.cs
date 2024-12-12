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
}
