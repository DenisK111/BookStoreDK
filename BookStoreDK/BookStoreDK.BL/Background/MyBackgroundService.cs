using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BookStoreDK.BL.Background
{
    public class MyBackgroundService : BackgroundService
    {

        private readonly ILogger<MyBackgroundService> _logger;
        private Timer? _timer;

        public MyBackgroundService(ILogger<MyBackgroundService> logger)
        {
            _logger = logger;           
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
          _timer = new Timer(DoWork, null, TimeSpan.Zero,
          TimeSpan.FromSeconds(5));
                        
          //  return Task.CompletedTask;
        }

        private void DoWork(object? state)
        {
            _logger.LogError($"Hello from {nameof(MyBackgroundService)} at {DateTime.Now}");
        }
    }
}
