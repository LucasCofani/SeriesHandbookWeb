using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SeriesHandbookSPA.Authentication
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private ILocalStorageService _localStorage;

        public CustomAuthenticationStateProvider(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;

        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();
            var token = await _localStorage.GetItemAsync<string>("tokenJWT");
            if (token != null)
            {
                identity = new ClaimsIdentity(ParseJWTClaims(token), "jwt");
            }



            return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity)));
        }

        public async Task MarkUserAsAuthenticated()
        {
            var identity = new ClaimsIdentity();
            var token = await _localStorage.GetItemAsync<string>("tokenJWT");
            if (token != null)
            {
                identity = new ClaimsIdentity(ParseJWTClaims(token), "jwt");
            }
            NotifyAuthenticationStateChanged(
                Task.FromResult(
                    new AuthenticationState(new ClaimsPrincipal(identity))
            ));
        }

        public async Task MarkUserAsLoggedOut()
        {
            var identity = new ClaimsIdentity();

            await _localStorage.RemoveItemAsync("tokenJWT");

            NotifyAuthenticationStateChanged(
                Task.FromResult(
                    new AuthenticationState(new ClaimsPrincipal(identity))
                ));
        }
        private List<Claim> ParseJWTClaims(string tokenString)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(tokenString);
            return token.Claims.ToList();
        }
    }
}
