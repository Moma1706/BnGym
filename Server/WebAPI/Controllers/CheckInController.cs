using Application.CheckIn;
using Application.CheckIn.CheckIn;
using Application.GymUser;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class CheckInController : ApiBaseController
    {
        [HttpPost]
        public async Task<IActionResult> CheckIn([FromBody] CheckInCommand command)
        {
            var checkInResult = await Mediator.Send(command);

            if (checkInResult.Success)
                return Ok();

            return Conflict(new { checkInResult.Error });
        }

        [HttpGet]
        public async Task<IActionResult> GetCheckinByDate([FromQuery] CheckInGetByDateCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}
