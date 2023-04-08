using System;
using Application.Common.Models.GymUser;

namespace Application.Common.Models.DailyTraining
{
    public class DailyTrainingResult
    {
        public bool Success { get; set; }
        public string Error { get; set; }

        public DailyTrainingResult(bool sucess, string error)
        {
            Success = sucess;
            Error = error;
        }

        public static DailyTrainingResult Sucessfull() => new(true, string.Empty);
        public static DailyTrainingResult Failure(string error) => new(false, error);
    }
}
