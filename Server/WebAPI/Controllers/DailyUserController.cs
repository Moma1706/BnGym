using System.Data;
using Application.Common.Models.BaseResult;
using Application.Common.Models.DailyUser;
using Application.DailyUser;
using Application.DailyUser.Dtos;
using Application.GymUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DailyUserController : ApiBaseController
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DailyUserCreateCommand command)
        {
            var checkInResult = await Mediator.Send(command);

            if (checkInResult.Success)
                return Ok();

            return Conflict(new { checkInResult.Error });
        }

        [HttpGet]
        public async Task<IActionResult> GetByDate([FromQuery] DailyUserGetByDateCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpGet]
        [Route("{Id:Guid}")]
        public async Task<IActionResult> GetOne([FromRoute] DailyUserGetOneCommand command)
        {
            var dailyUserResult = await Mediator.Send(command);

            if (dailyUserResult.Success)
                return Ok(dailyUserResult);

            return Conflict(new { dailyUserResult.Error });
        }

        [HttpPut]
        [Route("{Id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid Id, [FromBody] UpdateDailyUserDto data)
        {
            var command = new DailyUserUpdateCommand
            {
                Id = Id,
                Data = data
            };
            var dailyUserResult = await Mediator.Send(command);

            if (dailyUserResult.Success)
                return Ok();

            return Conflict(new { dailyUserResult.Error });
        }

        [HttpGet]
        [Route("users")]
        public async Task<IActionResult> GetUsers([FromQuery] DailyUserGetAllCommand command)
        {
            var gymUserResult = await Mediator.Send(command);
            return Ok(gymUserResult);
        }

        [HttpPost]
        [Route("arrival/{Id:Guid}")]
        public async Task<IActionResult> AddArrival([FromRoute] DailyUserAddArrivalCommand command)
        {
            var dailyUserResult = await Mediator.Send(command);

            if (dailyUserResult.Success)
                return Ok();

            return Conflict(new { dailyUserResult.Error });
        }
    }
}
