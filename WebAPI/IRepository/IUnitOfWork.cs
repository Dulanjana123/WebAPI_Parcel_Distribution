using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Data;

namespace WebAPI.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Booking> Booking { get; }
        IGenericRepository<Driver> Driver { get; }
        Task Save();
    }
}
