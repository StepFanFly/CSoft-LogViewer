using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace LogViewer.Infrastructure
{
    public abstract class AbstractActor<T>
    {
        private readonly BufferBlock<T> _mailbox;

        public int QueueCount => _mailbox.Count;

        public abstract int ThreadsCount { get; }

        public AbstractActor()
        {
            _mailbox = new BufferBlock<T>();

            var workers = new List<Task>();

            Task.Run(async () =>
            {
                while (true)
                {
                    while (workers.Count < ThreadsCount)
                    {
                        workers.Add(Handle());
                    }

                    await Task.WhenAny(workers);

                    workers.RemoveAll(s => s.IsCompleted);
                }
            });
        }

        private async Task Handle()
        {
            var message = await _mailbox.ReceiveAsync();
            try
            {
                await HandleMessage(message);
            } catch (Exception ex) 
            {
                _ = HandleError(message, ex);
            }

        }

        public abstract Task HandleMessage(T message);

        public abstract Task HandleError(T message, Exception ex);

        public Task SendAsync(T message) => _mailbox.SendAsync(message);

        public void Stop() => _mailbox.TryReceiveAll(out var _);
    }
}
