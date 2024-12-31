using Entities.LinkModels;
using Microsoft.AspNetCore.Http;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts;

public interface IUserTaskLinks
{
    LinkResponse TryGenerateLinks(IEnumerable<UserTaskDto> userTaskDto, string fields, HttpContext httpContext);
}
