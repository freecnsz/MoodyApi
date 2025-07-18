#nullable enable
using MoodyApi.Providers.Interfaces;
using Microsoft.Extensions.Logging;

namespace MoodyApi.Providers
{
    /// <summary>
    /// Base provider class with common functionality.
    /// </summary>
    public abstract class BaseProvider : IMessageProvider
    {
        /// <summary>
        /// Logger instance for logging provider-related information and errors.
        /// </summary>
        protected readonly ILogger? _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseProvider"/> class.
        /// </summary>
        /// <param name="logger">The logger instance to use for logging provider-related information and errors.</param>
        protected BaseProvider(ILogger? logger = null)
        {
            _logger = logger;
        }

        /// <summary>
        /// Gets a message from the provider, handling any exceptions that may occur.
        /// </summary>
        /// <returns>A message string from the provider, or a fallback message if an error occurs.</returns>
        public string GetMessage()
        {
            try
            {
                return GetMessageInternal();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error generating message for {ProviderType}", GetType().Name);
                return "Something went wrong, but we're still here!";
            }
        }

        /// <summary>
        /// When implemented in a derived class, retrieves a message from the provider.
        /// </summary>
        /// <returns>A message string from the provider.</returns>
        protected abstract string GetMessageInternal();
    }
}