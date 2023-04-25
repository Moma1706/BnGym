using System.Data;
using Application.GymWorker;
using Application.GymWorker.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class GymWorkerController : ApiBaseController
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] GymWorkerCreateCommand command)
        {
            var gymWorkerResult = await Mediator.Send(command);

            if (gymWorkerResult.Success)
                return Ok(gymWorkerResult);

            return Conflict(new { gymWorkerResult.Error });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GymWorkerGetAllCommand command)
        {
            var gymWorkerResult = await Mediator.Send(command);
            return Ok(gymWorkerResult);
        }

        [HttpGet]
        [Route("{Id:Guid}")]
        public async Task<IActionResult> GetOne([FromRoute] GymWorkerGetOneCommand command)
        {
            var gymWorkerResult = await Mediator.Send(command);
            if (gymWorkerResult.Success)
                return Ok(gymWorkerResult);

            return BadRequest(new { gymWorkerResult.Error });
        }

        [HttpPut]
        [Route("{Id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid Id, [FromBody] UpdateGymWorkerDto data)
        {
            var command = new GymWorkerUpdateCommand
            {
                Id = Id,
                Data = data
            };
            var gymWorkerResult = await Mediator.Send(command);

            if (gymWorkerResult.Success)
                return Ok();

            return Conflict(new { gymWorkerResult.Error });
        }

        [HttpDelete]
        [Route("{Id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] GymWorkerDeleteCommand data)
        {
            var gymUserResult = await Mediator.Send(data);

            if (gymUserResult.Success)
                return Ok();

            return Conflict(new { gymUserResult.Error });
        }
    }
}
