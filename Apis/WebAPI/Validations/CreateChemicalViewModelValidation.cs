using Application.DTO.Chemicals;
using FluentValidation;

namespace WebAPI.Validations;

public class CreateChemicalViewModelValidation : AbstractValidator<ChemicalCreateDTO>
{
    public CreateChemicalViewModelValidation()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
    }
}
