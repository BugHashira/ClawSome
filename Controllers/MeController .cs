using ClawSome.Dto;
using ClawSome.Entities;
using ClawSome.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ClawSome.Controllers;

[ApiController]
[Route("/me")]
public class MeController(CatFactService catFactService, IOptions<User> userConfigOptions) : ControllerBase
{
    private readonly User _configUser = userConfigOptions.Value;

    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<ProfileDto>> GetProfileAsync()
    {
        var fact = await catFactService.GetRandomCatFactAsync();

        var userDto = new UserDto
        {
            Email = _configUser.Email,
            Name = _configUser.Name,
            Stack = _configUser.Stack
        };

        var response = new ProfileDto
        {
            User = userDto,
            Timestamp = DateTimeOffset.UtcNow.ToString("o"),
            Fact = fact
        };

        return Ok(response);
    }
}
