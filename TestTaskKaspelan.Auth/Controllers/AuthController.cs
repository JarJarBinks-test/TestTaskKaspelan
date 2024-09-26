using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using TestTaskKaspelan.Auth.Services.Interfaces;

namespace TestTaskKaspelan.Auth.Controllers
{
    /// <summary>
    /// Authenticate controller.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="authService">The authentication service.</param>
        /// <param name="logger">The logger.</param>
        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Posts the specified login request.
        /// </summary>
        /// <param name="loginRequest">The login request.</param>
        /// <returns>
        /// JWT token.
        /// </returns>
        [HttpPost("token")]
        public string Post([FromBody] LoginRequest loginRequest)
        {
            return _authService.Authenticate(loginRequest);
        }
    }
}
