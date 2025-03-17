using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Users.CreateUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.CreateUser;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings;

/// <summary>
/// AutoMapper profile for CreateUserRequest mappings
/// </summary>
public class CreateUserRequestProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateUserRequestProfile"/> class
    /// </summary>
    public CreateUserRequestProfile()
    {
        CreateMap<CreateUserRequest, CreateUserCommand>();
    }
}