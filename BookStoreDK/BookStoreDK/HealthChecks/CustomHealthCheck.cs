using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BookStoreDK.HealthChecks
{
    public class CustomHealthCheck : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            return HealthCheckResult.Healthy("Customer Health Check is OK");
        }
    }
}
