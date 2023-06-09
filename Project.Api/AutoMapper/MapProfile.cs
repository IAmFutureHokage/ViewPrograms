﻿using AutoMapper;
using Microsoft.Extensions.Configuration;
using Project.Core.Entities;
using Project.Core.Models.Dto;
using Project.Core.Models.Dto.User;
using System.Linq;

namespace Project.Api
{
    internal class MapProfile : Profile
    {

        public MapProfile(IConfiguration configuration)
        {
            ConfigureUser();
        }
        private void ConfigureUser()
        {
            CreateMap<User, DtoUser>();
            CreateMap<DtoUser, User>();

            CreateMap<User, DtoEditUser>();
            CreateMap<DtoEditUser, User>();

            CreateMap<User, DtoLogUser>();
            CreateMap<DtoLogUser, User>();

            CreateMap<User, DtoClaimUser>();
            CreateMap<DtoClaimUser, User>();

            CreateMap<User, DtoAddUser>();
            CreateMap<DtoAddUser, User>();
        }
       

    }

}