using Application.Common.Models.CheckIn;

namespace Application.Common.Interfaces
{
    public interface ICheckInService
    {
        Task<CheckInResult> CheckIn(Guid gymUserId);
    }
}
