using System.Net;

namespace Seminario.Api.Middleware.ExceptionMiddleware;

public class SeminarioException : Exception
{
    public int StatusCode { get; }
    
    public SeminarioException(string message, HttpStatusCode code = HttpStatusCode.InternalServerError) : base(message)
    {
        this.StatusCode = (int)code;
    }
}