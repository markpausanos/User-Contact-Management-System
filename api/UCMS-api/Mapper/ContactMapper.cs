using AutoMapper;
using User_Contact_Management_System.Dtos.Contacts;
using User_Contact_Management_System.Models;

namespace User_Contact_Management_System.Mapper
{
    public class ContactMapper : Profile
    {
        public ContactMapper()
        {
            CreateMap<Contact, ContactReturnDto>();
            CreateMap<ContactCreateDto, Contact>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ApplicationUser, opt => opt.Ignore());
        }
    }
}
