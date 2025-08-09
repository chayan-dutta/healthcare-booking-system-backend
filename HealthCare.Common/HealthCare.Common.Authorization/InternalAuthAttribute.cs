using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Text;

namespace HealthCare.Common.Authorization;

/// <summary>
/// Custom authorization filter to allow ONLY internal service-to-service calls.
/// Uses a pre-shared internal JWT with specific issuer/audience.
/// Apply as [InternalAuth] on controllers/actions.
/// 
/*
Why we implement Attribute + IAuthorizationFilter:

1. Attribute:
   - Allows us to decorate controllers or actions with[InternalAuth].
   - This makes the filter reusable and declarative(no manual wiring in middleware).

2. IAuthorizationFilter:
   - Part of ASP.NET Core’s filter pipeline.
   - Runs early in the request lifecycle, before the controller action executes.
   - Perfect place to block unauthorized requests without hitting business logic.

In short:
[InternalAuth] works like a gatekeeper — 
implemented as an attribute for easy usage,
and as IAuthorizationFilter so it plugs directly into ASP.NET Core’s authorization flow.
*/
/// </summary>
public class InternalAuthAttribute : Attribute, IAuthorizationFilter
{
    // This method name and signature are FIXED by IAsyncAuthorizationFilter.
    // ASP.NET Core calls this automatically during the authorization stage.
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // 1️. Get the Authorization header from the incoming HTTP request.
        // Example expected: Authorization: Bearer <token>
        var authHeader = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();

        // 2️. Quick reject if header missing or doesn’t start with Bearer.
        if (authHeader is null || !authHeader.StartsWith("Bearer "))
        {
            context.Result = new UnauthorizedResult(); // 401
            return;
        }

        // 3️. Extract just the token part (skip the "Bearer " prefix).
        var token = authHeader["Bearer ".Length..];

        try
        {
            // 4️. Create a token handler (built-in .NET JWT parser/validator).
            var jwtHandler = new JwtSecurityTokenHandler();

            // 5️. Define validation rules for internal tokens.
            // - Issuer: Who issued the token (must match)
            // - Audience: Who the token is meant for (must match)
            // - Key: Shared secret for internal services
            var validationParams = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = "internal", // must match token's "iss" claim
                ValidAudience = "internal-services", // must match token's "aud" claim
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes("SuperSecretInternalKeyHere")) // shared secret
            };

            // 6️. Actually validate the token (throws exception if invalid).
            jwtHandler.ValidateToken(token, validationParams, out _);

            // If no exception, request continues normally.
        }
        catch
        {
            // Any failure (wrong key, expired, invalid issuer/audience) → reject.
            context.Result = new UnauthorizedResult();
        }
    }
}
