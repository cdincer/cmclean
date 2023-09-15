using cmclean.API.Contacts;
using cmclean.Application.Contacts.GetAllContactDetails;
using cmclean.Application.Contacts.GetContacDetails;
using cmclean.Application.Contacts.RegisterContact;
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
    public async Task<IActionResult> Post(ContactRequest userDto)
    {
        var contact = await _mediator.Send(
            new RegisterContactCommand(
                userDto.Salutation, userDto.Firstname,
                userDto.Lastname, userDto.Email,
                userDto.Displayname, userDto.Birthdate,
                userDto.Phonenumber
            ));
        return Ok(contact.Id);
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
    public async Task<IActionResult> UpdateUser(ContactRequest userDto)
    {

        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteUser(Guid guid)
    {
        return Ok();
    }
}
