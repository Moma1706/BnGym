namespace Application.Common.Models.Auth;

public class RegisterResult
{
    public bool Success { get; set; }
    public string Errors { get; set; }

    public RegisterResult(bool successful, string errors)
    {
        Success = successful;
        Errors = errors;
    }

    public static RegisterResult Successful() => new(true, string.Empty);

    public static RegisterResult Failure(string errors) => new(false, errors);
}