using Contracts;
using Entities.LinkModels;
using Entities.Models;
using Microsoft.Net.Http.Headers;
using Shared.DataTransferObjects;
using System.ComponentModel.Design;
using System.Reflection.Metadata.Ecma335;

namespace UTaskManager.Utility;

public class UserTaskLinks : IUserTaskLinks
{
    private readonly LinkGenerator _linkGenerator;
    private readonly IDataShaper<UserTaskDto> _dataShaper;

    public UserTaskLinks(LinkGenerator linkGenerator, IDataShaper<UserTaskDto> dataShaper)
    {
        _linkGenerator = linkGenerator;
        _dataShaper = dataShaper;
    }

    public LinkResponse TryGenerateLinks(IEnumerable<UserTaskDto> userTaskDto, string fields, HttpContext httpContext)
    {
        var shapedUserTasks = ShapeData(userTaskDto, fields);

        if (ShouldGenerateLinks(httpContext))
            return ReturnLinkedUserTasks(userTaskDto, fields, httpContext, shapedUserTasks);

        return ReturnShapedUserTasks(shapedUserTasks);
    }

    private List<Entity> ShapeData(IEnumerable<UserTaskDto> userTaskDto, string fields) =>
        _dataShaper.ShapeData(userTaskDto, fields)
            .Select(e => e.Entity)
            .ToList();

    private bool ShouldGenerateLinks(HttpContext httpContext)
    {
        var mediaType = (MediaTypeHeaderValue)httpContext.Items["AcceptHeaderMediaType"]!;

        return mediaType!.SubTypeWithoutSuffix.EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase);
    }

    private LinkResponse ReturnShapedUserTasks(List<Entity> shapedUserTasks) =>
        new LinkResponse { ShapedEntities = shapedUserTasks };

    private LinkResponse ReturnLinkedUserTasks(IEnumerable<UserTaskDto> userTaskDto, string fields,
        HttpContext httpContext, List<Entity> shapedUserTasks)
    {
        var userTaskDtoList = userTaskDto.ToList();

        for (var index = 0; index < userTaskDtoList.Count(); index++)
        {
            var userTaskLinks = CreateLinksForUserTask(httpContext, userTaskDtoList[index].Id, fields);
            shapedUserTasks[index].Add("Links", userTaskLinks);
        }

        var userTaskCollection = new LinkCollectionWrapper<Entity>(shapedUserTasks);
        var linkedUserTasks = CreateLinksForUserTasks(httpContext, userTaskCollection);

        return new LinkResponse { HasLinks = true, LinkedEntities = linkedUserTasks };
    }

    private List<Link> CreateLinksForUserTask(HttpContext httpContext, Guid id, string fields = "")
    {
        var links = new List<Link>()
        {
            new Link(_linkGenerator.GetUriByAction(httpContext, "GetUserTask",
                values: new { id, fields})!,
                "self",
                "GET"),
            new Link(_linkGenerator.GetUriByAction(httpContext,"DeleteUserTask",
                values: new { id })!,
                "delete_employee",
                "DELETE"),
            new Link(_linkGenerator.GetUriByAction(httpContext,"UpdateUserTask",
                values: new { id })!,
                "update_user_task",
                "PUT"),
            new Link(_linkGenerator.GetUriByAction(httpContext,"PartiallyUpdateUserTask",
                values: new { id })!,
                "partially_update_user_task",
                "PATCH")
        };

        return links;
    }

    private LinkCollectionWrapper<Entity> CreateLinksForUserTasks(HttpContext httpContext,
        LinkCollectionWrapper<Entity> userTasksWrapper)
    {
        userTasksWrapper.Links.Add(new Link(_linkGenerator.GetUriByAction(httpContext,
            "GetUserTasks", values: new { })!,
            "self",
            "GET"));

        return userTasksWrapper;
    }
}