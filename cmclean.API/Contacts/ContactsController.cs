using cmclean.API.Contacts;
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
    public async Task<IActionResult> Post(ContactRequest ContactDto)
    {
        var contact = await _mediator.Send(
            new RegisterContactCommand(
                ContactDto.salutation, ContactDto.firstname,
                ContactDto.lastname, ContactDto.email,
                ContactDto.displayname, ContactDto.birthdate,
                ContactDto.phonenumber
            ));

        if (contact != null)
        {
            return Ok(contact.Id);
        }
        else
        {
            return BadRequest("Please enter a unique customer email");
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUser()
    {
        var contact = await _mediator.Send(
          new GetAllContactDetailsQuery());
        return Ok(contact);
    }

    [HttpGet("GetUser/{guid}")]
    public async Task<IActionResult> GetUser(Guid guid)
    {
        var GetDetails = await _mediator.Send(new GetContactDetailsQuery(guid));
        return Ok(GetDetails);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUser(UpdateContactRequest updatecontactdto)
    {
        var contact = await _mediator.Send(
                  new UpdateContactCommand(
                      updatecontactdto.id,
                      updatecontactdto.salutation, updatecontactdto.firstname,
                      updatecontactdto.lastname, updatecontactdto.email,
                      updatecontactdto.displayname, updatecontactdto.birthdate,
                      updatecontactdto.phonenumber
                  ));


        if (contact != null)
        {
            return Ok(contact.Id);
        }
        else
        {
            return BadRequest("Please enter a unique customer email");
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteUser(Guid guid)
    {
        return Ok();
    }
}
