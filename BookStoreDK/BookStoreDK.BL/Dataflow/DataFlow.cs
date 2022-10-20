using Microsoft.Extensions.Hosting;

namespace BookStoreDK.BL.Dataflow
{
    public abstract class DataFlow : IDisposable, IHostedService
    {
        public abstract void Dispose();

        public abstract Task StartAsync(CancellationToken cancellationToken);

        public abstract Task StopAsync(CancellationToken cancellationToken);

        protected abstract Task StartDataFlow(CancellationToken cancellationToken);
    }
}
