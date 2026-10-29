using Microsoft.AspNetCore.Mvc;
using SmartRecruitment.API.Models;
using SmartRecruitment.Application.UseCases.JobDescription.ApproveJobDescription;
using SmartRecruitment.Application.UseCases.JobDescription.PublishJobDescription;
using SmartRecruitment.Application.UseCases.JobDescription.RejectJobDescription;
using SmartRecruitment.Application.UseCases.JobDescription.StartJobDescriptionReview;
using SmartRecruitment.Application.UseCases.JobDescription.SubmitJobDescription;

namespace SmartRecruitment.API.Controllers
{
    [ApiController]
    [Route("api/job-descriptions")]
    public class JobDescriptionsController : ControllerBase
    {
        private readonly SubmitJobDescriptionHandler _submitHandler;
        private readonly StartJobDescriptionReviewHandler _startReviewHandler;
        private readonly ApproveJobDescriptionHandler _approveHandler;
        private readonly RejectJobDescriptionHandler _rejectHandler;
        private readonly PublishJobDescriptionHandler _publishHandler;

        public JobDescriptionsController(
            SubmitJobDescriptionHandler submitHandler,
            StartJobDescriptionReviewHandler startReviewHandler,
            ApproveJobDescriptionHandler approveHandler,
            RejectJobDescriptionHandler rejectHandler,
            PublishJobDescriptionHandler publishHandler)
        {
            this._submitHandler = submitHandler;
            this._startReviewHandler = startReviewHandler;
            this._approveHandler = approveHandler;
            this._rejectHandler = rejectHandler;
            this._publishHandler = publishHandler;
        }

        [HttpPost]
        public async Task<IActionResult> Submit([FromBody] SubmitJobDescriptionRequest request)
        {
            try
            {
                var command = new SubmitJobDescriptionCommand(
                    request.CompanyId, request.Title, request.Description, request.Requirements);
                var jobDescriptionId = await this._submitHandler.HandleAsync(command);

                return this.CreatedAtAction(nameof(Submit), new { id = jobDescriptionId }, new { id = jobDescriptionId });
            }
            catch (KeyNotFoundException ex)
            {
                return this.NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                // e.g. company is not verified yet
                return this.Conflict(new { message = ex.Message });
            }
        }

        [HttpPatch("{id}/start-review")]
        public async Task<IActionResult> StartReview(Guid id)
        {
            try
            {
                await this._startReviewHandler.HandleAsync(new StartJobDescriptionReviewCommand(id));
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

        [HttpPatch("{id}/approve")]
        public async Task<IActionResult> Approve(Guid id)
        {
            try
            {
                await this._approveHandler.HandleAsync(new ApproveJobDescriptionCommand(id));
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

        [HttpPatch("{id}/reject")]
        public async Task<IActionResult> Reject(Guid id, [FromBody] RejectJobDescriptionRequest request)
        {
            try
            {
                await this._rejectHandler.HandleAsync(new RejectJobDescriptionCommand(id, request.Reason));
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

        [HttpPatch("{id}/publish")]
        public async Task<IActionResult> Publish(Guid id)
        {
            try
            {
                await this._publishHandler.HandleAsync(new PublishJobDescriptionCommand(id));
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