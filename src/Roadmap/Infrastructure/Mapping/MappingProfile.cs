using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Roadmap.Models.Db;
using Roadmap.Models.Dto.Dto;
using Roadmap.Models.Dto.Requests.User;

namespace Tiove.Roadmap.Infrastructure.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateUserRequest, DbUser>()
            .ForMember(db => db.Id, _ => Guid.NewGuid())
            .ForMember(db => db.Status, opt => opt.MapFrom(x => 0))
            .ForMember(db => db.IsAdmin, opt => opt.MapFrom(x => false))
            .ForMember(db => db.IsActive, opt => opt.MapFrom(x => true))
            .ForMember(db => db.CreatedBy, _ => Guid.NewGuid()) //TODO with authentication
            .ForMember(db => db.CreatedAtUtc, opt => opt.MapFrom(x => DateTime.UtcNow));

        CreateMap<DbUser, UserDto>()
            .ReverseMap();

        CreateMap<EditUserRequest, DbUser>()
            .ReverseMap();

        CreateMap<Operation<EditUserRequest>, Operation<DbUser>>()
            .ForMember(db => db.op, opt => opt.MapFrom(x => x.op))
            .ForMember(db => db.path, opt => opt.MapFrom(x => x.path))
            .ForMember(db => db.value, opt => opt.MapFrom(x => x.value))
            .ReverseMap();

        CreateMap<JsonPatchDocument<EditUserRequest>, JsonPatchDocument<DbUser>>()
            .ForMember(db => db.Operations, opt => opt.MapFrom(x => x.Operations))
            .ReverseMap();
    }
}