using Microsoft.AspNetCore.SignalR;

namespace TicketFlow.Hubs;

public class TicketHub : Hub
{
    public static string GroupFor(int ticketId) => $"ticket-{ticketId}";

    public async Task Subscribe(int ticketId) =>
        await Groups.AddToGroupAsync(Context.ConnectionId, GroupFor(ticketId));

    public async Task Unsubscribe(int ticketId) =>
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, GroupFor(ticketId));
}