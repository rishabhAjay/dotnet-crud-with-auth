namespace HotelListing.API.Core.Exceptions
{
    public class BadRequestException : ApplicationException
    {
        public BadRequestException(string name) : base($"{name} has made a bad request ")
        {

        }
        public BadRequestException(string name, object key) : base($"{name} has made a bad request with {key} ")
        {

        }
    }
}
