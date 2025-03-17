namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.ListUsers;

public class ListUserRequest
{
    /// <summary>
    /// The unique identifier of the user to retrieve
    /// </summary>
    public Guid Id { get; set; }
}