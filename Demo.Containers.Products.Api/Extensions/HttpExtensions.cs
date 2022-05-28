namespace Demo.Containers.Products.Api.Extensions;

public static class HttpExtensions
{
    public static string GetHeaderValue(this HttpRequest request, string headerName)
    {
        if (string.IsNullOrEmpty(headerName)) return string.Empty;

        var headers = request.Headers;
        if (!headers.Any()) return string.Empty;

        if (headers.TryGetValue(headerName, out var headerValue)) return headerValue;

        return string.Empty;
    }
}