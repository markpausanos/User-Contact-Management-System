using AutoMapper;
using User_Contact_Management_System.Dtos.Users;
using User_Contact_Management_System.Models;

namespace User_Contact_Management_System.Mapper
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<UserCreateDto, ApplicationUser>();
        }
    }
}
