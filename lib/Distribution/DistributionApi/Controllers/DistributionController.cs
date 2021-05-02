using DistributionCycleRunner;
using Microsoft.AspNetCore.Mvc;

namespace Distributionapi.Controllers
{
    [Route("api/[controller]")] // url: 
    [ApiController]
    public class DistributionController : ControllerBase
    {
        [HttpPost] // https://localhost:44339/api/Distribution
        public DistributionEventOutcomeOutput[] GetNumberOfCombinations([FromBody] DistributionEventOutcomeInput[] events)
        {
            return DistributionCycleRun.ComputeNumOfCombinations(events);
        }

    }
}
