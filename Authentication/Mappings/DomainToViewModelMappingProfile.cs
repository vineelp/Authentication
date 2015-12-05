using Authentication.DAL;
using Authentication.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Authentication.Mappings
{
    public class DomainToViewModelMappingProfile: Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<User, UserViewModel>();
        }
    }
}