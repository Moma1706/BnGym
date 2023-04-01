namespace Application.Common.Exceptions
{
    public class IdentityException : Exception
    {
        public IEnumerable<String> Errors { get; }

        public IdentityException(IEnumerable<string> errors) : base("Identity exception has occurred") => Errors = errors.ToArray();

        public static IdentityException RegisterException(IEnumerable<string> errors) => new IdentityException(errors);
    }
}