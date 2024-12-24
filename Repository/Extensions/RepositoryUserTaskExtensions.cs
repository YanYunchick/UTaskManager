using Entities.Models;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Extensions;

public static class RepositoryUserTaskExtensions
{
    public static IQueryable<UserTask> FilterUserTask(this IQueryable<UserTask> userTasks, int? priority, int? status) =>
        userTasks.Where(ut => (!priority.HasValue || ut.Priority == priority) &&
                                (!status.HasValue || ut.Status == status));

    public static IQueryable<UserTask> Search(this IQueryable<UserTask> userTasks, string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return userTasks;

        var lowerCaseTerm = searchTerm.Trim().ToLower();

        return userTasks.Where(ut => ut.Title!.ToLower().Contains(lowerCaseTerm));
    }
}
