using FluentValidation;

namespace Application.Interfaces.Auths;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public RegisterCommandValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must not less than 6 characters.")
            .MaximumLength(20);
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.")
            .MinimumLength(6).WithMessage("Username must not less than 6 characters.")
            .MaximumLength(100).WithMessage("Username must not exceed 100 characters.")
            // .MustAsync(BeUniqueUsername).WithMessage("The specified Username already exists.")
            ;
    }

    private async Task<bool> BeUniqueUsername(string username, CancellationToken cancellationToken)
    {
        return await _unitOfWork.UserRepository.AllAsync(
            assert: x => x.Username != username,
            cancellationToken: cancellationToken);
    }
}
