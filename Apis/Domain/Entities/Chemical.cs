namespace Domain.Entities;

public class Chemical : BaseEntity
{
    public string ChemicalType { get; set; } = null!;

    public int PreHarvestIntervalInDays { get; set; }

    public string ActiveIngredient { get; set; } = null!;

    public string Name { get; set; } = null!;
}
