using System.Data;
using Application.Common.Exceptions;
using Application.Common.Models.BaseResult;
using Application.Common.Models.DailyUser;
using Application.DailyUser;
using Application.DailyUser.Dtos;
using Application.Enums;
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
            try
            {
                var checkInResult = await Mediator.Send(command);

                if (checkInResult.Success)
                    return Ok();

                return BadRequest(checkInResult.Error);
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

        [HttpGet]
        public async Task<IActionResult> GetByDate([FromQuery] DailyUserGetByDateCommand command)
        {
            try
            {
                return Ok(await Mediator.Send(command));
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

        [HttpGet]
        [Route("{Id:Guid}")]
        public async Task<IActionResult> GetOne([FromRoute] DailyUserGetOneCommand command)
        {
            try
            {
                var dailyUserResult = await Mediator.Send(command);

                if (dailyUserResult.Success)
                    return Ok(dailyUserResult);

                return BadRequest(dailyUserResult.Error);
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
        [Route("{Id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid Id, [FromBody] UpdateDailyUserDto data)
        {
            try
            {
                var command = new DailyUserUpdateCommand
                {
                    Id = Id,
                    Data = data
                };
                var dailyUserResult = await Mediator.Send(command);

                if (dailyUserResult.Success)
                    return Ok();

                return BadRequest(dailyUserResult.Error);
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
            try
            {
                var dailyUserResult = await Mediator.Send(command);

                if (dailyUserResult.Success)
                    return Ok();

                return BadRequest(dailyUserResult.Error);
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
    }
}
