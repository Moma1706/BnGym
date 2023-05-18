using System.Data;
using Application.CheckIn;
using Application.CheckIn.CheckIn;
using Application.Common.Exceptions;
using Application.Common.Models.BaseResult;
using Application.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class CheckInController : ApiBaseController
    {
        [HttpPost]
        //[Authorize(Roles = "Admin, Regular User")]
        public async Task<IActionResult> CheckIn([FromBody] CheckInCommand command)
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetCheckinByDate([FromQuery] CheckInGetByDateCommand command)
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
    }
}
