using Application.GymUser;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class GymUserController: ApiBaseController
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] GymUserCreateCommand command)
        {
            var gymUserResult = await Mediator.Send(command);

            if (gymUserResult.Success)
                return Ok();

            return Conflict(new { gymUserResult.Error });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var gymUserResult = await Mediator.Send(new GymUserGetAllCommand());
            return Ok(gymUserResult);
        }

        [HttpGet]
        [Route("{Id:Guid}")]
        public async Task<IActionResult> GetOne([FromRoute] GymUserGetOneCommand command)
        {
            var gymUserResult = await Mediator.Send(command);
            return Ok(gymUserResult);
        }
    }
}
