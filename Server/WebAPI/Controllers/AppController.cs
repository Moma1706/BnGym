using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Application.User;
using System.Security.Claims;
using Application.GymUser;
using Application.GymUser.Dtos;
using Application.App.Dtos;
using Application.App;

namespace WebApi.Controllers
{
    [Authorize(Roles = "Regular User")]
    public class AppController : ApiBaseController
    {
        [HttpGet]
        [Route("{Id:Int}")]
        public async Task<IActionResult> GetRegularUser([FromRoute] RegularUserGetCommand command)
        {
            var userId = GetUserId();
            if (userId != command.Id)
                return BadRequest("Invalid id provided");

            var result = await Mediator.Send(command);
            if (result.Success)
                return Ok(result);

            return Conflict(new { result.Error });
        }

        [HttpPut]
        [Route("{Id:Int}")]
        public async Task<IActionResult> Update([FromRoute] int Id, [FromBody] UpdateRegularUserDto data)
        {
            var userId = GetUserId();
            if (userId != Id)
                return BadRequest("Invalid id provided");

            var command = new RegularUserUpdateCommand
            {
                Id = Id,
                Data = data
            };
            var gymUserResult = await Mediator.Send(command);

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
