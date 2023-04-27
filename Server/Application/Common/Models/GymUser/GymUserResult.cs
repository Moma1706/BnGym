using Application.Common.Models.Auth;
using Application.Common.Models.BaseResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Common.Models.GymUser
{
    public class GymUserResult
    {
        public bool Success { get; set; }
        public Error Error { get; set; }

        public GymUserResult(bool sucess, Error error)
        {
            Success = sucess;
            Error = error;
        }

        public static GymUserResult Sucessfull() => new(true, new Error { Code = 0, Message = string.Empty });
        public static GymUserResult Failure(Error error) => new(false, error);
    }
}
