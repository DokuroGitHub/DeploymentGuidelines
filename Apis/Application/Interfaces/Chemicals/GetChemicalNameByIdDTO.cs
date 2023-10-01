using Application.Commons.Mappings;
using Domain.Entities;

namespace Application.Interfaces.Chemicals;

public class GetChemicalNameByIdDTO : IMapFrom<Chemical>
{
    public string Name { get; init; } = null!;
}
