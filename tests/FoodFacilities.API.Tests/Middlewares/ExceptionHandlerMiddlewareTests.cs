using FoodFacilitiesAPI.Middlewares;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace FoodFacilities.API.Tests.Middlewares;

[TestClass]
public class ExceptionHandlerMiddlewareTests
{
    private ExceptionHandlerMiddleware _exceptionHandlerMiddleware;
    private RequestDelegate _requestDelegate;
    private ILogger<ExceptionHandlerMiddleware> _logger;

    [TestInitialize]
    public void InitializeTest()
    {
        _logger = Substitute.For<ILogger<ExceptionHandlerMiddleware>>();
        _requestDelegate = Substitute.For<RequestDelegate>();
        _exceptionHandlerMiddleware = new ExceptionHandlerMiddleware(_requestDelegate, _logger);
    }

    [TestMethod, TestCategory(nameof(ExceptionHandlerMiddleware.InvokeAsync))]
    public async Task InvokeAsync_InvokeShouldNotSendErrorIfNoException()
    {
        // Arrange
        var httpContext = Substitute.For<HttpContext>();
        httpContext.Response.Body = new MemoryStream();
        _requestDelegate(httpContext).Returns(Task.CompletedTask);
        
        // Act
        await _exceptionHandlerMiddleware.InvokeAsync(httpContext);
        
        // Assert
        _logger.ReceivedWithAnyArgs(0).LogError(string.Empty);
    }
    
    [TestMethod, TestCategory(nameof(ExceptionHandlerMiddleware.InvokeAsync))]
    public async Task InvokeAsync_InvokeShouldSendErrorIfExceptionThrown()
    {
        // Arrange
        var httpContext = Substitute.For<HttpContext>();
        httpContext.Response.Body = new MemoryStream();
        _requestDelegate(httpContext).Returns((_) => throw new Exception());
        
        // Act
        await _exceptionHandlerMiddleware.InvokeAsync(httpContext);
        
        // Assert
        _logger.ReceivedWithAnyArgs(1).LogError(string.Empty);
    }
    
    [TestMethod, TestCategory(nameof(ExceptionHandlerMiddleware.InvokeAsync))]
    public async Task InvokeAsync_InvokeShouldSendErrorIfArgumentExceptionThrown()
    {
        // Arrange
        var httpContext = Substitute.For<HttpContext>();
        httpContext.Response.Body = new MemoryStream();
        _requestDelegate(httpContext).Returns((_) => throw new ArgumentException());
        
        // Act
        await _exceptionHandlerMiddleware.InvokeAsync(httpContext);
        
        // Assert
        _logger.ReceivedWithAnyArgs(1).LogError(string.Empty);
    }
}