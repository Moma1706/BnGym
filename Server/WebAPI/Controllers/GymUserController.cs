using Application.Common.Exceptions;
using Application.Common.Models.BaseResult;
using Application.Enums;
using Application.GymUser;
using Application.GymUser.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class GymUserController : ApiBaseController
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] GymUserCreateCommand command)
        {
            try
            {
                var gymUserResult = await Mediator.Send(command);
                if (gymUserResult.Success)
                    return Ok(gymUserResult);

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

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GymUserGetAllCommand command)
        {
            var gymUserResult = await Mediator.Send(command);
            return Ok(gymUserResult);
        }

        [HttpGet]
        [Route("{Id:Int}")]
        public async Task<IActionResult> GetOne([FromRoute] GymUserGetOneCommand command)
        {
            try
            {
                var gymUserResult = await Mediator.Send(command);
                if (gymUserResult.Success)
                    return Ok(gymUserResult);

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

        [HttpPut]
        [Route("{Id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid Id, [FromBody] UpdateGymUserDto data)
        {
            try
            {
                var command = new GymUserUpdateCommand
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

        [HttpPut]
        [Route("freez/{Id:Guid}")]
        public async Task<IActionResult> FreezMembership([FromRoute] GymUserFreezCommand command)
        {
            try
            {
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

        [HttpPut]
        [Route("freez-all")]
        public async Task<IActionResult> FreezAllMemberships([FromRoute] GymUserFreezAllCommand command)
        {
            try
            {
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

        [HttpPut]
        [Route("activate-all")]
        public async Task<IActionResult> ActivateAllMemberships([FromRoute] GymUserActivateAllCommand command)
        {
            try
            {
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

        [HttpPut]
        [Route("activate/{Id:Guid}")]
        public async Task<IActionResult> ActivateMembership([FromRoute] GymUserActivateCommand command)
        {
            try
            {
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

        [HttpPut]
        [Route("extend/{Id:Guid}")]
        public async Task<IActionResult> ExtendMembership([FromRoute] Guid id, [FromBody] ExtendMembershipDto data)
        {
            try
            {
                var command = new GymUserExtendCommand
                {
                    Id = id,
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

        //[HttpGet]
        //[Route("most-arrivals")]
        //public async Task<IActionResult> GetWithMostAttivals([FromQuery] GymUserGetAllCommand command)
        //{
        //    var gymUserResult = await Mediator.Send(command);
        //    return Ok(gymUserResult);
        //}
    }
}