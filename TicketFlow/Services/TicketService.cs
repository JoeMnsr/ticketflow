using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TicketFlow.Data;
using TicketFlow.DTOs;
using TicketFlow.Enums;
using TicketFlow.Hubs;
using TicketFlow.Models;

namespace TicketFlow.Services;

public class TicketService(IBaseRepository baseRepository, IN8nClient n8NClient, IHubContext<TicketHub> ticketHub) : ITicketService
{
    public async Task<Ticket> CreateTicketAsync(string title, string description, string customerEmail)
    {
        var newTicket = new Ticket(title, description, customerEmail);
        await baseRepository.AddAsync(newTicket);
        await baseRepository.SaveChangesAsync();
        
        await n8NClient.NotifyTicketCreatedAsync(newTicket);
        
        return newTicket;
    }

    public async Task<Ticket?> GetTicketAsync(int id)
    {
        var ticket = await baseRepository.GetReadOnlyQueryable<Ticket>()
            .FirstOrDefaultAsync(t => t.Id == id);

        return ticket;
    }

    public async Task<TriageResult> ApplyTriageAsync(int id, TriageResultRequest request, CancellationToken ct = default)
    {
        var ticket = await baseRepository.GetQueryable<Ticket>()
            .FirstOrDefaultAsync(t => t.Id == id, ct);
        
        if (ticket is null)
        {
            return new TriageResult(TriageOutcome.NotFound);
        }

        bool isCategoryValid = Enum.TryParse<TicketCategory>(request.Category, ignoreCase: true, out var category);
        bool isPriorityValid = Enum.TryParse<TicketPriority>(request.Priority, ignoreCase: true, out var priority); 
        if (!isCategoryValid || !isPriorityValid)
        {
            return new TriageResult(TriageOutcome.InvalidValues);
        }
        
        ticket.UpdateTriage(category, priority, request.Summary);

        await baseRepository.SaveChangesAsync();
        
        await ticketHub.Clients.Group(TicketHub.GroupFor(ticket.Id)).SendAsync("TicketTriaged", ticket.Id, ct);
        
        return new TriageResult(TriageOutcome.Success, ticket);
    }
}