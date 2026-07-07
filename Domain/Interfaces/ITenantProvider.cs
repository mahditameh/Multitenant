namespace Domain.Interfaces;

public interface ITenantProvider
{
    string TenantId { get; }
}