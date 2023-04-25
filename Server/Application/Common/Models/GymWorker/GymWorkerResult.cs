using Application.Common.Models.Auth;
using Application.Common.Models.BaseResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Common.Models.GymWorker
{
	public class GymWorkerResult
	{
        public bool Success { get; set; }
        public Error Error { get; set; }

        public GymWorkerResult(bool sucess, Error error)
        {
            Success = sucess;
            Error = error;
        }

        public static GymWorkerResult Sucessfull() => new(true, new Error { Code = 0, Message = string.Empty});
        public static GymWorkerResult Failure(Error error) => new(false, error);
    }
}
