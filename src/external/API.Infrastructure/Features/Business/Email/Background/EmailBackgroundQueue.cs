using API.Domain.Common.Models.Messages;
using System.Threading.Channels;

namespace API.Infrastructure.Features.Business.Email.Background
{
    public record EmailWorkItem(string To, EmailMessage Message);

    public interface IEmailQueue
    {
        ValueTask QueueBackgroundEmailAsync(string to, EmailMessage message);
        IAsyncEnumerable<EmailWorkItem> ReadAllAsync(CancellationToken cancellationToken);
    }

    internal class EmailQueue : IEmailQueue
    {
        private readonly Channel<EmailWorkItem> _channel;

        public EmailQueue()
        {
            var options = new BoundedChannelOptions(1000)
            {
                FullMode = BoundedChannelFullMode.Wait
            };
            _channel = Channel.CreateBounded<EmailWorkItem>(options);
        }

        public async ValueTask QueueBackgroundEmailAsync(string to, EmailMessage message)
        {
            await _channel.Writer.WriteAsync(new EmailWorkItem(to, message));
        }

        public IAsyncEnumerable<EmailWorkItem> ReadAllAsync(CancellationToken cancellationToken)
        {
            return _channel.Reader.ReadAllAsync(cancellationToken);
        }
    }
}
