using Microsoft.Extensions.Options;
using TicketFlow.Models;

namespace TicketFlow.Services;

public class N8nClient(HttpClient http, IOptions<N8nSettings> options, ILogger<N8nClient> logger) : IN8nClient
{
    private readonly N8nSettings _settings = options.Value;

    public async Task NotifyTicketCreatedAsync(Ticket ticket, CancellationToken ct = default)
    {
        try
        {
            var payload = new
            {
                ticket.Id,
                ticket.Title,
                ticket.Description,
                ticket.CustomerEmail,
                ticket.CreatedAt
            };

            var response = await http.PostAsJsonAsync(_settings.TicketCreatedPath, payload, ct);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            // A failed notification should not make ticket creation fail
            logger.LogError(ex, "Failed to notify n8n about ticket {TicketId}", ticket.Id);
        }
    }
}