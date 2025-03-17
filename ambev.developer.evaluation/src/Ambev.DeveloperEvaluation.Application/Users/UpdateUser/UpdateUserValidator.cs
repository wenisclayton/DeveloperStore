using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Users.UpdateUser;

public class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
{
    /// <summary>
    /// Initializes validation rules for GetUserCommand
    /// </summary>
    public UpdateUserValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("User ID is required");
    }
}