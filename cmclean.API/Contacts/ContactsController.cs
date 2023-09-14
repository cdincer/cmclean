using cmclean.API.Contacts;
using cmclean.Application.Contacts;
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

        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUser()
    {
        return Ok();
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
