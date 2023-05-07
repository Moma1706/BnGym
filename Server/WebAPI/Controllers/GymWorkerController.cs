using System.Data;
using System.Security.Claims;
using Application.Enums;
using Application.GymWorker;
using Application.GymWorker.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class GymWorkerController : ApiBaseController
    {
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] GymWorkerCreateCommand command)
        {
            throw new NotImplementedException("Functionality is disabled.");
            var gymWorkerResult = await Mediator.Send(command);

            if (gymWorkerResult.Success)
                return Ok(gymWorkerResult);

            return Conflict(new { gymWorkerResult.Error });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll([FromQuery] GymWorkerGetAllCommand command)
        {
            throw new NotImplementedException("Functionality is disabled.");
            var gymWorkerResult = await Mediator.Send(command);
            return Ok(gymWorkerResult);
        }

        [HttpGet]
        [Route("{Id:Guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetOne([FromRoute] GymWorkerGetOneCommand command)
        {
            throw new NotImplementedException("Functionality is disabled.");
            var gymWorkerResult = await Mediator.Send(command);
            if (gymWorkerResult.Success)
                return Ok(gymWorkerResult);

            return BadRequest(new { gymWorkerResult.Error });
        }

        [HttpPut]
        [Route("{Id:Int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromRoute] int Id, [FromBody] UpdateGymWorkerDto data)
        {
            var userId = GetUserId();
            if (userId != Id)
                return BadRequest("Invalid id provided");

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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete([FromRoute] GymWorkerDeleteCommand data)
        {
            throw new NotImplementedException("Functionality is disabled.");
            var gymUserResult = await Mediator.Send(data);

            if (gymUserResult.Success)
                return Ok();

            return Conflict(new { gymUserResult.Error });
        }

        [HttpPut]
        [Route("activate/{Id:Guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Activate([FromRoute] GymWorkerActivateCommand data)
        {
            throw new NotImplementedException("Functionality is disabled.");
            var gymUserResult = await Mediator.Send(data);

            if (gymUserResult.Success)
                return Ok();

            return Conflict(new { gymUserResult.Error });
        }

        protected int GetUserId()
        {
            return int.Parse(this.User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value);
        }
    }
}
