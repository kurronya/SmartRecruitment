using SmartRecruitment.Application.Interfaces;

namespace SmartRecruitment.Application.UseCases.JobDescription.RejectJobDescription
{
    public class RejectJobDescriptionHandler
    {
        private readonly IJobDescriptionRepository _jobDescriptionRepository;

        public RejectJobDescriptionHandler(IJobDescriptionRepository jobDescriptionRepository)
        {
            this._jobDescriptionRepository = jobDescriptionRepository;
        }

        public async Task HandleAsync(RejectJobDescriptionCommand command)
        {
            var jobDescription = await this._jobDescriptionRepository.GetByIdAsync(command.JobDescriptionId);
            if (jobDescription == null)
                throw new KeyNotFoundException("Job description not found.");

            // Entity enforces that only a job description under review can be rejected,
            // and that a rejection reason must be provided
            jobDescription.Reject(command.Reason);

            await this._jobDescriptionRepository.UpdateAsync(jobDescription);
        }
    }
}