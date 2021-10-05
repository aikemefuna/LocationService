namespace LocationService.Application.Interfaces
{
    using LocationService.Location.Application.DTOs;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="ILocationRepository" />.
    /// </summary>
    public interface ILocationRepository
    {
        /// <summary>
        /// Get Users By Location.
        /// </summary>
        /// <param name="location">The location<see cref="string"/>.</param>
        /// <returns>List of users either in London or within 50 miles of London. The <see cref="Task{List{UserLocationDTO}}"/>.</returns>
        Task<List<UserLocationDTO>> GetUsersByLocation(string location);
    }
}
