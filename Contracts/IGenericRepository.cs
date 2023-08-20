using HotelListing.API.Data;

namespace hotelListingAPI.Contracts
{
    //enforcing what needs to happen via an interface
    //the controller needs not know that the GenericRepository is responsible
    //for knowing the details

    //the interface acts as a contract of the capabilities of the repository
    public interface IGenericRepository<T> where T : class
    {
        //get one record
        Task<T> GetAsync(int? id);

        //get all records
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        Task DeleteAsync(int id);

        Task<T> UpdateAsync(T entity);


        Task<bool> Exists(int id);
    }


   
}
