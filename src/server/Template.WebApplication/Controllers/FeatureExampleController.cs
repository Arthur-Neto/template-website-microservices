using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Template.Application.FeatureExampleModule;

namespace Template.WebApplication.Controllers
{
    [ApiController]
    [Route("api/feature-example")]
    public class FeatureExampleController : ControllerBase
    {
        private Lazy<IFeatureExampleAppService> _featureExampleAppService;

        public FeatureExampleController(
            Lazy<IFeatureExampleAppService> featureExampleAppService)
        {
            _featureExampleAppService = featureExampleAppService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _featureExampleAppService.Value.RetrieveAllAsync());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok("value");
        }

        [HttpPost]
        public IActionResult Post([FromBody] string value)
        {
            return Ok(true);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] string value)
        {
            return Ok(true);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return Ok(true);
        }
    }
}
