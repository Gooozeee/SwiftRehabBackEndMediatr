using AutoMapper;
using SwiftUserManagement.Application.Features.Commands.AuthenticateUser;
using SwiftUserManagement.Application.Features.Commands.CreateUser;
using SwiftUserManagement.Application.Features.Queries.GetUser;
using SwiftUserManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwiftUserManagement.Application.Mappings
{
    public class UserManagementMappingProfile : Profile
    {
        public UserManagementMappingProfile()
        {
            CreateMap<User, CreateUserCommand>().ReverseMap();
            CreateMap<User, UserVm>().ReverseMap();
            CreateMap<Tokens, TokenVM>().ReverseMap();
        }
    }
}
