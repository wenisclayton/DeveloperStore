using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Users.UpdateUser;

public class UpdateProfile : Profile
{
/// <summary>
/// Initializes the mappings for GetUser operation
/// </summary>
public UpdateProfile()
{
    CreateMap<User, UpdateUserResult>();
}
}