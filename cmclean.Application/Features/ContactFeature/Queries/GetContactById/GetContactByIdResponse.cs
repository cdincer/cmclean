namespace cmclean.Application.Features.ContactFeature.Queries.GetContactById;

public record GetContactByIdResponse
{
    public Guid Id { get; init; }
    public string Salutation { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string DisplayName { get; init; }
    public DateTime BirthDate { get; init; }
    public DateTime CreationTimestamp { get; init; }
    public DateTime LastChangeTimeStamp { get; init; }
    public bool NotifyHasBirthdaySoon { get; init; }
    public string Email { get; init; }
    public string Phonenumber { get; init; }
}
