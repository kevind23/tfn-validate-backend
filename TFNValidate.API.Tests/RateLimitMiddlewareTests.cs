using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Moq;
using System.Text.Json;
using System.Threading;
using NUnit.Framework;
using TFNValidate.API.Middleware;
using TFNValidate.API.Models;
using TFNValidate.Services;

namespace TFNValidate.API.Tests
{
    public class RateLimitMiddlewareTests
    {
        [Test]
        public async Task TestInvokeAsync_shouldDenyRequest()
        {
            var taxFileNumber = "1234";
            var clientIpAddress = "192.168.1.1";
            var expectedResponse = new ResultDTO("Too many similar attempts. Please try again later.");
            var expectedJson = JsonSerializer.Serialize(expectedResponse);

            var httpContext = new DefaultHttpContext();
            httpContext.Connection.RemoteIpAddress = IPAddress.Parse(clientIpAddress);
            httpContext.Request.RouteValues["taxFileNumber"] = taxFileNumber;
            httpContext.Response.Body = new MemoryStream();

            var rateLimiterMock = new Mock<IRateLimiter>();
            rateLimiterMock.Setup(p => p.ShouldDenyRequest(taxFileNumber, clientIpAddress)).ReturnsAsync(true);

            async Task OnSuccess(HttpContext httpContext)
            {
                await httpContext.Response.WriteAsync("success");
            }

            var middleware = new RateLimitMiddleware(OnSuccess);
            await middleware.InvokeAsync(httpContext, rateLimiterMock.Object);

            Assert.That(httpContext.Response.Headers.ContainsKey("content-type"));
            Assert.That(httpContext.Response.Headers["content-type"], Is.EqualTo("application/json"));
            httpContext.Response.Body.Position = 0;
            using (var streamReader = new StreamReader(httpContext.Response.Body))
            {
                var actualResponse = await streamReader.ReadToEndAsync();
                Assert.AreEqual(expectedJson, actualResponse);
            }

            rateLimiterMock.VerifyAll();
        }

        [Test]
        public async Task TestInvokeAsync_shouldAllowRequest()
        {
            var taxFileNumber = "1234";
            var clientIpAddress = "192.168.1.1";
            var expectedResponseBody = "success";

            var httpContext = new DefaultHttpContext();
            httpContext.Connection.RemoteIpAddress = IPAddress.Parse(clientIpAddress);
            httpContext.Request.RouteValues["taxFileNumber"] = taxFileNumber;
            httpContext.Response.Body = new MemoryStream();

            var rateLimiterMock = new Mock<IRateLimiter>();
            rateLimiterMock.Setup(p => p.ShouldDenyRequest(taxFileNumber, clientIpAddress)).ReturnsAsync(false);

            async Task OnSuccess(HttpContext httpContext)
            {
                await httpContext.Response.WriteAsync(expectedResponseBody);
            }

            var middleware = new RateLimitMiddleware(OnSuccess);
            await middleware.InvokeAsync(httpContext, rateLimiterMock.Object);

            httpContext.Response.Body.Position = 0;
            using (var streamReader = new StreamReader(httpContext.Response.Body))
            {
                var actualResponse = await streamReader.ReadToEndAsync();
                Assert.AreEqual(expectedResponseBody, actualResponse);
            }

            rateLimiterMock.VerifyAll();
        }

        [Test]
        public async Task TestInvokeAsync_requestDoesNotHaveTaxFileNumber_shouldAllowRequest()
        {
            string taxFileNumber = null;
            var clientIpAddress = "192.168.1.1";
            var expectedResponseBody = "success";

            var httpContext = new DefaultHttpContext();
            httpContext.Connection.RemoteIpAddress = IPAddress.Parse(clientIpAddress);
            httpContext.Request.RouteValues["taxFileNumber"] = taxFileNumber;
            httpContext.Response.Body = new MemoryStream();

            var rateLimiterMock = new Mock<IRateLimiter>();

            async Task OnSuccess(HttpContext httpContext)
            {
                await httpContext.Response.WriteAsync(expectedResponseBody);
            }

            var middleware = new RateLimitMiddleware(OnSuccess);
            await middleware.InvokeAsync(httpContext, rateLimiterMock.Object);

            httpContext.Response.Body.Position = 0;
            using (var streamReader = new StreamReader(httpContext.Response.Body))
            {
                var actualResponse = await streamReader.ReadToEndAsync();
                Assert.AreEqual(expectedResponseBody, actualResponse);
            }

            rateLimiterMock.VerifyAll();
        }
    }
}
