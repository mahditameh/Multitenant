using Domain.Entities;

namespace Domain.Interfaces;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(Guid id, string tenantId, CancellationToken cancellationToken);
    Task<List<Order>> GetByTenantAsync(string tenantId, CancellationToken cancellationToken);
    void Add(Order order);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}