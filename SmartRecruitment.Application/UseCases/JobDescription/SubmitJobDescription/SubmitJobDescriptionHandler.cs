using SmartRecruitment.Application.Interfaces;
using SmartRecruitment.Domain.Enums;

namespace SmartRecruitment.Application.UseCases.JobDescription.SubmitJobDescription
{
    public class SubmitJobDescriptionHandler
    {
        private readonly IJobDescriptionRepository _jobDescriptionRepository;
        private readonly ICompanyRepository _companyRepository;

        public SubmitJobDescriptionHandler(
            IJobDescriptionRepository jobDescriptionRepository,
            ICompanyRepository companyRepository)
        {
            this._jobDescriptionRepository = jobDescriptionRepository;
            this._companyRepository = companyRepository;
        }

        public async Task<Guid> HandleAsync(SubmitJobDescriptionCommand command)
        {
            var company = await this._companyRepository.GetByIdAsync(command.CompanyId);
            if (company == null)
                throw new KeyNotFoundException("Company not found.");

            // Business rule: only a verified company can post job descriptions
            if (company.VerificationStatus != CompanyVerificationStatus.Verified)
                throw new InvalidOperationException("Only verified companies can submit job descriptions.");

            var jobDescription = new Domain.Entities.JobDescription(
                command.CompanyId,
                command.Title,
                command.Description,
                command.Requirements
            );

            await this._jobDescriptionRepository.AddAsync(jobDescription);

            return jobDescription.Id;
        }
    }
}