# TicketFlow

A lightweight support ticket management system built with **.NET 10** and **n8n**. When a ticket is created, n8n automatically triages it (categorizes, prioritizes, and summarizes it using AI) and notifies the customer in real time via **SignalR**.

---

## How It Works

```
Customer submits ticket
        │
        ▼
 .NET API saves ticket to PostgreSQL
        │
        ▼
 API calls n8n webhook (ticket-created)
        │
        ▼
 n8n workflow triages the ticket (AI categorization, priority, summary)
        │
        ▼
 n8n calls back the API with triage results
        │
        ▼
 API updates ticket in DB & pushes real-time update via SignalR
        │
        ▼
 Customer receives live notification
```

1. A customer submits a ticket with a title, description, and email.
2. The API saves the ticket to PostgreSQL and immediately notifies n8n via a webhook.
3. n8n processes the ticket (e.g. using an AI node) to determine its category, priority, and a short summary.
4. n8n calls the `/api/tickets/{id}/triage` endpoint with the results.
5. The API updates the ticket and broadcasts a `TicketTriaged` event to any connected SignalR clients subscribed to that ticket.

---

## Tech Stack

| Component       | Technology                              |
|-----------------|-----------------------------------------|
| API             | ASP.NET Core 10                         |
| Database        | PostgreSQL 17 (via EF Core + Npgsql)    |
| Automation      | n8n (self-hosted)                       |
| Real-time push  | ASP.NET Core SignalR                    |
| API Docs        | Swagger UI (development only)           |
| Containerization| Docker / Docker Compose                 |

---

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/get-started) (for PostgreSQL and n8n)

---

## Getting Started

### 1. Start the infrastructure

Spin up PostgreSQL and n8n using Docker Compose:

```bash
docker compose up -d
```

This starts:
- **PostgreSQL** on port `5435`
- **n8n** on port `5678`

### 2. Apply database migrations

```bash
dotnet ef database update --project TicketFlow
```

### 3. Run the API

```bash
dotnet run --project TicketFlow
```

The API will be available at `https://localhost:{port}`.  
Swagger UI is available at `/swagger` in development mode.

### 4. Configure n8n

1. Open n8n at `http://localhost:5678`.
2. Create a workflow that:
   - Is triggered by the webhook at `POST /webhook/ticket-created`
   - Processes the ticket payload (e.g. with an AI/LLM node)
   - Calls back `POST /api/tickets/{id}/triage` with the result

---

## API Endpoints

| Method | Endpoint                        | Description                        |
|--------|---------------------------------|------------------------------------|
| POST   | `/api/tickets`                  | Create a new ticket                |
| GET    | `/api/tickets/{id}`             | Get a ticket by ID                 |
| POST   | `/api/tickets/{id}/triage`      | Apply triage results (called by n8n) |

## Real-Time (SignalR)

Connect to the hub at `/hubs/tickets` and subscribe to a specific ticket:

```js
await connection.invoke("Subscribe", ticketId);

connection.on("TicketTriaged", (ticketId) => {
  console.log(`Ticket ${ticketId} has been triaged!`);
});
```

---

## Project Structure

```
TicketFlow/
├── Controllers/      # HTTP endpoints
├── DTOs/             # Request/response models
├── Data/             # EF Core DbContext and repository
├── Enums/            # TicketStatus, TicketCategory, TicketPriority
├── Hubs/             # SignalR hub (TicketHub)
├── Models/           # Domain entities (Ticket)
├── Services/         # Business logic (TicketService, N8nClient)
├── appsettings.json  # Base configuration (no secrets)
└── Program.cs        # App entry point and DI setup
```
