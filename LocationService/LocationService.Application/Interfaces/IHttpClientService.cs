namespace LocationService.Application.Interfaces
{
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IHttpClientService" />.
    /// </summary>
    public interface IHttpClientService
    {
        /// <summary>
        /// Http client handler - Delete.
        /// </summary>
        /// <param name="uri">.</param>
        /// <param name="authToken">.</param>
        /// <returns>.</returns>
        Task DeleteAsync(string uri, string authToken = "");

        /// <summary>
        /// Http client handler - Get.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="uri">.</param>
        /// <param name="authToken">.</param>
        /// <param name="branchId">The branchId<see cref="string"/>.</param>
        /// <returns>.</returns>
        Task<T> GetAsync<T>(string uri, string authToken = "");

        /// <summary>
        /// Http client handler - Post. Specifiy the base data and the class to deserialize json into.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <typeparam name="TR">.</typeparam>
        /// <param name="uri">.</param>
        /// <param name="data">The data<see cref="T"/>.</param>
        /// <param name="authToken">.</param>
        /// <returns>.</returns>
        Task<TR> PostAsync<T, TR>(string uri, T data, string authToken = "");

        /// <summary>
        /// Http client handler - Post. Specifiy the base data.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="uri">.</param>
        /// <param name="data">The data<see cref="T"/>.</param>
        /// <param name="authToken">.</param>
        /// <returns>.</returns>
        Task<T> PostAsync<T>(string uri, T data, string authToken = "");

        /// <summary>
        /// Http client handler - Put. Specifiy the base data.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="uri">.</param>
        /// <param name="data">The data<see cref="T"/>.</param>
        /// <param name="authToken">.</param>
        /// <returns>The <see cref="Task{T}"/>.</returns>
        Task<T> PutAsync<T>(string uri, T data, string authToken = "");

        /// <summary>
        /// The PutAsync.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <typeparam name="TR">.</typeparam>
        /// <param name="uri">The uri<see cref="string"/>.</param>
        /// <param name="data">The data<see cref="T"/>.</param>
        /// <param name="authToken">The authToken<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{TR}"/>.</returns>
        Task<TR> PutAsync<T, TR>(string uri, T data, string authToken = "");
    }
}
