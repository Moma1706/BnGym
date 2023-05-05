using Application.Common.Models.BaseResult;
using Application.Common.Models.DailyTraining;
using Application.DailyTraining.Dtos;
using Microsoft.Data.SqlClient;

namespace Application.Common.Interfaces
{
    public interface IDailyTrainingService
    {
        Task<DailyTrainingResult> Create(string firstName, string lastName, DateTime dateOfBirth);
        Task<PageResult<DailyHistoryGetResult>> GetDailyByDate(DateTime date, string searchString, int page, int pageSize);
        Task<DailyTrainingResult> Update(Guid id, UpdateDailyTrainingDto data);
        Task<DailyTrainingResult> AddArrival(Guid id);
        Task<PageResult<DailyTrainingGetResult>> GetDailyUsers(string searchString, int page, int pageSize, SortOrder SortOrder);
        Task<DailyTrainingGetResult> GetOne(Guid id);
    }
}
