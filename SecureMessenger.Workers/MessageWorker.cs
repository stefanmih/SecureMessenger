using MediatR;
using SecureMessenger.Application.Messages.Queries;

namespace SecureMessenger.Workers
{
    public class MessageWorker : IHostedService, IDisposable
    {
        private readonly IMediator _mediator;
        private Timer _timer;
        private int _intervalInMilliseconds = 5000;
        private Action _updateUI;

        public MessageWorker(IMediator mediator, Action updateUI)
        {
            _mediator = mediator;
            _updateUI = updateUI;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(LoadMessages, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(_intervalInMilliseconds));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        private async void LoadMessages(object state)
        {
            int? loggedInUserId = 1;

            if (loggedInUserId == null) return;

            var query = new GetMessagesQuery { ReceiverId = loggedInUserId.Value };
            var messages = await _mediator.Send(query);

            _updateUI?.Invoke();
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
