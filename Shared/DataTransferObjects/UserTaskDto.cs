using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects;

public record UserTaskDto
{
    public Guid Id { get; init; }
    public string? Title { get; init; }
    public string? Description { get; init; }
    public int Priority { get; init; }
    public int Status { get; init; }
    public DateTime Deadline { get; init; }
}
