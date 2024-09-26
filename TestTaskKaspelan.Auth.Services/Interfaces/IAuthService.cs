using Microsoft.AspNetCore.Identity.Data;

namespace TestTaskKaspelan.Auth.Services.Interfaces
{
    /// <summary>
    /// Authenticate service interface.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Authenticates the specified login request.
        /// </summary>
        /// <param name="loginRequest">The login request.</param>
        /// <returns>
        /// JWT token.
        /// </returns>
        string Authenticate(LoginRequest loginRequest);
    }
}
