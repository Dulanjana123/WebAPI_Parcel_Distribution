using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Data;
using WebAPI.IRepository;

namespace WebAPI.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly APIDBContext _context;

        private IGenericRepository<Booking> _booking;

        private IGenericRepository<Driver> _driver;


        public UnitOfWork(APIDBContext context)
        {
            _context = context;
        }

        public IGenericRepository<Booking> Booking => _booking ??= new GenericRepository<Booking>(_context);

        public IGenericRepository<Driver> Driver => _driver ??= new GenericRepository<Driver>(_context);

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
