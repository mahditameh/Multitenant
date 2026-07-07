using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces;

public interface IApplicationDbContext
{
    // به جای DbSet از IQueryable استفاده می‌کنیم
    IQueryable<Order> Orders { get; }
    
    // متد SaveChanges بدون وابستگی به EF
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    
    // افزودن موجودیت (بدون وابستگی خاص)
    void Add(Order order);
}