﻿using System.Data;
using Application.GymUser;
using Application.GymWorker;
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
            var gymUserResult = await Mediator.Send(command);

            if (gymUserResult.Success)
                return Ok();

            return Conflict(new { gymUserResult.Error });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GymWorkerGetAllCommand command)
        {
            var gymUserResult = await Mediator.Send(command);
            return Ok(gymUserResult);
        }

        [HttpGet]
        [Route("{Id:Guid}")]
        public async Task<IActionResult> GetOne([FromRoute] GymWorkerGetOneCommand command)
        {
            var gymUserResult = await Mediator.Send(command);
            if (gymUserResult.Success)
                return Ok(gymUserResult);

            return BadRequest(new { gymUserResult.Error });
        }

        [HttpPost]
        [Route("{Id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] GymWorkerGetOneCommand data)
        {
            var gymUserResult = await Mediator.Send(data);

            if (gymUserResult.Success)
                return Ok();

            return Conflict(new { gymUserResult.Error });
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
