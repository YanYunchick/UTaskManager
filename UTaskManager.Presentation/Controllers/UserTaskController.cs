using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
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

        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

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

        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        _service.UserTaskService.UpdateUserTask(id, userTask, trackChanges: true);

        return NoContent();
    }

    [HttpPatch("{id:guid}")]
    public IActionResult PartiallyUpdateUserTask(Guid id, [FromBody] JsonPatchDocument<UserTaskForUpdateDto> patchDoc)
    {
        if (patchDoc is null)
            return BadRequest("patchDoc object sent from client is null");

        var result = _service.UserTaskService.GetUserTaskForPatch(id, trackChanges: true);

        patchDoc.ApplyTo(result.userTaskToPatch, ModelState);

        TryValidateModel(result.userTaskToPatch);

        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        _service.UserTaskService.SaveChangesForPatch(result.userTaskToPatch, result.userTaskEntity);

        return NoContent();
    }
}
