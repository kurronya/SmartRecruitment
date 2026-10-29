using Microsoft.AspNetCore.Mvc;
using SmartRecruitment.Application.UseCases.Candidate.InterviewTransitions;

namespace SmartRecruitment.API.Controllers
{
    // Service-to-service endpoints. NOT routed by the API Gateway.
    [ApiController]
    [Route("internal/candidates")]
    public class InternalCandidatesController : ControllerBase
    {
        private readonly CandidateInterviewTransitionHandler _handler;

        public InternalCandidatesController(CandidateInterviewTransitionHandler handler)
        {
            this._handler = handler;
        }

        [HttpPost("{id}/enter-interview")]
        public Task<IActionResult> EnterInterview(Guid id)
            => this.RunAsync(() => this._handler.EnterInterviewAsync(id));

        [HttpPost("{id}/mark-employed")]
        public Task<IActionResult> MarkEmployed(Guid id)
            => this.RunAsync(() => this._handler.MarkAsEmployedAsync(id));

        [HttpPost("{id}/return-to-unemployed")]
        public Task<IActionResult> ReturnToUnemployed(Guid id)
            => this.RunAsync(() => this._handler.ReturnToUnemployedAsync(id));

        private async Task<IActionResult> RunAsync(Func<Task> action)
        {
            try
            {
                await action();
                return this.NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return this.NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                // e.g. candidate is not unemployed, so cannot enter an interview
                return this.Conflict(new { message = ex.Message });
            }
        }
    }
}
