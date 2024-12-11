using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Configuration;

public class UserTaskConfiguration : IEntityTypeConfiguration<UserTask>
{
    public void Configure(EntityTypeBuilder<UserTask> builder)
    {
        builder.HasData
        (
            new UserTask
            {
                Id = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"),
                Title = "TestTitle1",
                Description = "TestDescription1",
                Priority = 1,
                Status = 1,
                Deadline = new DateTime(2024, 12, 25, 15, 30, 0),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            },
            new UserTask
            {
                Id = new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3"),
                Title = "TestTitle2",
                Description = "TestDescription2",
                Priority = 2,
                Status = 0,
                Deadline = new DateTime(2025, 12, 25, 15, 30, 0),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            }
        );
    }
}
