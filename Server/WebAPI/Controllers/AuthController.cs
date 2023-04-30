using System.Data;
using Application.Auth;
using Application.Auth.ForgotPassword;
using Application.Auth.Login;
using Application.Auth.Register;
using Application.Auth.ResetPassword;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class AuthController : ApiBaseController
{
    //[HttpPost]
    //[Route("register")]
    //public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    //{
    //    var registerResult = await Mediator.Send(command);
    //    if (registerResult.Success)
    //        return Ok();

    //    return Conflict(new { registerResult.Errors });
    //}

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var loginResult = await Mediator.Send(command);
        if (loginResult.Success)
            return Ok(loginResult);

        return Unauthorized(new { loginResult.Error });
    }

    [HttpPost]
    [Route("login-app")]
    public async Task<IActionResult> LoginApp([FromBody] LoginAppCommand command)
    {
        var loginResult = await Mediator.Send(command);
        if (loginResult.Success)
            return Ok(loginResult);

        return Unauthorized(new { loginResult.Error });
    }

    [HttpPost]
    [Route("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailCommand command)
    {
        var confirmEmailResult = await Mediator.Send(command);
        if (confirmEmailResult.Success)
            return Ok(confirmEmailResult);

        return BadRequest(new { confirmEmailResult.Error });
    }

    [HttpPost]
    [Route("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
    {
        var resetPasswordResult = await Mediator.Send(command);
        if (resetPasswordResult.Success)
            return Ok(resetPasswordResult);

        return BadRequest(new { resetPasswordResult.Error });
    }

    [HttpPost]
    [Route("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand command)
    {
        await Mediator.Send(command);
        return Ok();
    }

    [HttpPost]
    [Route("change-password")]
    [Authorize(Roles = "Admin, Worker, Regular User")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
    {
        var result = await Mediator.Send(command);
        if (result.Success)
            return Ok();

        return Conflict(new { result.Error });
    }
}