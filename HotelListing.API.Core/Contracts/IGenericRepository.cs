using HotelListing.API.Core.Models;

namespace HotelListing.API.Core.Contracts
{
    //enforcing what needs to happen via an interface
    //the controller needs not know that the GenericRepository is responsible
    //for knowing the details

    //the interface acts as a contract of the capabilities of the repository
    public interface IGenericRepository<T> where T : class
    {
        //get one record
        Task<TResult> GetAsync<TResult>(int? id);
        Task<T> GetAsync(int? id);
        //get all records
        Task<IEnumerable<T>> GetAllAsync();

        //add this to return a paginated list of records
        Task<PagedRecords<TResult>> GetAllAsync<TResult>(QueryParameters queryParameters);
        Task<T> AddAsync(T entity);

        // the Generics are defined after the function is named and are mainly 
        //used when you wish to make paramter types and return types generic
        Task<TResult> AddAsync<TSource, TResult>(TSource source);
        Task DeleteAsync(int id);

        Task<T> UpdateAsync(T entity);
        Task UpdateAsync<TSource>(int id, TSource source);

        Task<bool> Exists(int id);
    }



}
