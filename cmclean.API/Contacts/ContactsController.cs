using cmclean.Application.Contacts.DeleteContact;
using cmclean.Application.Contacts.GetAllContactDetails;
using cmclean.Application.Contacts.GetContacDetails;
using cmclean.Application.Contacts.RegisterContact;
using cmclean.Application.Contacts.UpdateContact;
using cmclean.Contacts.UpdateContact;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace cmclean.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ContactsController : ControllerBase
{

    private readonly ILogger<ContactsController> _logger;
    private readonly IMediator _mediator;
    public ContactsController(ILogger<ContactsController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> RegisterContact(RegisterContactCommandRequest ContactDto)
    {
        var contact = await _mediator.Send(new RegisterContactCommand(ContactDto));
         return Created(contact.id.ToString(),null);  
    }

    [HttpGet]
    public async Task<IActionResult> GetAllContact()
    {
        var contact = await _mediator.Send(new GetAllContactDetailsQuery());
        return Ok(contact);
    }

    [HttpGet("GetContact/{guid}")]
    public async Task<IActionResult> GetContact(Guid guid)
    {
        var GetDetails = await _mediator.Send(new GetContactDetailsQuery(guid));
        return Ok(GetDetails);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateContact(UpdateContactRequest updatecontactdto)
    {
      var contact = await _mediator.Send(new UpdateContactCommand(updatecontactdto));


      return Ok(contact.id);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteContact(Guid guid)
    {
        var GetDetails = await _mediator.Send(new DeleteContactCommand(guid));
        return Ok(GetDetails.message);
    }
}
