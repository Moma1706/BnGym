using Application.Common.Models.BaseResult;
using Application.Common.Models.CheckIn;
using Microsoft.Data.SqlClient;

namespace Application.Common.Interfaces
{
    public interface ICheckInService
    {
        Task<CheckInResult> CheckIn(Guid gymUserId);
        Task<PageResult<CheckInGetResult>> GetCheckInsByDate(DateTime date, string searchString, int page, int pageSize, SortOrder SortOrder);
    }
}
