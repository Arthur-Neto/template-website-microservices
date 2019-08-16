using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Template.Application.FeatureExampleModule;
using Template.Application.FeatureExampleModule.Models.Commands;

namespace Template.Api.Controllers
{
    [ApiController]
    [Route("api/feature-example")]
    public class FeatureExampleController : ControllerBase
    {
        private IFeatureExampleAppService _featureExampleAppService;

        public FeatureExampleController(
            IFeatureExampleAppService featureExampleAppService)
        {
            _featureExampleAppService = featureExampleAppService;
        }

        [HttpGet]
        public async Task<IActionResult> RetrieveAllAsync()
        {
            return Ok(await _featureExampleAppService.RetrieveAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> RetrieveByIDAsync(int id)
        {
            return Ok(await _featureExampleAppService.RetrieveByIDAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(FeatureExampleAddCommand command)
        {
            return Ok(await _featureExampleAppService.AddAsync(command));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync(FeatureExampleUpdateCommand command)
        {
            return Ok(await _featureExampleAppService.UpdateAsync(command));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveAsync(int id)
        {
            return Ok(await _featureExampleAppService.RemoveAsync(id));
        }
    }
}
