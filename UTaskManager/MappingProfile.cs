using AutoMapper;
using Entities.Models;
using Shared.DataTransferObjects;

namespace UTaskManager;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserTask, UserTaskDto>();
        CreateMap<UserTaskForCreationDto, UserTask>();
        CreateMap<UserTaskForUpdateDto, UserTask>().ReverseMap();
    }
}
