using Microsoft.AspNetCore.Mvc;
using SmartRecruitment.Application.UseCases.Company.GetCompanyVerificationStatus;

namespace SmartRecruitment.API.Controllers
{
    // Service-to-service endpoints. NOT routed by the API Gateway.
    [ApiController]
    [Route("internal/companies")]
    public class InternalCompaniesController : ControllerBase
    {
        private readonly GetCompanyVerificationStatusHandler _handler;

        public InternalCompaniesController(GetCompanyVerificationStatusHandler handler)
        {
            this._handler = handler;
        }

        [HttpGet("{id}/verification-status")]
        public async Task<IActionResult> GetVerificationStatus(Guid id)
        {
            try
            {
                var status = await this._handler.HandleAsync(id);
                return this.Ok(new { status });
            }
            catch (KeyNotFoundException ex)
            {
                return this.NotFound(new { message = ex.Message });
            }
        }
    }
}
