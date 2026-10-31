namespace TicketFlow.DTOs;

public record CreateTicketRequest(string Title, string Description, string CustomerEmail);