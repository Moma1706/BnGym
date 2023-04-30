using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Enums
{
    public enum ExceptionType
    {
        EntityNotExist = 1,
        EntityAlreadyExists,
        UserIsInActive,
        UserIsFrozen,
        UserIsBlocked,
        WorkerIsActive,
        ExpiredMembership,
        CanNotAccesTwice,
        InvalidGymUserType,

        UnableToDelete,
        UnableToUpdate,
        UnableToCheckIn,
        UnableToRegister,
        UnableToCreate,
        EmailAlredyExists
    }
}
