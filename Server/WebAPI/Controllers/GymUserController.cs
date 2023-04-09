using Application.Enums;
using Application.GymUser;
using Application.GymWorker;
using Microsoft.AspNetCore.Mvc;
using Application.GymUser.Dtos;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace WebApi.Controllers
{
    public class GymUserController: ApiBaseController
    {
        [HttpPost]
        //[Authorize(Roles = "admin, worker")]
        public async Task<IActionResult> Create([FromBody] GymUserCreateCommand command)
        {
            var gymUserResult = await Mediator.Send(command);

            if (gymUserResult.Success)
                return Ok();

            return Conflict(new { gymUserResult.Error });
        }

        [HttpGet]
        //[Authorize(Roles = "admin, worker")]
        public async Task<IActionResult> GetAll([FromQuery] GymUserGetAllCommand command)
        {
            var gymUserResult = await Mediator.Send(command);
            return Ok(gymUserResult);
        }

        [HttpGet]
        [Route("{Id:Guid}")]
        //[Authorize(Roles = "admin, worker")]
        public async Task<IActionResult> GetOne([FromRoute] GymUserGetOneCommand command)
        {
            var gymUserResult = await Mediator.Send(command);
            if (gymUserResult.Success)
                return Ok(gymUserResult);

            return BadRequest(new { gymUserResult.Error });
        }

        [HttpPut]
        [Route("{Id:Guid}")]
        //[Authorize(Roles = "admin, worker")]
        public async Task<IActionResult> Update([FromRoute] Guid Id, [FromBody] UpdateGymUserDto data)
        {
            var command = new GymUserUpdateCommand
            {
                Id = Id,
                Data = data
            };
            var gymUserResult = await Mediator.Send(command);

            if (gymUserResult.Success)
                return Ok();

            return Conflict(new { gymUserResult.Error });
        }

        [HttpPut]
        [Route("freez/{Id:Guid}")]
        //[Authorize(Roles = "admin, worker")]
        public async Task<IActionResult> FreezMembership([FromRoute] GymUserFreezCommand command)
        {
            var gymUserResult = await Mediator.Send(command);

            if (gymUserResult.Success)
                return Ok();

            return Conflict(new { gymUserResult.Error });
        }

        [HttpPut]
        [Route("activate/{Id:Guid}")]
        //[Authorize(Roles = "admin, worker")]
        public async Task<IActionResult> ActivateMembership([FromRoute] GymUserActivateCommand command)
        {
            var gymUserResult = await Mediator.Send(command);

            if (gymUserResult.Success)
                return Ok();

            return Conflict(new { gymUserResult.Error });
        }

        [HttpPut]
        [Route("extend/{Id:Guid}")]
        //[Authorize(Roles = "admin, worker")]
        public async Task<IActionResult> ExtendMembership([FromRoute] Guid id, [FromBody] ExtendMembershipDto data)
        {
            var command = new GymUserExtendCommand
            {
                Id = id,
                Data = data
            };

            var gymUserResult = await Mediator.Send(command);

            if (gymUserResult.Success)
                return Ok();

            return Conflict(new { gymUserResult.Error });
        }

        [HttpDelete]
        [Route("{Id:Guid}")]
        //[Authorize(Roles = "admin, worker")]
        public async Task<IActionResult> Delete([FromRoute] GymUserDeleteCommand command)
        {
            var gymUserResult = await Mediator.Send(command);

            if (gymUserResult.Success)
                return Ok();

            return Conflict(new { gymUserResult.Error });
        }
    }
}
