using HotelListing.API.Data;
using hotelListingAPI.Contracts;
using Microsoft.EntityFrameworkCore;

namespace hotelListingAPI.Repositories
{
    //the repository contains the implementations of the contract interfaces
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly HotelListingDbContext _context;

        public GenericRepository(HotelListingDbContext context)
        {
            _context = context;
        }
        public async Task DeleteAsync(int id)
        {
            var entity = await GetAsync(id);
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }   

        public async Task<bool> Exists(int id)
        {
            var entity = await GetAsync(id);
            return entity != null;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            //return the context of the specified DB model in T
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetAsync(int? id)
        {
            if (id is null)
            {
                return null;
            } 
            return await _context.Set<T>().FindAsync(id);

        }

        public async Task<T> UpdateAsync(T entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
            return entity;

        }
  
    }
}
