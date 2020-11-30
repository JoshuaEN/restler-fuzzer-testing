using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Server.Authentication
{
    public class TimestampAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public const string AuthenticationScheme = "Timestamp";
        public const string TimestampFormat = "yyyy-MM-ddT HH-mm-ss.FFFFFFZ";
        public static readonly IFormatProvider TimestampFormatProvider = CultureInfo.InvariantCulture;
        public TimestampAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (this.Request?.Headers?.ContainsKey("Authentication") != true)
            {
                return AuthenticateResult.NoResult();
            }

            var authHeaders = this.Request.Headers["Authentication"];

            if (authHeaders.Count == 0 || authHeaders.Count > 1)
            {
                return AuthenticateResult.NoResult();
            }

            var authHeader = authHeaders.First();

            if (authHeader.StartsWith($"{AuthenticationScheme} ") != true)
            {
                return AuthenticateResult.NoResult();
            }

            if (authHeader.Length < $"{AuthenticationScheme} ".Length + 1)
            {
                return AuthenticateResult.Fail($"Auth header contained no value");
            }

            var authValue = authHeader.Substring($"{AuthenticationScheme} ".Length);

            var decodedAuthValue = Encoding.UTF8.GetString(Convert.FromBase64String(authValue));
            if (DateTime.TryParseExact(decodedAuthValue, TimestampFormat, TimestampFormatProvider, DateTimeStyles.AssumeUniversal, out DateTime result))
            {
                result = result.ToUniversalTime();
                var now = DateTime.UtcNow;
                if (now < result)
                {
                    var principal = new System.Security.Claims.ClaimsPrincipal();
                    principal.AddIdentity(new System.Security.Claims.ClaimsIdentity(new[] { new Claim(ClaimTypes.Role, "Authed") }));
                    return AuthenticateResult.Success(new AuthenticationTicket(principal, TimestampAuthenticationHandler.AuthenticationScheme));
                }
                return AuthenticateResult.Fail($"Auth token expired at {result} (it is currently {now} on the server)");
            }

            return AuthenticateResult.Fail($"Failed to parse authentication token value");
        }
    }
}
