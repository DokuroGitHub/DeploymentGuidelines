using Application.Commons.Mappings;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Application.DTO.Users;

public class UserDTO : IMapFrom<User>
{
    public Guid _Id { get; init; }
    public string Username { get; init; } = null!;
    public DateTime DateOfBirth { get; init; }
    public UserRole Role { get; init; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<User, UserDTO>()
            .ForMember(d => d._Id, opt => opt.MapFrom(s => s.Id))
            .ReverseMap();
    }
}
