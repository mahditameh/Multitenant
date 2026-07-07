using MediatR;
using Domain.Enums;
using Domain.Interfaces;
 // اینجا استفاده شود اشکال ندارد چون Application به EF Core وابسته می‌شود


namespace Application.Orders.Commands;

public class ChangeOrderStatusCommandHandler : IRequestHandler<ChangeOrderStatusCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ITenantProvider _tenantProvider;

    public ChangeOrderStatusCommandHandler(IApplicationDbContext context, ITenantProvider tenantProvider)
    {
        _context = context;
        _tenantProvider = tenantProvider;
    }

    public async Task Handle(ChangeOrderStatusCommand request, CancellationToken cancellationToken)
    {
        // نیاز به ToListAsync داریم - اینجا باید از EF Core استفاده کنیم
        // پس باید در Application لایه، ارجاع به EF Core داشته باشیم
        
        var order = _context.Orders
            .FirstOrDefault(o => o.Id == request.OrderId && o.TenantId == _tenantProvider.TenantId);
        
        // اما IQueryable متدهای Async ندارد، پس باید راه دیگری برویم
        
        if (order == null)
            throw new NotFoundException($"Order {request.OrderId} not found");

        order.ChangeStatus(request.NewStatus);
        await _context.SaveChangesAsync(cancellationToken);
    }
}