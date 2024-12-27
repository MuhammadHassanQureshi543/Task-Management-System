using AutoMapper;
using TaskManagementSystem.Models.DTOs;

namespace TaskManagementSystem.Models.Configrations
{
    public class autoMapConfigrations:Profile
    {
        public autoMapConfigrations()
        {
            CreateMap<UsersTable, UserDTO>().ReverseMap();
            CreateMap<TasksTable, UserDTO>().ReverseMap();

            CreateMap<TasksTable, TaskDTO>()
            .ForMember(dest => dest.AssignedUserName, opt => opt.MapFrom(src => src.AssignedUser.Name))
            .ForMember(dest => dest.CreatedUserName, opt => opt.MapFrom(src => src.CreatedUser.Name))
            .ReverseMap();

            CreateMap<TasksTable, TaskResponseDTO>()
            .ForMember(dest => dest.AssignedUserName, opt => opt.MapFrom(src => src.AssignedUser.Name))
            .ForMember(dest => dest.CreatedUserName, opt => opt.MapFrom(src => src.CreatedUser.Name))
            .ReverseMap();



        }
    }
}
