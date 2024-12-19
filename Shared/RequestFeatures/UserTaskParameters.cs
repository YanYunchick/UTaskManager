using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.RequestFeatures;

public class UserTaskParameters : RequestParameters
{
    public int? Priority { get; set; }
    public int? Status { get; set; }

    public bool ValidPriority => Priority is null ? true : Priority > 0 && Priority < 4;
    public bool ValidStatus => Status is null ? true : Status > 0 && Status < 3;

}
