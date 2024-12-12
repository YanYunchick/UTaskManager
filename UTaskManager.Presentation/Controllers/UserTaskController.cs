using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

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
    public IActionResult GetCompanies()
    {
        var userTasks = _service.UserTaskService.GetAllUserTasks(trackChanges: false);
        return Ok(userTasks);
    }
}
