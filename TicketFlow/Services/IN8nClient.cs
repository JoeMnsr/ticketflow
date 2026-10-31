using TicketFlow.Models;

namespace TicketFlow.Services;

public interface IN8nClient
{
    Task NotifyTicketCreatedAsync(Ticket ticket, CancellationToken ct = default);
}