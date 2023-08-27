namespace HotelListing.API.Data.Entities
{
    //Define the Country Entity that models that Countries Table
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }

        public virtual IList<Hotel> Hotels { get; set; }
    }
}
