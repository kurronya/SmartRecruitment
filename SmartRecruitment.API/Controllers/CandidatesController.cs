using Microsoft.AspNetCore.Mvc;
using SmartRecruitment.API.Models;
using SmartRecruitment.Application.UseCases.Candidate.ChangeCandidateStatus;
using SmartRecruitment.Application.UseCases.Candidate.CreateCandidate;

namespace SmartRecruitment.API.Controllers
{
    [ApiController]
    [Route("api/candidates")]
    public class CandidatesController : ControllerBase
    {
        private readonly CreateCandidateHandler _createCandidateHandler;
        private readonly ChangeCandidateStatusHandler _changeCandidateStatusHandler;

        public CandidatesController(
            CreateCandidateHandler createCandidateHandler,
            ChangeCandidateStatusHandler changeCandidateStatusHandler)
        {
            this._createCandidateHandler = createCandidateHandler;
            this._changeCandidateStatusHandler = changeCandidateStatusHandler;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCandidate([FromBody] CreateCandidateRequest request)
        {
            try
            {
                var command = new CreateCandidateCommand(request.FullName, request.Email, request.PhoneNumber);
                var candidateId = await this._createCandidateHandler.HandleAsync(command);

                return this.CreatedAtAction(nameof(CreateCandidate), new { id = candidateId }, new { id = candidateId });
            }
            catch (InvalidOperationException ex)
            {
                // e.g. duplicate email a client-side error, not a server error
                return this.Conflict(new { message = ex.Message });
            }
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> ChangeStatus(Guid id, [FromBody] ChangeCandidateStatusRequest request)
        {
            try
            {
                var command = new ChangeCandidateStatusCommand(id, request.NewStatus);
                await this._changeCandidateStatusHandler.HandleAsync(command);

                return this.NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return this.NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                // e.g. 30 days lock still active
                return this.Conflict(new { message = ex.Message });
            }
        }
    }
}