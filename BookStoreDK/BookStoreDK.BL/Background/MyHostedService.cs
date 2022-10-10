using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BookStoreDK.BL.Background
{
    public class MyHostedService : IHostedService
    {

        private readonly ILogger<MyHostedService> _logger;
        public MyHostedService(ILogger<MyHostedService> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogError($"Hello from {nameof(MyHostedService)}");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogError($"Stopping {nameof(MyHostedService)}");
            return Task.CompletedTask;
        }
    }
}
