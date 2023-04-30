using System.Data;
using Application.Common.Models.BaseResult;
using Application.Common.Models.DailyTraining;
using Application.DailyTraining;
using Application.DailyTraining.Dtos;
using Application.GymUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize(Roles = "Admin, Worker")]
    public class DailyTrainingController : ApiBaseController
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DailyTrainingCreateCommand command)
        {
            var checkInResult = await Mediator.Send(command);

            if (checkInResult.Success)
                return Ok();

            return Conflict(new { checkInResult.Error });
        }

        [HttpGet]
        public async Task<IActionResult> GetByDate([FromQuery] DailyTrainingGetByDateCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpGet]
        [Route("{Id:Guid}")]
        public async Task<IActionResult> GetOne([FromRoute] DailyTrainingGetOneCommand command)
        {
            var dailyTrainingResult = await Mediator.Send(command);

            if (dailyTrainingResult.Success)
                return Ok(dailyTrainingResult);

            return Conflict(new { dailyTrainingResult.Error });
        }

        [HttpPut]
        [Route("{Id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid Id, [FromBody] UpdateDailyTrainingDto data)
        {
            var command = new DailyTrainingUpdateCommand
            {
                Id = Id,
                Data = data
            };
            var dailyTrainingResult = await Mediator.Send(command);

            if (dailyTrainingResult.Success)
                return Ok();

            return Conflict(new { dailyTrainingResult.Error });
        }

        [HttpGet]
        [Route("users")]
        public async Task<IActionResult> GetUsers([FromQuery] DailyTrainingGetAllCommand command)
        {
            var gymUserResult = await Mediator.Send(command);
            return Ok(gymUserResult);
        }

        [HttpPost]
        [Route("arrival/{Id:Guid}")]
        public async Task<IActionResult> AddArrival([FromRoute] DailyTrainingAddArrivalCommand command)
        {
            var dailyTrainingResult = await Mediator.Send(command);

            if (dailyTrainingResult.Success)
                return Ok();

            return Conflict(new { dailyTrainingResult.Error });
        }
    }
}
