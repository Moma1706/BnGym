using System;
using Application.Common.Models.BaseResult;

namespace Application.Common.Models.User
{
    public class UserGetResult
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public bool Success { get; set; }
        public Error Error { get; set; }

        public UserGetResult(bool success, Error error, int id, string firstName, string lastName, string email)
        {
            Success = success;
            Error = error;
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }

        public UserGetResult() { }

        public static UserGetResult Sucessfull(int id, string firstName, string lastName, string email) => new(true, new Error { Code = 0, Message = string.Empty }, id, firstName, lastName, email);
        public static UserGetResult Failure(Error error) => new(false, error, 0, string.Empty, string.Empty, string.Empty);
    }
}
