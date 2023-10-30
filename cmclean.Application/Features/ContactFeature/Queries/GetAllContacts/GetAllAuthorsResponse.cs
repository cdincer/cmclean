namespace cmclean.Application.Features.ContactFeature.Queries.GetAllContacts;

public record GetAllContactsResponse
{
    public Guid Id { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public DateTime DateOfBirth { get; init; }
}
