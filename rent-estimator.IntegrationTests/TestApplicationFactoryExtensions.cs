using System;
using Microsoft.AspNetCore.Hosting;
using Xunit.Abstractions;
using Microsoft.Extensions.Configuration;
using System.IO;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

internal static class TestApplicationFactoryExtensions
{
    public static void WithDefaultBuilderSettings(this IWebHostBuilder builder, ITestOutputHelper outputHelper)
    {
        builder.UseSetting("AnyOverrideSetting", "value");

        Environment.SetEnvironmentVariable("DB_PASSWORD", "Password13579999");

        builder.ConfigureAppConfiguration((context, configBuilder) =>
        {
            configBuilder.SetBasePath(Directory.GetCurrentDirectory());
            configBuilder.AddJsonFile("appsettings.Integration.json", optional: false, reloadOnChange: false);
        });
    }
}