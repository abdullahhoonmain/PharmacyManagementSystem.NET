using global::PharmacyManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using PharmacyManagementSystem.Services;
namespace PharmacyManagementSystem.Controllers
{
    

    
        [ApiController]
        [Route("api/[controller]")]
        public class AIController : ControllerBase
        {
            private readonly IMedicineAnalysisService _analysisService;

            public AIController(IMedicineAnalysisService analysisService)
            {
                _analysisService = analysisService;
            }

            [HttpPost("analyze")]
            public async Task<IActionResult> Analyze([FromBody] QueryRequest request)
            {
                var response = await _analysisService.AnalyzeMedicineAsync(request.Query);
                return Ok(new { answer = response });
            }

            [HttpGet("badges")]
            public async Task<IActionResult> GetBadges()
            {
                var badges = await _analysisService.GetMedicineBadgesAsync();
                return Ok(badges);
            }
        }

        public class QueryRequest
        {
            public string Query { get; set; }
        }
    

}
