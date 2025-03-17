﻿using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace WebLab2.HelperMethods;

public class AuthHelper
{
    private readonly IJSRuntime _jsRuntime;

    public AuthHelper(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task<bool> IsUserAdminAsync()
    {
        var token = await GetAccessTokenAsync();

        if (string.IsNullOrEmpty(token))
        {
            return false;
        }

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

        if (jwtToken is null)
        {
            return false;
        }

        var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

        return roleClaim?.Value == "Admin";
    }

    private async Task<string?> GetAccessTokenAsync()
    {
        return await _jsRuntime.InvokeAsync<string>("getCookie", "AccessToken");
    }
}
