using cmclean.Domain.Model;

namespace cmclean.Application.Features.ContactFeature.Commands.CreateContact;

public record CreateContactResponse
{
    public Guid Id { get; init; }
    public string Salutation { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string DisplayName { get; set; }
    public DateTime BirthDate { get; init; }
    public DateTime CreationTimestamp { get; init; }
    public DateTime LastChangeTimeStamp { get; init; }
    public string Email { get; init; }
    public string Phonenumber { get; init; }
    public bool NotifyHasBirthdaySoon { get; init; }
}

