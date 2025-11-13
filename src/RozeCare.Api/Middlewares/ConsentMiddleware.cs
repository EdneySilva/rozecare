using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.IdentityModel.JsonWebTokens;
using RozeCare.Api.Services;
using RozeCare.Application.Common.Interfaces;

namespace RozeCare.Api.Middlewares;

public class ConsentMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ConsentScopeResolver _resolver;

    public ConsentMiddleware(RequestDelegate next, ConsentScopeResolver resolver)
    {
        _next = next;
        _resolver = resolver;
    }

    public async Task InvokeAsync(HttpContext context, IConsentService consentService, IAuditService auditService)
    {
        var endpoint = context.GetEndpoint();
        if (endpoint is null)
        {
            await _next(context);
            return;
        }

        var descriptor = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();
        var controllerName = descriptor?.ControllerName;
        var actionName = descriptor?.ActionName;

        if (controllerName is null || actionName is null)
        {
            await _next(context);
            return;
        }

        var scope = _resolver.Resolve(controllerName + "Controller", actionName);
        if (scope is null)
        {
            await _next(context);
            return;
        }

        var routeData = context.Request.RouteValues;
        if (!routeData.TryGetValue("patientId", out var patientObj) && !routeData.TryGetValue("id", out patientObj))
        {
            await _next(context);
            return;
        }

        if (!Guid.TryParse(Convert.ToString(patientObj), out var patientId))
        {
            await _next(context);
            return;
        }

        var actorIdClaim = context.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? context.User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (!Guid.TryParse(actorIdClaim, out var actorId))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsJsonAsync(new { error = "Consent required" });
            return;
        }

        if (actorId == patientId)
        {
            await auditService.LogAsync(actorId, "consent.self", controllerName, patientId, new { scope, allowed = true }, context.RequestAborted);
            await _next(context);
            return;
        }

        var hasConsent = await consentService.HasConsentAsync(patientId, actorId, scope, context.RequestAborted);
        await auditService.LogAsync(actorId, "consent.check", controllerName, patientId, new { scope, allowed = hasConsent }, context.RequestAborted);
        if (!hasConsent)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsJsonAsync(new { error = "Consent missing" });
            return;
        }

        await _next(context);
    }
}
