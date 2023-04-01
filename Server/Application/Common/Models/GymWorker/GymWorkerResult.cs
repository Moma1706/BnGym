using System;
using Application.Common.Models.GymUser;

namespace Application.Common.Models.GymWorker
{
	public class GymWorkerResult
	{
        public bool Success { get; set; }
        public string Error { get; set; }

        public GymWorkerResult(bool sucess, string error)
        {
            Success = sucess;
            Error = error;
        }

        public static GymWorkerResult Sucessfull() => new(true, string.Empty);
        public static GymWorkerResult Failure(string error) => new(false, error);
    }
}

