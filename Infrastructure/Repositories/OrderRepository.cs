using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _context;

    public OrderRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Order?> GetByIdAsync(Guid id, string tenantId, CancellationToken cancellationToken)
    {
        return await _context.Orders
            .FirstOrDefaultAsync(o => o.Id == id && o.TenantId == tenantId, cancellationToken);
    }

    public async Task<List<Order>> GetByTenantAsync(string tenantId, CancellationToken cancellationToken)
    {
        return await _context.Orders
            .Where(o => o.TenantId == tenantId)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public void Add(Order order)
    {
        _context.Orders.Add(order);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}