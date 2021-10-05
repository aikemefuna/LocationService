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
    public class GetUsersInLondAndWithinNMilesQuery : IRequest<Response<List<UserLocationDTO>>>
    {

    }
    public class GetUsersInLondAndWithinNMilesQueryHandler : IRequestHandler<GetUsersInLondAndWithinNMilesQuery, Response<List<UserLocationDTO>>>
    {
        private readonly ILocationRepository _locationRepository;
        private readonly ExternalServiceSetting _externalServiceSettings;

        public GetUsersInLondAndWithinNMilesQueryHandler(ILocationRepository locationRepository, IOptions<ExternalServiceSetting> externalServiceSettings)
        {
            _locationRepository = locationRepository;
            _externalServiceSettings = externalServiceSettings.Value;
        }
        public async Task<Response<List<UserLocationDTO>>> Handle(GetUsersInLondAndWithinNMilesQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<List<UserLocationDTO>>();
            try
            {
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
