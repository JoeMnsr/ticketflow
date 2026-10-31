using Microsoft.AspNetCore.Mvc;
using TicketFlow.DTOs;
using TicketFlow.Enums;
using TicketFlow.Services;

namespace TicketFlow.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketController(ITicketService ticketService) : ControllerBase
{
    [HttpPost("create")]
    public async Task<ActionResult<CreateTicketResponse>> CreateTicket([FromBody] CreateTicketRequest ticket)
    {
        if (string.IsNullOrWhiteSpace(ticket.Title) || string.IsNullOrWhiteSpace(ticket.Description) || string.IsNullOrWhiteSpace(ticket.CustomerEmail))
        {
            return BadRequest("Title, Description, and CustomerEmail are required.");
        }
        
        var createdTicket = await ticketService.CreateTicketAsync(ticket.Title, ticket.Description, ticket.CustomerEmail);
        return CreatedAtAction(nameof(GetTicket), new { id = createdTicket.Id }, createdTicket);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TicketResponse>> GetTicket(int id)
    {
        var ticket = await ticketService.GetTicketAsync(id);
        return ticket is null ? NotFound() : Ok(TicketResponse.From(ticket));
    }
    
    [HttpPut("{id:int}/triage")]
    public async Task<ActionResult<TriageResult>> Triage(int id, TriageResultRequest request, CancellationToken ct)
    {
        var result = await ticketService.ApplyTriageAsync(id, request, ct);
        return result.Outcome switch
        {
            TriageOutcome.Success => Ok(result.Ticket),
            TriageOutcome.NotFound => NotFound(),
            TriageOutcome.InvalidValues => BadRequest("Invalid category or priority value."),
            _ => StatusCode(500)
        };
    }
}