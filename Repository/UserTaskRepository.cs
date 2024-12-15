﻿using Contracts;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository;

public class UserTaskRepository : RepositoryBase<UserTask>, IUserTaskRepository
{
    public UserTaskRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
    {
    }

    public IEnumerable<UserTask> GetAllUserTasks(bool trackChanges) =>
        FindAll(trackChanges)
        .OrderBy(ut => ut.Title)
        .ToList();

    public UserTask GetUserTask(Guid userTaskId, bool trackChanges) =>
        FindByCondition(ut => ut.Id.Equals(userTaskId), trackChanges)
        .SingleOrDefault()!;

    public void CreateUserTask(UserTask userTask) => Create(userTask);

    public IEnumerable<UserTask> GetByIds(IEnumerable<Guid> ids, bool trackChanges) =>
        FindByCondition(x => ids.Contains(x.Id), trackChanges)
        .ToList();

    public void DeleteUserTask(UserTask userTask) => Delete(userTask); 

}
