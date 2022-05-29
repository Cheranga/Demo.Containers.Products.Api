using Demo.Containers.Products.Api.Infrastructure.DataAccess;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Demo.Containers.Products.Api.Features.HealthChecks;

public class ConfigChecks : IHealthCheck
{
    private readonly DatabaseConfig _databaseConfig;

    public ConfigChecks(DatabaseConfig databaseConfig)
    {
        _databaseConfig = databaseConfig;
    }
    
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        var isDatabaseConfigValid = IsDatabaseConfigValid();

        if (!isDatabaseConfigValid)
        {
            return Task.FromResult(HealthCheckResult.Unhealthy("database configurations are not valid"));
        }

        return Task.FromResult(HealthCheckResult.Healthy());
    }

    private bool IsDatabaseConfigValid()
    {
        if (string.IsNullOrEmpty(_databaseConfig?.ConnectionString))
        {
            return false;
        }

        return true;
    }
}