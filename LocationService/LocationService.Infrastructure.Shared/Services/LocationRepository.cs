namespace LocationService.Infrastructure.Shared.Services
{
    using LocationService.Application.Interfaces;
    using LocationService.Application.Settings;
    using LocationService.Location.Application.DTOs;
    using Microsoft.Extensions.Options;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="LocationRepository" />.
    /// </summary>
    public class LocationRepository : ILocationRepository
    {
        /// <summary>
        /// Defines the _httpClientService.
        /// </summary>
        private readonly IHttpClientService _httpClientService;

        /// <summary>
        /// Defines the _externalServiceSettings.
        /// </summary>
        private readonly ExternalServiceSetting _externalServiceSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationRepository"/> class.
        /// </summary>
        /// <param name="httpClientService">The httpClientService<see cref="IHttpClientService"/>.</param>
        /// <param name="externalServiceSettings">The externalServiceSettings<see cref="IOptions{ExternalServiceSetting}"/>.</param>
        public LocationRepository(IHttpClientService httpClientService,
                                  IOptions<ExternalServiceSetting> externalServiceSettings)
        {
            _httpClientService = httpClientService;
            _externalServiceSettings = externalServiceSettings.Value;
        }

        /// <summary>
        /// Base Method to get users in and within a location
        /// </summary>
        /// <param name="location">The location<see cref="string"/>.</param>
        /// <returns>The list of  <see cref="Task{List{UserLocationDTO}}"/>.</returns>
        public async Task<List<UserLocationDTO>> GetUsersByLocation(string location)
        {
            var response = new List<UserLocationDTO>();
            //get users listed in london from the external service
            var usersInLondon = await GetUsersInLondon(location);
            var usersWithinNRangeOfMilesFromLondon = await GetUsersWithinNMilesLondon(usersInLondon);
            response.AddRange(usersInLondon);
            response.AddRange(usersWithinNRangeOfMilesFromLondon);
            return response;
        }

        /// <summary>
        /// Makes a GET request to the external service consumed.
        /// </summary>
        /// <returns>Deserialized List of <see cref="Task{List{UserLocationDTO}}"/>.</returns>
        private async Task<List<UserLocationDTO>> GetUsersFromExternalSource()
        {
            var users = new List<UserLocationDTO>();
            try
            {
                string url = $"{_externalServiceSettings.BaseUrl}{_externalServiceSettings.GetAllUsers}";
                var result = await _httpClientService.GetAsync<List<UserLocationDTO>>(url);
                if (result != null)
                    users = result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return users;
        }

        /// <summary>
        /// Makes a GET request to an endpoint on the external service.
        /// </summary>
        /// <param name="location">The location<see cref="string"/>.</param>
        /// <returns>Deserialized List of <see cref="Task{List{UserLocationDTO}}"/>.</returns>
        private async Task<List<UserLocationDTO>> GetUsersInLondon(string location)
        {
            var users = new List<UserLocationDTO>();
            try
            {
                var endpoint = string.Format(_externalServiceSettings.GetUsersByCity, location);
                string url = $"{_externalServiceSettings.BaseUrl}{endpoint}";
                var result = await _httpClientService.GetAsync<List<UserLocationDTO>>(url);
                if (result != null)
                    users = result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return users;
        }

        /// <summary>
        /// Get users with 'N'miles. The miles is configurable on the 'app-settings.json' file
        /// </summary>
        /// <param name="usersMarkedAsInLondon">The usersMarkedAsInLondon<see cref="List{UserLocationDTO}"/>.</param>
        /// <returns>The <see cref="Task{List{UserLocationDTO}}"/>.</returns>
        private async Task<List<UserLocationDTO>> GetUsersWithinNMilesLondon(List<UserLocationDTO> usersMarkedAsInLondon)
        {
            var users = new List<UserLocationDTO>();
            try
            {
                var allUsers = await GetUsersFromExternalSource();
                var otherUsers = allUsers
                    .Where(c => usersMarkedAsInLondon
                    .All(k => k.id != c.id) &&
                    CalculateDistance(_externalServiceSettings.LatitudeOfLondon,
                                      double.Parse(c.latitude),
                                      _externalServiceSettings.LongitudeOfLondon,
                                      double.Parse(c.longitude))
                    <= _externalServiceSettings.MilesAverage);
                users.AddRange(otherUsers);
                //foreach (var user in otherUsers)
                //{
                //    users.Add(user);
                //    //if (CalculateDistance(_externalServiceSettings.LatitudeOfLondon, double.Parse(user.latitude), _externalServiceSettings.LongitudeOfLondon, double.Parse(user.longitude)) <= _externalServiceSettings.MilesAverage)
                //    //{
                //    //    users.Add(user);
                //    //}
                //}

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return users;
        }

        /// <summary>
        /// Converts from Degree to Radian.
        /// </summary>
        /// <param name="degree">The degree<see cref="double"/>.</param>
        /// <returns>The Radian  <see cref="double"/> .</returns>
        private double ToRadian(double degree)
        {
            var radiansOverDegrees = (Math.PI / 180.0);

            var radian = degree * radiansOverDegrees;
            return radian;
        }

        /// <summary>
        /// The Calculates the distance between two points in miles.
        /// </summary>
        /// <param name="pointALatitude">The pointALatitude<see cref="double"/>.</param>
        /// <param name="pointBLatitude">The pointBLatitude<see cref="double"/>.</param>
        /// <param name="pointALongitude">The pointALongitude<see cref="double"/>.</param>
        /// <param name="pointBLongitude">The pointBLongitude<see cref="double"/>.</param>
        /// <returns>The <see cref="double"/>.</returns>
        private double CalculateDistance(double pointALatitude, double pointBLatitude, double pointALongitude, double pointBLongitude)
        {
            pointALatitude = ToRadian(pointALatitude);
            pointBLatitude = ToRadian(pointBLatitude);
            pointALongitude = ToRadian(pointALongitude);
            pointBLongitude = ToRadian(pointBLongitude);

            // using the Haversine formula
            double result = CalculateUsingHaversineFormular(pointALatitude, pointBLatitude, pointALongitude, pointBLongitude);

            double angle = 2 * Math.Asin(Math.Sqrt(result));

            // Radius in miles
            double radiusInMiles = 3956;

            return (angle * radiusInMiles);
        }

        /// <summary>
        ///  Calculate Using Haversine Formular. The haversine formula determines the great-circle distance between two points on a sphere given their longitudes and latitudes.
        /// </summary>
        /// <param name="pointALatitude">The pointALatitude<see cref="double"/>.</param>
        /// <param name="pointBLatitude">The pointBLatitude<see cref="double"/>.</param>
        /// <param name="pointALongitude">The pointALongitude<see cref="double"/>.</param>
        /// <param name="pointBLongitude">The pointBLongitude<see cref="double"/>.</param>
        /// <returns>The <see cref="double"/>.</returns>
        private double CalculateUsingHaversineFormular(double pointALatitude, double pointBLatitude, double pointALongitude, double pointBLongitude)
        {
            double baseLongitude = pointBLongitude - pointALongitude;
            double baseLatitude = pointALatitude - pointBLatitude;

            double result = Math.Pow(Math.Sin(baseLatitude / 2), 2) + Math.Cos(pointALatitude) * Math.Cos(pointBLatitude) * Math.Pow(Math.Sin(baseLongitude / 2), 2);
            return result;
        }
    }
}
