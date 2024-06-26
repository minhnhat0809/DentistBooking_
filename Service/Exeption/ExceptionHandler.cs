namespace Service.Exeption;

public class ExceptionHandler
{
    public class ServiceException : Exception
    {
        public ServiceException(string message) : base(message) { }
        public ServiceException(string message, Exception innerException) : base(message, innerException) { }
    }
    public class NotFoundException : ServiceException
    {
        public NotFoundException(string message) : base(message) { }
    }
}