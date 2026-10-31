using Microsoft.EntityFrameworkCore;
using TicketFlow.Models;

namespace TicketFlow.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Ticket> Tickets => Set<Ticket>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ticket>(e =>
        {
            e.HasKey(t => t.Id);

            e.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(200);

            e.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(4000);

            e.Property(t => t.CustomerEmail)
                .IsRequired()
                .HasMaxLength(320);

            e.Property(t => t.Summary)
                .HasMaxLength(1000);

            e.Property(t => t.Status)
                .HasConversion<string>()
                .HasMaxLength(20);

            e.Property(t => t.Priority)
                .HasConversion<string>()
                .HasMaxLength(20);

            e.Property(t => t.Category)
                .HasConversion<string>()
                .HasMaxLength(30);

            e.HasIndex(t => t.Status);
            e.HasIndex(t => t.CreatedAt);
        });
        
        ApplyLowerCaseNamingConvention(modelBuilder);
    }
    
    private void ApplyLowerCaseNamingConvention(ModelBuilder modelBuilder)
    {
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            // lowers the table names
            entity.SetTableName(entity.GetTableName()!.ToLower());

            // iterates over the entity properties and lowercase them
            foreach (var property in entity.GetProperties())
                property.SetColumnName(property.GetColumnName()!.ToLower());

            // iterates over the entity keys and lowercase them
            foreach (var key in entity.GetKeys())
                key.SetName(key.GetName()!.ToLower());

            // iterates over the entity indexes and lowercase them
            foreach (var index in entity.GetIndexes())
                index.SetDatabaseName(index.GetDatabaseName()!.ToLower());
        }
    }
}