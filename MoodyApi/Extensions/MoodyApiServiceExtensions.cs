#nullable enable
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MoodyApi.Configuration;
using MoodyApi.Core;
using MoodyApi.Models;
using MoodyApi.Providers;
using MoodyApi.Providers.Interfaces;

namespace MoodyApi.Extensions
{
    /// <summary>
    /// Service collection extensions for registering MoodyApi services.
    /// </summary>
    public static class MoodyApiServiceExtensions
    {
        /// <summary>
        /// Registers MoodyApi services with the dependency injection container.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configureOptions">Optional configuration action.</param>
        /// <returns>The service collection for chaining.</returns>
        public static IServiceCollection AddMoodyApi(this IServiceCollection services, Action<MoodOptions>? configureOptions = null)
        {
            // Register configuration
            var options = new MoodOptions();
            configureOptions?.Invoke(options);

            // Validate options
            if (options.KarmaThreshold < 1)
            {
                throw new ArgumentException("KarmaThreshold must be greater than 0.", nameof(options.KarmaThreshold));
            }

            services.AddSingleton(options);

            // Register provider dictionary - FIX: Proper provider registration
            services.AddSingleton<Dictionary<MoodType, IMessageProvider>>(provider =>
            {
                var logger = provider.GetService<ILogger<MoodEngine>>();
                return new Dictionary<MoodType, IMessageProvider>
                {
                    [MoodType.Neutral] = new NeutralProvider(logger),
                    [MoodType.Motivational] = new MotivationProvider(logger),
                    [MoodType.Sarcastic] = new SarcasmProvider(logger),
                    [MoodType.KarmaBased] = new KarmaProvider(logger),
                    [MoodType.TimeBased] = new TimeBasedProvider(logger),
                    [MoodType.ErrorBased] = new ErrorBasedProvider(logger)
                };
            });

            // Register core services
            services.AddSingleton<UserTracker>();
            services.AddSingleton<MoodEngine>();

            return services;
        }
    }
}