using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public sealed class UserTaskPriorityBadRequestException : BadRequestException
    {
        public UserTaskPriorityBadRequestException()
            :base("The priority can be in the range from 1 to 3 (Low = 1, Medium = 2, High = 3).")
        {
            
        }
    }
}
