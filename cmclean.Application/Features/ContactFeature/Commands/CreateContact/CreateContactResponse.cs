using cmclean.Domain.Model;

namespace cmclean.Application.Features.ContactFeature.Commands.CreateContact;

public record CreateContactResponse
{
    public Contact Data;
    public string Message;
    public bool Success;
}

