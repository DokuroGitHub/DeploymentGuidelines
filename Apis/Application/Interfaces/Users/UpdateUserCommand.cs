using Application.Commons.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Interfaces.Users;

public record UpdateUserCommand : IMapFrom<User>
{
    public Guid Id { get; set; }
    public DateTime? DateOfBirth { get; init; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateUserCommand, User>()
            .ForMember(des => des.Id, opt => opt.Ignore())
            .ForMember(des => des.DateOfBirth, opt => opt.Condition(src => src.DateOfBirth != null));
    }
}
