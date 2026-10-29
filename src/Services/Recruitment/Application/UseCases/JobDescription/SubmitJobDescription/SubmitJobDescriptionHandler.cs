using SmartRecruitment.Application.Interfaces;

namespace SmartRecruitment.Application.UseCases.JobDescription.SubmitJobDescription
{
    public class SubmitJobDescriptionHandler
    {
        private readonly IJobDescriptionRepository _jobDescriptionRepository;
        private readonly ICompanyGateway _companyGateway;

        public SubmitJobDescriptionHandler(
            IJobDescriptionRepository jobDescriptionRepository,
            ICompanyGateway companyGateway)
        {
            this._jobDescriptionRepository = jobDescriptionRepository;
            this._companyGateway = companyGateway;
        }

        public async Task<Guid> HandleAsync(SubmitJobDescriptionCommand command)
        {
            // The Company Service throws KeyNotFoundException when the company does not exist.
            // Business rule: only a verified company can post job descriptions
            var isVerified = await this._companyGateway.IsVerifiedAsync(command.CompanyId);
            if (!isVerified)
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
