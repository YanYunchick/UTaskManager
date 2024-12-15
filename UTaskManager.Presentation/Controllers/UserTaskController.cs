using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;
using UTaskManager.Presentation.ModelBinders;

namespace UTaskManager.Presentation.Controllers;

[Route("api/userTasks")]
[ApiController]
public class UserTaskController : ControllerBase
{
    private readonly IServiceManager _service;

    public UserTaskController(IServiceManager service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetUserTasks()
    {
        var userTasks = _service.UserTaskService.GetAllUserTasks(trackChanges: false);
        return Ok(userTasks);
    }

    [HttpGet("{id:guid}", Name = "UserTaskById")]
    public IActionResult GetUserTask(Guid id)
    {
        var userTask = _service.UserTaskService.GetUserTask(id, trackChanges: false);
        return Ok(userTask);
    }

    [HttpPost]
    public IActionResult CreateUserTask([FromBody] UserTaskForCreationDto userTask)
    {
        if (userTask is null)
            return BadRequest("UserTaskForCreation object is null");

        var createdUserTask = _service.UserTaskService.CreateUserTask(userTask);

        return CreatedAtRoute("UserTaskById", new { id = createdUserTask.Id }, createdUserTask);
    }

    [HttpGet("collection/({ids})", Name = "UserTaskCollection")]
    public IActionResult GetUserTaskCollection(
        [ModelBinder(BinderType = typeof(ArrayModelBinder))]IEnumerable<Guid> ids)
    {
        var userTasks = _service.UserTaskService.GetByIds(ids, trackChanges: false);

        return Ok(userTasks);
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteUserTask(Guid id)
    {
        _service.UserTaskService.DeleteUserTask(id, trackChanges: false);

        return NoContent();
    }

    [HttpPut("{id:guid}")]
    public IActionResult UpdateUserTask(Guid id, [FromBody] UserTaskForUpdateDto userTask)
    {
        if (userTask is null)
            return BadRequest("UserTaskForUpdateDto object is null");

        _service.UserTaskService.UpdateUserTask(id, userTask, trackChanges: true);

        return NoContent();
    }
}
