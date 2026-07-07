using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.Hubs;

public class OrderHub : Hub
{
    public async Task JoinTenantGroup(string tenantId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, tenantId);
    }
    
    public async Task NotifyStatusChanged(string tenantId, Guid orderId, string newStatus)
    {
        await Clients.Group(tenantId).SendAsync("OrderStatusChanged", orderId, newStatus);
        await Clients.Caller.SendAsync("OrderStatusChanged",orderId,newStatus,CancellationToken.None);
    }
}