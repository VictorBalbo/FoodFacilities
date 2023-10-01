using Dawn;
using FoodFacilities.Models;
using FoodFacilitiesAPI.Managers;
using Microsoft.AspNetCore.Mvc;

namespace FoodFacilitiesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermitController : ControllerBase
    {
        private const string DefaultStatus = "Approved";
        private const int DefaultTake = 5;
        private readonly IPermitManager _permitManager;

        public PermitController(IPermitManager permitManager)
        {
            _permitManager = permitManager;
        }

        /// <summary>
        /// Get a list of <see cref="Permit"/> filtering by Applicant Name and Status
        /// </summary>
        /// <param name="applicantName">Name of the Applicant to be searched</param>
        /// <param name="status">Status to be searched</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("applicants/{applicantName}")]
        [Produces("application/json")]
        public async Task<IAsyncEnumerable<Permit>> GetPermitByApplicantName([FromRoute] string applicantName,
            [FromQuery] string? status, CancellationToken cancellationToken)
        {
            Guard.Argument(applicantName).NotNull().NotEmpty().NotWhiteSpace();
            return await _permitManager.GetByApplicantAsync(applicantName, status, cancellationToken);
        }

        /// <summary>
        /// Get a list of <see cref="Permit"/> filtering by Address
        /// </summary>
        /// <param name="address">Address to be searched</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("address/{address}")]
        [Produces("application/json")]
        public async Task<IAsyncEnumerable<Permit>> GetPermitByAddress([FromRoute] string address,
            CancellationToken cancellationToken)
        {
            Guard.Argument(address).NotNull().NotEmpty().NotWhiteSpace();
            return await _permitManager.GetByAddressAsync(address, cancellationToken);
        }

        /// <summary>
        /// Get a list of <see cref="Permit"/> filtering by Status and ordering by proximity to a specific coordinate
        /// </summary>
        /// <param name="latitude">Latitude of reference</param>
        /// <param name="longitude">Longitude of reference</param>
        /// <param name="cancellationToken"></param>
        /// <param name="status">Status to be filtered</param>
        /// <param name="take">Amount of <see cref="Permit"/> to get</param>
        /// <returns></returns>
        [HttpGet("nearest/{latitude}/{longitude}")]
        [Produces("application/json")]
        public async Task<IAsyncEnumerable<Permit>> GetPermitByNearestName([FromRoute] double latitude,
            [FromRoute] double longitude, CancellationToken cancellationToken,
            [FromQuery] string status = DefaultStatus, [FromQuery] int take = DefaultTake)
        {
            Guard.Argument(latitude, nameof(latitude)).NotNaN().Min(-90).Max(90);
            Guard.Argument(longitude, nameof(longitude)).NotNaN().Min(-180).Max(180);
            Guard.Argument(status, nameof(status)).NotNull().NotEmpty().NotWhiteSpace();
            Guard.Argument(take, nameof(take)).Positive();
            return await _permitManager.GetByNearestAsync(latitude, longitude, status, take, cancellationToken);
        }
    }
}