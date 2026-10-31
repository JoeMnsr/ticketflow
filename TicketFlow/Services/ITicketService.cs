using TicketFlow.DTOs;
using TicketFlow.Models;

namespace TicketFlow.Services;

public interface ITicketService
{
    Task<Ticket> CreateTicketAsync(string title, string description, string customerEmail);
    Task<Ticket?> GetTicketAsync(int id);
    Task<TriageResult> ApplyTriageAsync(int id, TriageResultRequest request, CancellationToken ct = default);
}