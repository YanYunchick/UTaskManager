using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Shared.RequestFeatures;
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

    public async Task<PagedList<UserTask>> GetAllUserTasksAsync(UserTaskParameters userTaskParameters, bool trackChanges)
    {
        var userTasks = await FindByCondition(ut => 
                                (!userTaskParameters.Priority.HasValue || ut.Priority == userTaskParameters.Priority) && 
                                (!userTaskParameters.Status.HasValue || ut.Status == userTaskParameters.Status)
                                , trackChanges)
                                .OrderBy(ut => ut.Title)
                                .ToListAsync();

        return PagedList<UserTask>.ToPagedList(userTasks, userTaskParameters.PageNumber, userTaskParameters.PageSize);
    }

    public async Task<UserTask> GetUserTaskAsync(Guid userTaskId, bool trackChanges) =>
        await FindByCondition(ut => ut.Id.Equals(userTaskId), trackChanges)
        .SingleOrDefaultAsync()!;

    public void CreateUserTask(UserTask userTask) => Create(userTask);

    public async Task<IEnumerable<UserTask>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) =>
        await FindByCondition(x => ids.Contains(x.Id), trackChanges)
        .ToListAsync();

    public void DeleteUserTask(UserTask userTask) => Delete(userTask); 

}
