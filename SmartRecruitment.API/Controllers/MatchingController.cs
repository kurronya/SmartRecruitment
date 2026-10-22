using Microsoft.AspNetCore.Mvc;
using SmartRecruitment.Application.UseCases.Matching.MatchCandidatesForJobDescription;

namespace SmartRecruitment.API.Controllers
{
    [ApiController]
    [Route("api/matching")]
    public class MatchingController : ControllerBase
    {
        private readonly MatchCandidatesForJobDescriptionHandler _matchHandler;

        public MatchingController(MatchCandidatesForJobDescriptionHandler matchHandler)
        {
            this._matchHandler = matchHandler;
        }

        // Ranks candidates against a published job description by CV-embedding similarity
        [HttpGet("job-descriptions/{jobDescriptionId}/candidates")]
        public async Task<IActionResult> GetMatchingCandidates(Guid jobDescriptionId, [FromQuery] int topN = 10)
        {
            try
            {
                var query = new MatchCandidatesForJobDescriptionQuery(jobDescriptionId, topN);
                var results = await this._matchHandler.HandleAsync(query);

                return this.Ok(results);
            }
            catch (KeyNotFoundException ex)
            {
                return this.NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                // e.g. job description has no embedding yet (not published)
                return this.Conflict(new { message = ex.Message });
            }
        }
    }
}