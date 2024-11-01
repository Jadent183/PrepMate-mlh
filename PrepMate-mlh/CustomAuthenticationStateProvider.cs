using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Microsoft.JSInterop;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly IUserService _userService;
    private readonly IJSRuntime _jsRuntime;

    public CustomAuthenticationStateProvider(IUserService userService, IJSRuntime jsRuntime)
    {
        _userService = userService;
        _jsRuntime = jsRuntime;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var identity = new ClaimsIdentity();

        try
        {
            var user = await _userService.GetCurrentUserAsync();
            if (user != null)
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email)
                };
                identity = new ClaimsIdentity(claims, "custom");
            }
        }
        catch (InvalidOperationException)
        {
            // This exception is thrown during prerendering. We can safely ignore it.
        }

        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    public void NotifyAuthenticationStateChanged()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}