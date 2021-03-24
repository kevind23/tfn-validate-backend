using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TFNValidate.API.Models;
using TFNValidate.Services;

namespace TFNValidate.API.Middleware
{
    public class RateLimitMiddleware
    {
        private readonly RequestDelegate _next;

        public RateLimitMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IRateLimiter rateLimiter)
        {
            var taxFileNumber = GetTaxFileNumberFrom(context);
            var clientIpAddress = GetIpAddressFrom(context);
            if (taxFileNumber == null)
            {
                await _next(context);
                return;
            }
            var shouldDenyRequest = await rateLimiter.ShouldDenyRequest(taxFileNumber, clientIpAddress);
            if (shouldDenyRequest)
            {
                await RejectRequest(context, "Too many similar attempts. Please try again later.");
                return;
            }
            await _next(context);
        }

        private string GetTaxFileNumberFrom(HttpContext context)
        {
            return context.GetRouteValue("taxFileNumber") as string;
        }

        private string GetIpAddressFrom(HttpContext context)
        {
            return context.Connection.RemoteIpAddress.ToString();
        }

        private Task RejectRequest(HttpContext context, string error)
        {
            var result = new ResultDTO(error);
            var jsonResult = JsonSerializer.Serialize(result);
            context.Response.Headers.Add("content-type", "application/json");
            return context.Response.WriteAsync(jsonResult);
        }
    }
}