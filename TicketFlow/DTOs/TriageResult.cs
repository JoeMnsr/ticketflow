using TicketFlow.Enums;
using TicketFlow.Models;

namespace TicketFlow.DTOs;

public record TriageResult(TriageOutcome Outcome, Ticket? Ticket = null);
