using TicketFlow.Models;

namespace TicketFlow.DTOs;

public record TicketResponse(int Id, string Title, string Description, string Status, DateTime CreatedAt, string? Category, string? Priority, string? Summary, DateTime? TriagedAt)
{
    public static TicketResponse From(Ticket t) => new(t.Id, t.Title, t.Description, t.Status.ToString(), t.CreatedAt, t.Category?.ToString(), t.Priority?.ToString(), t.Summary, t.TriagedAt);
}