using System;
using CommandDotNet;
using CommandDotNet.Attributes;
using CommandDotNet.IoC.MicrosoftDependencyInjection;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Tests
{
    public class IoCTests
    {
        [Fact]
        public void CanMicrosoftInjectService()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IService, Service>();
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            AppRunner<ServiceApp> serviceApp =
                new AppRunner<ServiceApp>().UseMicrosoftDependencyInjection(serviceProvider);
            
            serviceApp.Run("Process").Should().Be(4);
        }
    }
    
    public class ServiceApp
    {
        [InjectProperty]
        public IService Service { get; set; }

        public int Process()
        {
            return Service?.value ?? throw new Exception("Service is not injected");
        }
    }

    public interface IService
    {
        int value { get; set; }
    }

    public class Service : IService
    {
        public int value { get; set; } = 4;
    }
}