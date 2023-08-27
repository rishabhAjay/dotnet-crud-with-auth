namespace HotelListing.API.Core.Exceptions
{
    public class UnauthorizedException : ApplicationException
    {
        public UnauthorizedException(string name) : base($"{name} has raised an unauthorized error")
        {

        }
        public UnauthorizedException(string name, object key) : base($"{name} is unauthorized with the id {key} ")
        {

        }
    }
}
