using Authentication.DAL;
using Authentication.Models;
using AutoMapper;

namespace Authentication.Mappings
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<UserViewModel, User>();
        }
    }
}