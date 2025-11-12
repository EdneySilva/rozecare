using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using RozeCare.Application.Authentication.Commands;
using Xunit;

namespace RozeCare.Domain.Tests.Application;

public class MediatRConventionsTests
{
    private static readonly Assembly ApplicationAssembly = typeof(LoginCommand).Assembly;

    [Fact]
    public void AllRequestsSpecifyResponseType()
    {
        var invalidRequests = ApplicationAssembly
            .GetTypes()
            .Where(t => t is { IsAbstract: false, IsInterface: false })
            .Where(t => t.GetInterfaces().Any(i => i == typeof(IRequest)))
            .ToArray();

        invalidRequests.Should()
            .BeEmpty("all requests must declare an explicit response type");
    }

    [Fact]
    public void HandlersReturnTasksMatchingTheirResponseTypes()
    {
        var handlerTypes = ApplicationAssembly
            .GetTypes()
            .Where(t => t is { IsAbstract: false, IsInterface: false })
            .Where(t => t.GetInterfaces().Any(i => i.IsGenericType &&
                (i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>) ||
                 i.GetGenericTypeDefinition() == typeof(IRequestHandler<>))))
            .ToArray();

        handlerTypes.Should().NotBeEmpty();

        var handlersMissingResponse = handlerTypes
            .Where(t => t.GetInterfaces().Any(i => i.IsGenericType &&
                i.GetGenericTypeDefinition() == typeof(IRequestHandler<>)))
            .ToArray();

        handlersMissingResponse.Should()
            .BeEmpty("all handlers must specify an explicit response type");

        foreach (var handlerType in handlerTypes)
        {
            var handleMethod = handlerType.GetMethod("Handle");
            handleMethod.Should().NotBeNull();

            var returnType = handleMethod!.ReturnType;
            returnType.IsGenericType.Should().BeTrue("handlers must return Task<TResponse>");
            returnType.GetGenericTypeDefinition().Should().Be(typeof(Task<>),
                "handlers must return Task<TResponse>");

            var actualResponseType = returnType.GetGenericArguments()[0];

            var handlerInterfaces = handlerType.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))
                .ToArray();

            handlerInterfaces.Should().NotBeEmpty();

            foreach (var handlerInterface in handlerInterfaces)
            {
                var expectedResponseType = handlerInterface.GetGenericArguments()[1];
                actualResponseType.Should().Be(expectedResponseType,
                    $"{handlerType.Name} should return Task<{expectedResponseType.Name}>");
            }
        }
    }
}
