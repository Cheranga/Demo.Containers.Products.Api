using Demo.Containers.Products.Api.Shared;

namespace Demo.Containers.Products.Api.Infrastructure.DataAccess;

public interface IQuery
{
    string CorrelationId { get; set; }
}