using Application.Common.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Common.Models.GymUser
{
    public class GymUserResult
    {
        public bool Success { get; set; }
        public string Error { get; set; }

        public GymUserResult(bool sucess, string error)
        {
            Success = sucess;
            Error = error;
        }

        public static GymUserResult Sucessfull() => new(true, string.Empty);
        public static GymUserResult Failure(string error) => new(false, error);
    }
}
