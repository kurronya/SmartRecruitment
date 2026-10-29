using Microsoft.AspNetCore.Mvc;
using SmartRecruitment.API.Models;
using SmartRecruitment.Application.UseCases.Interview.CancelInterview;
using SmartRecruitment.Application.UseCases.Interview.MarkInterviewCompleted;
using SmartRecruitment.Application.UseCases.Interview.RecordInterviewResult;
using SmartRecruitment.Application.UseCases.Interview.ScheduleInterview;

namespace SmartRecruitment.API.Controllers
{
    [ApiController]
    [Route("api/interviews")]
    public class InterviewsController : ControllerBase
    {
        private readonly ScheduleInterviewHandler _scheduleHandler;
        private readonly MarkInterviewCompletedHandler _markCompletedHandler;
        private readonly RecordInterviewResultHandler _recordResultHandler;
        private readonly CancelInterviewHandler _cancelHandler;

        public InterviewsController(
            ScheduleInterviewHandler scheduleHandler,
            MarkInterviewCompletedHandler markCompletedHandler,
            RecordInterviewResultHandler recordResultHandler,
            CancelInterviewHandler cancelHandler)
        {
            this._scheduleHandler = scheduleHandler;
            this._markCompletedHandler = markCompletedHandler;
            this._recordResultHandler = recordResultHandler;
            this._cancelHandler = cancelHandler;
        }

        // Company action invites a candidate to interview
        [HttpPost]
        public async Task<IActionResult> Schedule([FromBody] ScheduleInterviewRequest request)
        {
            try
            {
                var command = new ScheduleInterviewCommand(request.CandidateId, request.JobDescriptionId, request.ScheduledAt);
                var interviewId = await this._scheduleHandler.HandleAsync(command);

                return this.CreatedAtAction(nameof(Schedule), new { id = interviewId }, new { id = interviewId });
            }
            catch (KeyNotFoundException ex)
            {
                return this.NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                // e.g. candidate is currently interviewing elsewhere, or not unemployed
                return this.Conflict(new { message = ex.Message });
            }
        }

        // Company action marks that the interview has taken place
        [HttpPatch("{id}/complete")]
        public async Task<IActionResult> MarkCompleted(Guid id)
        {
            try
            {
                await this._markCompletedHandler.HandleAsync(new MarkInterviewCompletedCommand(id));
                return this.NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return this.NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return this.Conflict(new { message = ex.Message });
            }
        }

        // Company only action records the interview outcome, cascading the candidate's status update
        [HttpPatch("{id}/result")]
        public async Task<IActionResult> RecordResult(Guid id, [FromBody] RecordInterviewResultRequest request)
        {
            try
            {
                var command = new RecordInterviewResultCommand(id, request.Passed);
                await this._recordResultHandler.HandleAsync(command);

                return this.NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return this.NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return this.Conflict(new { message = ex.Message });
            }
        }

        // Either the company or the candidate can cancel, while the interview is still scheduled
        [HttpPatch("{id}/cancel")]
        public async Task<IActionResult> Cancel(Guid id, [FromBody] CancelInterviewRequest request)
        {
            try
            {
                await this._cancelHandler.HandleAsync(new CancelInterviewCommand(id, request.Reason));
                return this.NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return this.NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return this.Conflict(new { message = ex.Message });
            }
        }
    }
}