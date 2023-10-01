using Application.Commons.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.DTO.Chemicals;

public class ChemicalDTO : IMapFrom<Chemical>
{
    public string _Id { get; set; } = null!;
    public string ChemicalType { get; set; } = null!;
    public int PreHarvestIntervalInDays { get; set; }
    public string ActiveIngredient { get; set; } = null!;
    public string Name { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Chemical, ChemicalDTO>()
            .ForMember(d => d._Id, opt => opt.MapFrom(s => s.Id))
            .ReverseMap();
    }
}
