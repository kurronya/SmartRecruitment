using Microsoft.AspNetCore.Mvc;
using SmartRecruitment.Application.UseCases.CV.GetFinalizedCVEmbeddings;

namespace SmartRecruitment.API.Controllers
{
    // Service-to-service endpoints. NOT routed by the API Gateway.
    [ApiController]
    [Route("internal/cvs")]
    public class InternalCVsController : ControllerBase
    {
        private readonly GetFinalizedCVEmbeddingsHandler _handler;

        public InternalCVsController(GetFinalizedCVEmbeddingsHandler handler)
        {
            this._handler = handler;
        }

        [HttpGet("finalized-embeddings")]
        public async Task<IActionResult> GetFinalizedEmbeddings()
        {
            var results = await this._handler.HandleAsync();
            return this.Ok(results);
        }
    }
}
