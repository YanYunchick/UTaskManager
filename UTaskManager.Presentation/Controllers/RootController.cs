using Entities.LinkModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTaskManager.Presentation.Controllers;

[Route("api")]
[ApiController]
public class RootController : ControllerBase
{
    private readonly LinkGenerator _linkGenerator;

    public RootController(LinkGenerator linkGenerator)
    {
        _linkGenerator = linkGenerator;
    }

    [HttpGet(Name = "GetRoot")]
    public IActionResult GetRoot([FromHeader(Name = "Accept")] string mediaType)
    {
        if (mediaType.Contains("application/vnd.yy.apiroot"))
        {
            var list = new List<Link>
            {
                new Link
                {
                    Href = _linkGenerator.GetUriByName(HttpContext, nameof(GetRoot), new { }),
                    Rel = "self",
                    Method = "GET"
                },
                new Link
                {
                    Href = _linkGenerator.GetUriByName(HttpContext, "GetUserTasks", new { }),
                    Rel = "user_tasks",
                    Method = "GET"
                },
                new Link
                {
                    Href = _linkGenerator.GetUriByName(HttpContext, "CreateUserTask", new { }),
                    Rel = "create_user_task",
                    Method = "POST"
                }
            };
            return Ok(list);
        }
        return NoContent();
    }
}
