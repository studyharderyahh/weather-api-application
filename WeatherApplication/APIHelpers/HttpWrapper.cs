using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WeatherApplication.APIHelpers
{
    /// <summary>
    /// Singleton wrapper for HttpClient to ensure a single instance throughout the application.
    /// Implements IDisposable to clean up resources.
    /// </summary>
    public sealed class HttpClientWrapper : IDisposable
    {
        // Static instance of the HttpClientWrapper
        private static volatile HttpClientWrapper? m_instance;
        // Lock object for thread safety
        private static readonly object m_lock = new object();
        // HttpClient instance to be used for making HTTP requests
        private readonly HttpClient m_httpClient;

        /// <summary>
        /// Private constructor to prevent direct instantiation.
        /// Initializes the HttpClient with a timeout setting.
        /// </summary>
        private HttpClientWrapper()
        {
            m_httpClient = new HttpClient();
            m_httpClient.Timeout = TimeSpan.FromSeconds(10); // Set a timeout for the HttpClient
        }

        /// <summary>
        /// Singleton instance property.
        /// Ensures only one instance of HttpClientWrapper is created (thread-safe).
        /// </summary>
        public static HttpClientWrapper Instance
        {
            get
            {
                // Double-check locking to ensure thread safety
                if (m_instance == null)
                {
                    lock (m_lock)
                    {
                        if (m_instance == null)
                        {
                            m_instance = new HttpClientWrapper();
                        }
                    }
                }
                return m_instance;
            }
        }

        /// <summary>
        /// Implements IDisposable to dispose the HttpClient instance.
        /// </summary>
        public void Dispose()
        {
            m_httpClient.Dispose();
        }

        /// <summary>
        /// Exposes the HttpClient instance if direct access is needed.
        /// </summary>
        public HttpClient Client => m_httpClient;

        /// <summary>
        /// Sends an asynchronous GET request to the specified URI.
        /// </summary>
        /// <param name="requestUri">The URI to send the GET request to.</param>
        /// <returns>The HTTP response message.</returns>
        /// <exception cref="HttpClientWrapperException">Thrown when an HTTP request error or timeout occurs.</exception>
        public async Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            try
            {
                // Send GET request asynchronously
                return await m_httpClient.GetAsync(requestUri);
            }
            catch (HttpRequestException ex)
            {
                // Handle HTTP request exceptions and rethrow as a custom exception
                throw new HttpClientWrapperException("An error occurred while sending the HTTP request.", ex);
            }
            catch (TaskCanceledException ex)
            {
                // Handle timeout exceptions and rethrow as a custom exception
                throw new HttpClientWrapperException("The HTTP request timed out.", ex);
            }
        }
    }

    /// <summary>
    /// Custom exception class for HttpClientWrapper errors.
    /// </summary>
    public class HttpClientWrapperException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpClientWrapperException"/> class.
        /// </summary>
        public HttpClientWrapperException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpClientWrapperException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message.</param>
        public HttpClientWrapperException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpClientWrapperException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="innerException">The inner exception that caused this exception.</param>
        public HttpClientWrapperException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
