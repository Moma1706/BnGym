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
using Application.Common.Models.BaseResult;
using Application.Enums;
using Application.Common.Exceptions;

namespace WebApi.Controllers
{
    [Authorize(Roles = "Regular User")]
    public class AppController : ApiBaseController
    {
        [HttpGet]
        [Route("{Id:Int}")]
        public async Task<IActionResult> GetRegularUser([FromRoute] RegularUserGetCommand command)
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

        [HttpPut]
        [Route("{Id:Int}")]
        public async Task<IActionResult> Update([FromRoute] int Id, [FromBody] UpdateRegularUserDto data)
        {
            try
            {
                var userId = GetUserId();
                if (userId != Id)
                    return BadRequest(new Error { Message = "Proslidjen nevalidan id", Code = ExceptionType.Validation });

                var command = new RegularUserUpdateCommand
                {
                    Id = Id,
                    Data = data
                };
                var gymUserResult = await Mediator.Send(command);

                if (gymUserResult.Success)
                    return Ok();

                return BadRequest(gymUserResult.Error);
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
