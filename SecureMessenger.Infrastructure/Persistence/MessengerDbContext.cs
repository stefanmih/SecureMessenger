using Microsoft.EntityFrameworkCore;
using SecureMessenger.Domain.Entities;

namespace SecureMessenger.Infrastructure.Persistence;

public class MessengerDbContext : DbContext
{
    public MessengerDbContext(DbContextOptions<MessengerDbContext> options) : base(options) { }

    public DbSet<Message> Messages { get; set; }

    public DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.SenderId)
                  .IsRequired();

            entity.Property(e => e.ReceiverId)
                  .IsRequired();

            entity.Property(e => e.EncryptedContent)
                  .IsRequired()
                  .HasMaxLength(500);
        });

        base.OnModelCreating(modelBuilder);
    }

}
