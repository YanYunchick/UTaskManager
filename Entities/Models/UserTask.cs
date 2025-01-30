using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class UserTask
    {
        [Column("UserTaskId")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Task title is a required field.")]
        [MaxLength(60, ErrorMessage = " Maximum length for the Title is 60 characters.")]
        public string? Title { get; set; }

        public string? Description { get; set; }
        public int Priority { get; set; }
        public int Status { get; set; }
        public DateTime Deadline { get; set; }


        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}
