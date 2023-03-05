using Application.Common.Models.Auth;
using Application.Common.Models.CheckIn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface ICheckInService
    {
        Task<CheckInResult> CheckIn(int UserId);
    }
}
