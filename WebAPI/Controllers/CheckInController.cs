using Application.CheckIn.CheckIn;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class CheckInController : ApiBaseController
    {
        [HttpPost]
        [Route("check-in")]
        public async Task<IActionResult> CheckIn([FromBody] CheckInCommand command)
        {
            var checkInResult = await Mediator.Send(command);

            if (checkInResult.Success)
                return Ok();

            return Conflict(new { checkInResult.Error });
        }
    }
}
