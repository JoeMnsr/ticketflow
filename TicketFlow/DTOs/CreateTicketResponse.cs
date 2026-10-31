namespace TicketFlow.DTOs;

public record CreateTicketResponse(int Id, string Title, string Description, string CustomerEmail);