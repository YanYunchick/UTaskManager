using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects
{
    public abstract record UserTaskForManipulationDto
    {
        [Required(ErrorMessage = "Task title is a required field.")]
        [MaxLength(60, ErrorMessage = " Maximum length for the Title is 60 characters.")]
        public string? Title { get; init; }
        [MaxLength(1000, ErrorMessage = " Maximum length for the Description is 1000 characters.")]
        public string? Description { get; init; }
        [Range(1, 3, ErrorMessage = " The priority is required and can be in the range from 1 to 3 (Low = 1, Medium = 2, High = 3).")]
        public int Priority { get; init; }
        [Range(1, 2, ErrorMessage = " The status is required and can be 1(Active) or 2(Completed).")]
        public int Status { get; init; }
        public DateTime Deadline { get; init; }
    }
}
