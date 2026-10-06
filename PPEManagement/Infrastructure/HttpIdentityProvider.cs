using PPEManagement.Common;

namespace PPEManagement.Infrastructure;

public class HttpIdentityProvider : IIdentityProvider
{
    private readonly IHttpContextAccessor accessor;

    public HttpIdentityProvider(IHttpContextAccessor accessor) => this.accessor = accessor;

    public string Name
    {
        get
        {
            var name = accessor.HttpContext?.User.Identity?.Name;
            return string.IsNullOrWhiteSpace(name) ? "system" : name;
        }
    }
}