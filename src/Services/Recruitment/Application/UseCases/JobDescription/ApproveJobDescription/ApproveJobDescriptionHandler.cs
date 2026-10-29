using SmartRecruitment.Application.Interfaces;

namespace SmartRecruitment.Application.UseCases.JobDescription.ApproveJobDescription
{
    public class ApproveJobDescriptionHandler
    {
        private readonly IJobDescriptionRepository _jobDescriptionRepository;

        public ApproveJobDescriptionHandler(IJobDescriptionRepository jobDescriptionRepository)
        {
            this._jobDescriptionRepository = jobDescriptionRepository;
        }

        public async Task HandleAsync(ApproveJobDescriptionCommand command)
        {
            var jobDescription = await this._jobDescriptionRepository.GetByIdAsync(command.JobDescriptionId);
            if (jobDescription == null)
                throw new KeyNotFoundException("Job description not found.");

            // Entity enforces that only a job description under review can be approved
            jobDescription.Approve();

            await this._jobDescriptionRepository.UpdateAsync(jobDescription);
        }
    }
}