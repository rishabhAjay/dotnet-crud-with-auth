using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelListing.API.Core.Contracts;
using HotelListing.API.Core.Exceptions;
using HotelListing.API.Core.Models;
using HotelListing.API.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Core.Repositories
{
    //the repository contains the implementations of the contract interfaces
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly HotelListingDbContext _context;
        private readonly IMapper _mapper;

        public GenericRepository(HotelListingDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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

        public async Task<TResult> AddAsync<TSource, TResult>(TSource source)
        {
            var entity = _mapper.Map<T>(source);

            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();

            return _mapper.Map<TResult>(entity);
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
                //pass an exception and a custom string if id is null
                throw new NotFoundException(typeof(T).Name, id.HasValue ? id : "No Key Provided");
            }
            return await _context.Set<T>().FindAsync(id);

        }

        public async Task<TResult> GetAsync<TResult>(int? id)
        {
            if (id is null)
            {
                throw new NotFoundException(typeof(T).Name, id.HasValue ? id : "No Key Provided");
            }
            var result = await _context.Set<T>().FindAsync(id);
            //map the incoming dto generic to the result from the query
            return _mapper.Map<TResult>(result);

        }

        public async Task<T> UpdateAsync(T entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
            return entity;

        }


        public async Task UpdateAsync<TSource>(int id, TSource source)
        {

            var entity = await GetAsync(id);

            if (entity == null)
            {
                throw new NotFoundException(typeof(T).Name, id);
            }
            //map the source generic dto data to the entity that we found
            _mapper.Map(source, entity);
            //update
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<PagedRecords<TResult>> GetAllAsync<TResult>(QueryParameters queryParameters)
        {
            var totalSize = await _context.Set<T>().CountAsync();
            var records = await _context.Set<T>()
                .Skip(queryParameters.StartIndex)
                .Take(queryParameters.PageSize)
                //pass the generic as the mapper dto and the Config provider looks
                //through the mapper config to match the dto
                //this affects the SQL query to return only the needed data
                .ProjectTo<TResult>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return new PagedRecords<TResult>
            {
                Records = records,
                PageNumber = queryParameters.StartIndex,
                RecordNumber = queryParameters.PageSize,
                TotalCount = totalSize
            };
        }

        public Task UpdateAsync<TSource>(TSource source)
        {
            throw new NotImplementedException();
        }
    }
}
