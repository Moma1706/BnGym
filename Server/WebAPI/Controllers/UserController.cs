using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Application.User;
using System.Security.Claims;

namespace WebApi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : ApiBaseController
    {
        [HttpGet]
        [Route("{Id:Int}")]
        public async Task<IActionResult> GetUser([FromRoute] UserGetCommand command)
        {
            var userId = GetUserId();
            if (userId != command.Id)
                return BadRequest("Invalid id provided");

            var result = await Mediator.Send(command);
            if (result.Success)
                return Ok(result);

            return Conflict(new { result.Error });
        }

        protected int GetUserId()
        {
           return int.Parse(this.User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value);
        }
    }
}
