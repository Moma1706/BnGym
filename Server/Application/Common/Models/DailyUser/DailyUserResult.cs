using System;
using Application.Common.Models.BaseResult;
using Application.Common.Models.GymUser;

namespace Application.Common.Models.DailyUser
{
    public class DailyUserResult
    {
        public bool Success { get; set; }
        public Error Error { get; set; }

        public DailyUserResult(bool sucess, Error error)
        {
            Success = sucess;
            Error = error;
        }

        public static DailyUserResult Sucessfull() => new(true, new Error { Code = 0, Message = string.Empty});
        public static DailyUserResult Failure(Error error) => new(false, error);
    }
}
