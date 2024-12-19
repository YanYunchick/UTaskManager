using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;
using UTaskManager.Presentation.ActionFilters;
using UTaskManager.Presentation.ModelBinders;
using Shared.RequestFeatures;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

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
    public async Task<IActionResult> GetUserTasks([FromQuery] UserTaskParameters userTaskParameters)
    {
        var pagedResult = await _service.UserTaskService.GetAllUserTasksAsync(userTaskParameters, trackChanges: false);

        Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

        return Ok(pagedResult.userTasks);
    }

    [HttpGet("{id:guid}", Name = "UserTaskById")]
    public async Task<IActionResult> GetUserTask(Guid id)
    {
        var userTask = await _service.UserTaskService.GetUserTaskAsync(id, trackChanges: false);
        return Ok(userTask);
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> CreateUserTask([FromBody] UserTaskForCreationDto userTask)
    {
        var createdUserTask = await _service.UserTaskService.CreateUserTaskAsync(userTask);

        return CreatedAtRoute("UserTaskById", new { id = createdUserTask.Id }, createdUserTask);
    }

    [HttpGet("collection/({ids})", Name = "UserTaskCollection")]
    public async Task<IActionResult> GetUserTaskCollection(
        [ModelBinder(BinderType = typeof(ArrayModelBinder))]IEnumerable<Guid> ids)
    {
        var userTasks = await _service.UserTaskService.GetByIdsAsync(ids, trackChanges: false);

        return Ok(userTasks);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteUserTask(Guid id)
    {
        await _service.UserTaskService.DeleteUserTaskAsync(id, trackChanges: false);

        return NoContent();
    }

    [HttpPut("{id:guid}")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> UpdateUserTask(Guid id, [FromBody] UserTaskForUpdateDto userTask)
    {
        await _service.UserTaskService.UpdateUserTaskAsync(id, userTask, trackChanges: true);

        return NoContent();
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> PartiallyUpdateUserTask(Guid id, [FromBody] JsonPatchDocument<UserTaskForUpdateDto> patchDoc)
    {
        if (patchDoc is null)
            return BadRequest("patchDoc object sent from client is null");

        var result = await _service.UserTaskService.GetUserTaskForPatchAsync(id, trackChanges: true);

        patchDoc.ApplyTo(result.userTaskToPatch, ModelState);

        TryValidateModel(result.userTaskToPatch);

        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        await _service.UserTaskService.SaveChangesForPatchAsync(result.userTaskToPatch, result.userTaskEntity);

        return NoContent();
    }
}
