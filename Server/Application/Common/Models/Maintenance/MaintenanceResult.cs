using System;
using Application.Common.Models.GymUser;

namespace Application.Common.Models.Maintenance
{
    public class MaintenanceResult
    {
        public bool Success { get; set; }
        public string Error { get; set; }

        public MaintenanceResult(bool sucess, string error)
        {
            Success = sucess;
            Error = error;
        }

        public static MaintenanceResult Sucessfull() => new(true, string.Empty);
        public static MaintenanceResult Failure(string error) => new(false, error);
    }
}
