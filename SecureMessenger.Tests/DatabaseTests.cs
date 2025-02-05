using Microsoft.EntityFrameworkCore;
using SecureMessenger.Infrastructure.Persistence;
using SecureMessenger.Domain.Entities;

namespace SecureMessenger.Tests
{
    public class DatabaseTests
    {
        private DbContextOptions<MessengerDbContext> _options;

        public DatabaseTests()
        {
            _options = new DbContextOptionsBuilder<MessengerDbContext>()
                        .UseMySql("Server=localhost;Database=SecureMessenger;User=messenger_user;Password=frizider;",
                            new MySqlServerVersion(new Version(8, 0, 30))) 
                        .Options;
        }

        [Fact]
        public void CanInsertMessage_IntoDatabase()
        {
            using (var context = new MessengerDbContext(_options))
            {
                var message = new Message
                {
                    SenderId = 1,
                    ReceiverId = 2,
                    EncryptedContent = "Test Message",
                    SentAt = DateTime.Now
                };

                context.Messages.Add(message);
                context.SaveChanges();
            }

            using (var context = new MessengerDbContext(_options))
            {
                var retrievedMessage = context.Messages.FirstOrDefault(m => m.SenderId == 1);
                Assert.NotNull(retrievedMessage);
                Assert.Equal("Test Message", retrievedMessage.EncryptedContent);
            }
        }

        [Fact]
        public void CanUpdateMessage_InDatabase()
        {
            using (var context = new MessengerDbContext(_options))
            {
                var message = new Message
                {
                    SenderId = 1,
                    ReceiverId = 2,
                    EncryptedContent = "Test Message",
                    SentAt = DateTime.Now
                };

                context.Messages.Add(message);
                context.SaveChanges();
            }

            using (var context = new MessengerDbContext(_options))
            {
                var messageToUpdate = context.Messages.FirstOrDefault(m => m.SenderId == 1);
                if (messageToUpdate != null)
                {
                    messageToUpdate.EncryptedContent = "Updated Message";
                    context.SaveChanges();
                }
            }

            using (var context = new MessengerDbContext(_options))
            {
                var updatedMessage = context.Messages.FirstOrDefault(m => m.SenderId == 1);
                Assert.NotNull(updatedMessage);
                Assert.Equal("Updated Message", updatedMessage.EncryptedContent);
            }
        }

        [Fact]
        public void CanDeleteMessage_FromDatabase()
        {
            using (var context = new MessengerDbContext(_options))
            {
                var message = new Message
                {
                    SenderId = 1,
                    ReceiverId = 2,
                    EncryptedContent = "Test Message",
                    SentAt = DateTime.Now
                };

                context.Messages.Add(message);
                context.SaveChanges();
            }

            using (var context = new MessengerDbContext(_options))
            {
                var messageToDelete = context.Messages.FirstOrDefault(m => m.SenderId == 1);
                if (messageToDelete != null)
                {
                    context.Messages.Remove(messageToDelete);
                    context.SaveChanges();
                }
            }

            using (var context = new MessengerDbContext(_options))
            {
                var deletedMessage = context.Messages.FirstOrDefault(m => m.SenderId == 1);
                Assert.Null(deletedMessage); 
            }
        }
    }
}
