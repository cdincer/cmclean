namespace cmclean.Application.Features.ContactFeature.Commands.CreateContact;

public record CreateContactResponse
{
    public Guid Id { get; init; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; init; }
}