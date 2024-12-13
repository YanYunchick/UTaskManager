using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions;

public sealed class UserTaskNotFoundException : NotFoundException
{
    public UserTaskNotFoundException(Guid userTaskId)
        : base ($"The user task with id: {userTaskId} doesn't exist in the database.")
    {

    }
}
