using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TestTaskKaspelan.Auth.Services.Interfaces;
using TestTaskKaspelan.Common.Contracts;

namespace TestTaskKaspelan.Auth.Services.Services
{
    /// <summary>
    /// Authentication service.
    /// </summary>
    /// <seealso cref="TestTaskKaspelan.Auth.Services.Interfaces.IAuthService" />
    public class AuthService: IAuthService
    {
        private readonly AuthOptions _authOptions;
        private readonly ILogger<AuthService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthService"/> class.
        /// </summary>
        /// <param name="authOptions">The authentication options.</param>
        /// <param name="logger">The logger.</param>
        public AuthService(IOptionsSnapshot<AuthOptions> authOptions, ILogger<AuthService> logger)
        {
            _authOptions = authOptions.Value;
            _logger = logger;
        }

        /// <inheritdoc />
        public string Authenticate(LoginRequest loginRequest)
        {
            // TODO: Should use real check email/password.
            if (loginRequest.Email != "email@email.email" ||
                loginRequest.Password != "password")
            {
                throw new ArgumentException("Invalid email or password");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authOptions.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var securityToken = new JwtSecurityToken(_authOptions.Issuer,
              _authOptions.Issuer,
              new List<Claim>() { new Claim("Role", "test_role") }, // TODO: Need use real claims, and change in Gateway also.
              expires: DateTime.Now.AddMinutes(_authOptions.Expire),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
    }
}
