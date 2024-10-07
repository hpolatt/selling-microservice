using System;

namespace Web.ApiGateway.Infrastructure;

public class HttpClientDelegatingHandler: DelegatingHandler
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public HttpClientDelegatingHandler(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var accessToken = httpContextAccessor.HttpContext.Request.Headers["Authorization"];
        if (!string.IsNullOrEmpty(accessToken))
        {
            if (request.Headers.Contains("Authorization")) request.Headers.Remove("Authorization");
            request.Headers.Add("Authorization", new List<string> { accessToken });
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
