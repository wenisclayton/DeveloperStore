using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.ListUsers;

public class ListUserRequestValidator : AbstractValidator<ListUserRequest>
{
    /// <summary>
    /// Initializes validation rules for UpdateUserRequest
    /// </summary>
    public ListUserRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("User ID is required");
    }
}