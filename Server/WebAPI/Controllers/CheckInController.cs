using System.Data;
using Application.CheckIn;
using Application.CheckIn.CheckIn;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class CheckInController : ApiBaseController
    {
        [HttpPost]
        [Authorize(Roles = "Admin, Regular User")]
        public async Task<IActionResult> CheckIn([FromBody] CheckInCommand command)
        {
            var checkInResult = await Mediator.Send(command);

            if (checkInResult.Success)
                return Ok();

            return Conflict(new { checkInResult.Error });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetCheckinByDate([FromQuery] CheckInGetByDateCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}
