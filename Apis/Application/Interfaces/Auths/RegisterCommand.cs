using Application.Commons.Extensions;
using Application.Commons.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Interfaces.Auths;

public record RegisterCommand : IMapFrom<User>
{
    public string Username { get; init; } = null!;
    public string Password { get; init; } = null!;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<RegisterCommand, User>()
            .ForMember(d => d.PasswordHash, opt => opt.MapFrom(s => s.Password.Hash()));
    }
}
