using Application.Common.Models.BaseResult;
using Application.Common.Models.DailyUser;
using Application.DailyUser.Dtos;
using Microsoft.Data.SqlClient;

namespace Application.Common.Interfaces
{
    public interface IDailyUserService
    {
        Task<DailyUserResult> Create(string firstName, string lastName, DateTime dateOfBirth);
        Task<PageResult<DailyHistoryGetResult>> GetDailyByDate(DateTime date, string searchString, int page, int pageSize);
        Task<DailyUserResult> Update(Guid id, UpdateDailyUserDto data);
        Task<DailyUserResult> AddArrival(Guid id);
        Task<PageResult<DailyUserGetResult>> GetDailyUsers(string searchString, int page, int pageSize, SortOrder SortOrder);
        Task<DailyUserGetResult> GetOne(Guid id);
    }
}
