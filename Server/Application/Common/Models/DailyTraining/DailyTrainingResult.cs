using System;
using Application.Common.Models.BaseResult;
using Application.Common.Models.GymUser;

namespace Application.Common.Models.DailyTraining
{
    public class DailyTrainingResult
    {
        public bool Success { get; set; }
        public Error Error { get; set; }

        public DailyTrainingResult(bool sucess, Error error)
        {
            Success = sucess;
            Error = error;
        }

        public static DailyTrainingResult Sucessfull() => new(true, new Error { Code = 0, Message = string.Empty});
        public static DailyTrainingResult Failure(Error error) => new(false, error);
    }
}
