using Application.Commons.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Interfaces.Chemicals;

public record UpdateChemicalCommand : IMapFrom<Chemical>
{
    public Guid Id { get; set; }
    public string? ChemicalType { get; init; }
    public int? PreHarvestIntervalInDays { get; init; }
    public string? ActiveIngredient { get; init; }
    public string? Name { get; init; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateChemicalCommand, Chemical>()
            .ForMember(des => des.Id, opt => opt.Ignore())
            .ForMember(des => des.ChemicalType, opt => opt.Condition(src => src.ChemicalType != null))
            .ForMember(des => des.PreHarvestIntervalInDays, opt => opt.Condition(src => src.PreHarvestIntervalInDays != null))
            .ForMember(des => des.ActiveIngredient, opt => opt.Condition(src => src.ActiveIngredient != null))
            .ForMember(des => des.Name, opt => opt.Condition(src => src.Name != null));
    }
}
