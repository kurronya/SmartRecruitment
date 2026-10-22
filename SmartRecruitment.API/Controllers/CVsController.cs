using Microsoft.AspNetCore.Mvc;
using SmartRecruitment.API.Models;
using SmartRecruitment.Application.UseCases.CV.FinalizeCV;
using SmartRecruitment.Application.UseCases.CV.GenerateCV;
using SmartRecruitment.Application.UseCases.CV.UpdateCVContent;
using SmartRecruitment.Application.UseCases.CV.UploadCV;

namespace SmartRecruitment.API.Controllers
{
    [ApiController]
    [Route("api/cvs")]
    public class CVsController : ControllerBase
    {
        private readonly UploadCVHandler _uploadHandler;
        private readonly GenerateCVHandler _generateHandler;
        private readonly UpdateCVContentHandler _updateContentHandler;
        private readonly FinalizeCVHandler _finalizeHandler;

        public CVsController(
            UploadCVHandler uploadHandler,
            GenerateCVHandler generateHandler,
            UpdateCVContentHandler updateContentHandler,
            FinalizeCVHandler finalizeHandler)
        {
            this._uploadHandler = uploadHandler;
            this._generateHandler = generateHandler;
            this._updateContentHandler = updateContentHandler;
            this._finalizeHandler = finalizeHandler;
        }

        // Candidate uploads a CV file (PDF/DOCX) - multipart/form-data, not JSON
        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] UploadCVRequest request)
        {
            if (request.File == null || request.File.Length == 0)
                return this.BadRequest(new { message = "A file must be provided." });

            using var memoryStream = new MemoryStream();
            await request.File.CopyToAsync(memoryStream);
            var fileContent = memoryStream.ToArray();

            try
            {
                var command = new UploadCVCommand(request.CandidateId, fileContent, request.File.FileName);
                var cvId = await this._uploadHandler.HandleAsync(command);

                return this.CreatedAtAction(nameof(Upload), new { id = cvId }, new { id = cvId });
            }
            catch (KeyNotFoundException ex)
            {
                return this.NotFound(new { message = ex.Message });
            }
        }

        // Candidate requests an AI-generated CV built from their own profile
        [HttpPost("generate")]
        public async Task<IActionResult> Generate([FromBody] GenerateCVRequest request)
        {
            try
            {
                var command = new GenerateCVCommand(request.CandidateId);
                var cvId = await this._generateHandler.HandleAsync(command);

                return this.CreatedAtAction(nameof(Generate), new { id = cvId }, new { id = cvId });
            }
            catch (KeyNotFoundException ex)
            {
                return this.NotFound(new { message = ex.Message });
            }
        }

        // Candidate edits the content while the CV is still a draft
        [HttpPut("{id}/content")]
        public async Task<IActionResult> UpdateContent(Guid id, [FromBody] UpdateCVContentRequest request)
        {
            try
            {
                await this._updateContentHandler.HandleAsync(new UpdateCVContentCommand(id, request.NewContent));
                return this.NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return this.NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                // e.g. CV is already finalized
                return this.Conflict(new { message = ex.Message });
            }
        }

        // Locks the CV content and generates its embedding, making it eligible for matching
        [HttpPatch("{id}/finalize")]
        public async Task<IActionResult> Finalize(Guid id)
        {
            try
            {
                await this._finalizeHandler.HandleAsync(new FinalizeCVCommand(id));
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