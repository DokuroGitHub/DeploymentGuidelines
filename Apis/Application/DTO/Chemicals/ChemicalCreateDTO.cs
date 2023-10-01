using Application.Commons.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.DTO.Chemicals;

public class ChemicalCreateDTO : IMapFrom<Chemical>
{
    public string ChemicalType { get; set; } = null!;
    public int PreHarvestIntervalInDays { get; set; }
    public string ActiveIngredient { get; set; } = null!;
    public string Name { get; set; } = null!;
    public void Mapping(Profile profile)
    {
        profile.CreateMap<ChemicalCreateDTO, Chemical>();
    }
}
