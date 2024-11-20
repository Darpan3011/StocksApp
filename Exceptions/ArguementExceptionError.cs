namespace Exceptions
{
    public class ArguementExceptionError : ArgumentException
    {
        public ArguementExceptionError() : base() { }

        public ArguementExceptionError(string? message) : base(message) { }

        public ArguementExceptionError(string? message, Exception? innerException)
            : base(message, innerException) { }

        public ArguementExceptionError(string? message, string? paramName)
            : base(message, paramName)
        { }
    }
}