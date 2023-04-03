namespace Application.Common.Models.Auth;

public class Result
{
    public bool Success { get; set; }
    public string Token { get; set; }
    public string Error { get; set; }

    public Result(bool success, string token, string error)
    {
        Success = success;
        Token = token;
        Error = error;
    }

    public static Result Successful() => new(true, string.Empty, string.Empty);

    public static Result Successful(string token) => new(true, token, string.Empty);

    public static Result Failure(string error) => new(false, string.Empty, error);
}