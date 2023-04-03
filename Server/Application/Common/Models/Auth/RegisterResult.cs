namespace Application.Common.Models.Auth;

public class RegisterResult
{
    public bool Success { get; set; }
    public string Errors { get; set; }
    public int Id { get; set; }

    public RegisterResult(bool successful, string errors, int id)
    {
        Success = successful;
        Errors = errors;
        Id = id;
    }

    public static RegisterResult Successful() => new(true, string.Empty, 0);
    public static RegisterResult Successful(int id) => new(true, string.Empty, id);
    public static RegisterResult Failure(string errors) => new(false, errors, 0);
}