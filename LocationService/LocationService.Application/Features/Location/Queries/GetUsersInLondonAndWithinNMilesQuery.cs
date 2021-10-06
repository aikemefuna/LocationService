using LocationService.Application.Interfaces;
using LocationService.Application.Settings;
using LocationService.Application.Wrappers;
using LocationService.Location.Application.DTOs;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LocationService.Application.Features.Location.Queries
{
    public class GetUsersInLondonAndWithinNMilesQuery : IRequest<Response<List<UserLocationDTO>>>
    {

    }
    public class GetUsersInLondonAndWithinNMilesQueryHandler : IRequestHandler<GetUsersInLondonAndWithinNMilesQuery, Response<List<UserLocationDTO>>>
    {
        private readonly ILocationRepository _locationRepository;
        private readonly ExternalServiceSetting _externalServiceSettings;

        public GetUsersInLondonAndWithinNMilesQueryHandler(ILocationRepository locationRepository, IOptions<ExternalServiceSetting> externalServiceSettings)
        {
            _locationRepository = locationRepository;
            _externalServiceSettings = externalServiceSettings.Value;
        }
        public async Task<Response<List<UserLocationDTO>>> Handle(GetUsersInLondonAndWithinNMilesQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<List<UserLocationDTO>>();

            try
            {
                response.Succeeded = true;
                response.Message = $"Users who are listed as either living in {_externalServiceSettings.Location} retrieved or whose current coordinates are within 50 miles of {_externalServiceSettings.Location}";
                var usersResponse = await _locationRepository.GetUsersByLocation(_externalServiceSettings.Location);
                response.Data = usersResponse;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return response;
        }
    }
}
