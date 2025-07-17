#nullable enable
using System;
using Microsoft.Extensions.DependencyInjection;
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
            services.AddSingleton(options);

            // Register providers
            services.AddSingleton<IMessageProvider, NeutralProvider>();
            services.AddSingleton<IMessageProvider, MotivationProvider>();
            services.AddSingleton<IMessageProvider, SarcasmProvider>();
            services.AddSingleton<IMessageProvider, KarmaProvider>();
            services.AddSingleton<IMessageProvider, TimeBasedProvider>();
            services.AddSingleton<IMessageProvider, ErrorBasedProvider>();

            // Register provider dictionary
            services.AddSingleton<Dictionary<MoodType, IMessageProvider>>(provider =>
            {
                return new Dictionary<MoodType, IMessageProvider>
                {
                    [MoodType.Neutral] = new NeutralProvider(),
                    [MoodType.Motivational] = new MotivationProvider(),
                    [MoodType.Sarcastic] = new SarcasmProvider(),
                    [MoodType.KarmaBased] = new KarmaProvider(),
                    [MoodType.TimeBased] = new TimeBasedProvider(),
                    [MoodType.ErrorBased] = new ErrorBasedProvider()
                };
            });

            // Register core services
            services.AddSingleton<UserTracker>();
            services.AddSingleton<MoodEngine>();

            return services;
        }
    }
}