namespace PrismApp.Modules.CaseRunner.Models
{
    public enum SimulatorResponseCode
    {
        Success = 200,
        Accepted = 202,
        BadRequest = 400,
        Unauthorized = 401,
        NotFound = 404,
        Timeout = 408,
        Conflict = 409,
        InternalError = 500,
        ServiceUnavailable = 503,
        Error = 999
    }
}
