namespace HotelListing.API.Core.Exceptions
{

    //common exception class for NotFound errors
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string name) : base($"{name} not found")
        {

        }
        public NotFoundException(string name, object key) : base($"{name} with {key} not found")
        {

        }
    }
}
