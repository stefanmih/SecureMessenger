using MediatR;
using Moq;
using SecureMessenger.Application.Messages.Queries;
using SecureMessenger.Domain.Entities;
using SecureMessenger.Workers;

public class MessageWorkerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly MessageWorker _messageWorker;

    public MessageWorkerTests()
    {
        _mediatorMock = new Mock<IMediator>();

        _messageWorker = new MessageWorker(_mediatorMock.Object, () => { });
    }

    [Fact]
    public async Task StartAsync_CallsLoadMessagesEvery5Seconds()
    {
        var cancellationToken = new CancellationToken();

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetMessagesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Message>());

        await _messageWorker.StartAsync(cancellationToken);
        await Task.Delay(6000);

        _mediatorMock.Verify(m => m.Send(It.IsAny<GetMessagesQuery>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
    }

    [Fact]
    public void StopAsync_CancelsTimer()
    {
        var cancellationToken = new CancellationToken();
        _messageWorker.StartAsync(cancellationToken).Wait();
        _messageWorker.StopAsync(cancellationToken);
        
        _mediatorMock.Verify(m => m.Send(It.IsAny<GetMessagesQuery>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
