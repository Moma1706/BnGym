using Application.Enums;
using Application.GymUser;
using Application.GymWorker;
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
        public async Task<IActionResult> GetAll([FromQuery] GymUserGetAllCommand command)
        {
            var gymUserResult = await Mediator.Send(new GymUserGetAllCommand());
            return Ok(gymUserResult);
        }

        [HttpGet]
        [Route("{Id:Guid}")]
        public async Task<IActionResult> GetOne([FromRoute] GymUserGetOneCommand command)
        {
            var gymUserResult = await Mediator.Send(command);
            if (gymUserResult.Success)
                return Ok(gymUserResult);

            return BadRequest(new { gymUserResult.Error });
        }

        [HttpPut]
        [Route("{Id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid dataId, [FromBody] UpdateCommand command)
        {
            //var command = new UpdateCommand
            //{
            //    Id = id,
            //    Type = type
            //};
            var gymUserResult = await Mediator.Send(command);

            if (gymUserResult.Success)
                return Ok();

            return Conflict(new { gymUserResult.Error });
        }

        [HttpPut]
        [Route("freez/{Id:Guid}")]
        public async Task<IActionResult> FreezMembership([FromRoute] GymUserFreezCommand command)
        {
            var gymUserResult = await Mediator.Send(command);

            if (gymUserResult.Success)
                return Ok();

            return Conflict(new { gymUserResult.Error });
        }

        [HttpPut]
        [Route("activate/{Id:Guid}")]
        public async Task<IActionResult> ActivateMembership([FromRoute] GymUserActivateCommand command)
        {
            var gymUserResult = await Mediator.Send(command);

            if (gymUserResult.Success)
                return Ok();

            return Conflict(new { gymUserResult.Error });
        }

        [HttpPut]
        [Route("extend/{Id:Guid}")]
        public async Task<IActionResult> ExtendMembership([FromRoute] Guid id, [FromBody] GymUserType type)
        {
            var command = new GymUserExtendCommand
            {
                Id = id,
                Type = type
            };

            var gymUserResult = await Mediator.Send(command);

            if (gymUserResult.Success)
                return Ok();

            return Conflict(new { gymUserResult.Error });
        }
        

    }
}
