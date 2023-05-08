using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Application.User;
using System.Security.Claims;
using Application.Common.Models.BaseResult;
using Application.Enums;
using Application.Common.Exceptions;

namespace WebApi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : ApiBaseController
    {
        [HttpGet]
        [Route("{Id:Int}")]
        public async Task<IActionResult> GetUser([FromRoute] UserGetCommand command)
        {
            try
            {
                var userId = GetUserId();
                if (userId != command.Id)
                    return BadRequest(new Error { Message = "Proslijedjen nevalidan id", Code = ExceptionType.Validation });

                var result = await Mediator.Send(command);
                if (result.Success)
                    return Ok(result);

                return BadRequest(result.Error);
            }
            catch (Exception exception)
            {
                if (exception is ValidationException)
                {
                    string result = string.Join(". ", ((ValidationException)exception).Errors);
                    return BadRequest(new Error { Message = result, Code = ExceptionType.Validation });
                }
                throw;
            }
        }

        protected int GetUserId()
        {
           return int.Parse(this.User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value);
        }
    }
}
