using System.ComponentModel.DataAnnotations.Schema;
using TicketFlow.Enums;

namespace TicketFlow.Models;

[Table(nameof(Ticket), Schema = "tf")]
public class Ticket
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;

    public TicketStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }

    public TicketCategory? Category { get; set; }
    public TicketPriority? Priority { get; set; }
    public string? Summary { get; set; }
    public DateTime? TriagedAt { get; set; }

    private Ticket()
    {
        
    }

    public Ticket(string title, string description, string customerEmail)
    {
        Title = title;
        Description = description;
        CustomerEmail = customerEmail;
        Status = TicketStatus.New;
        CreatedAt = DateTime.UtcNow;
    }
    
    public void UpdateTriage(TicketCategory category, TicketPriority priority, string summary)
    {
        Category = category;
        Priority = priority;
        Summary = summary;
        Status = TicketStatus.Triaged;
        TriagedAt = DateTime.UtcNow;
    }
}