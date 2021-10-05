using LocationService.Application.Features.Location.Queries;
using LocationService.Application.Wrappers;
using LocationService.Location.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocationService.WebApi.Controllers.v1
{
    public class LocationController : BaseApiController
    {
        [HttpGet]
        [Route("get-users")]
        [ProducesResponseType(typeof(Response<List<UserLocationDTO>>), 200)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetUsersInLondAndWithinNMilesQuery() { }));
        }
    }
}
