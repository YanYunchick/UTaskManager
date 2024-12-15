﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects;

public record UserTaskForCreationDto(string Title, string Description, int Priority, int Status, DateTime Deadline);
