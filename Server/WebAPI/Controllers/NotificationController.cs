using Application.Common.Interfaces;
using Application.Common.Models.BaseResult;
using Application.Common.Models.GymUser;
using Application.GymUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class NotificationController : ApiBaseController
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var notificationGetResult = _notificationService.GetAll();
            return Ok(notificationGetResult);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAll()
        {
            var notificationGetResult = _notificationService.DeleteAll();
            return Ok(notificationGetResult);
        }

        [HttpDelete]
        [Route("{Id:Guid}")]
        public async Task<IActionResult> DeleteOne([FromRoute] Guid Id)
        {
            try
            {
                var notificationGetResult = _notificationService.DeleteOne(Id);
                if (notificationGetResult.Success)
                    return Ok();

                return BadRequest(notificationGetResult.Error);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}