using System;
using System.Threading.Tasks;
using HotelListing.Data;
using HotelListing.Entity;
using HotelListing.IRepository;

namespace HotelListing.Repository
{
    public class UnityOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        private IGenericRepository<Country> _countries;
        private IGenericRepository<Hotel> _hotels;
        public UnityOfWork(DataContext context)
        {
            _context = context;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool dispose)
        {
            _context.Dispose();
        }

        public IGenericRepository<Country> Countries => _countries ??= new GenericRepository<Country>(_context);
        public IGenericRepository<Hotel> Hotels => _hotels ??= new GenericRepository<Hotel>(_context);
        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
