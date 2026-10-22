using Microsoft.AspNetCore.Mvc;
using SmartRecruitment.API.Models;
using SmartRecruitment.Application.UseCases.Company.ApproveCompany;
using SmartRecruitment.Application.UseCases.Company.RegisterCompany;
using SmartRecruitment.Application.UseCases.Company.RejectCompany;

namespace SmartRecruitment.API.Controllers
{
    [ApiController]
    [Route("api/companies")]
    public class CompaniesController : ControllerBase
    {
        private readonly RegisterCompanyHandler _registerCompanyHandler;
        private readonly ApproveCompanyHandler _approveCompanyHandler;
        private readonly RejectCompanyHandler _rejectCompanyHandler;

        public CompaniesController(
            RegisterCompanyHandler registerCompanyHandler,
            ApproveCompanyHandler approveCompanyHandler,
            RejectCompanyHandler rejectCompanyHandler)
        {
            this._registerCompanyHandler = registerCompanyHandler;
            this._approveCompanyHandler = approveCompanyHandler;
            this._rejectCompanyHandler = rejectCompanyHandler;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterCompanyRequest request)
        {
            try
            {
                var command = new RegisterCompanyCommand(request.Name, request.Email, request.BusinessRegistrationNumber);
                var companyId = await this._registerCompanyHandler.HandleAsync(command);

                return this.CreatedAtAction(nameof(Register), new { id = companyId }, new { id = companyId });
            }
            catch (InvalidOperationException ex)
            {
                // e.g. duplicate email
                return this.Conflict(new { message = ex.Message });
            }
        }

        // Admin only action in practice authorization will be added once JWT/roles are wired up
        [HttpPatch("{id}/approve")]
        public async Task<IActionResult> Approve(Guid id)
        {
            try
            {
                var command = new ApproveCompanyCommand(id);
                await this._approveCompanyHandler.HandleAsync(command);

                return this.NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return this.NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                // e.g. company is not in Pending status
                return this.Conflict(new { message = ex.Message });
            }
        }

        // Admin only action in practice authorization will be added once JWT/roles are wired up
        [HttpPatch("{id}/reject")]
        public async Task<IActionResult> Reject(Guid id, [FromBody] RejectCompanyRequest request)
        {
            try
            {
                var command = new RejectCompanyCommand(id, request.Reason);
                await this._rejectCompanyHandler.HandleAsync(command);

                return this.NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return this.NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                // e.g. company is not Pending, or reason is empty
                return this.Conflict(new { message = ex.Message });
            }
        }
    }
}