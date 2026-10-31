using System.ComponentModel.DataAnnotations;

namespace TicketFlow.DTOs;

public class TriageResultRequest
{
    [Required] 
    public string Category { get; set; } = string.Empty;
    [Required] 
    public string Priority { get; set; } = string.Empty;
    [Required, MaxLength(1000)] 
    public string Summary { get; set; } = string.Empty;
}