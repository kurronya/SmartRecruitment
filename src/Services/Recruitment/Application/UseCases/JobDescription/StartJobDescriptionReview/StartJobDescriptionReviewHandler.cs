using SmartRecruitment.Application.Interfaces;

namespace SmartRecruitment.Application.UseCases.JobDescription.StartJobDescriptionReview
{
    public class StartJobDescriptionReviewHandler
    {
        private readonly IJobDescriptionRepository _jobDescriptionRepository;

        public StartJobDescriptionReviewHandler(IJobDescriptionRepository jobDescriptionRepository)
        {
            this._jobDescriptionRepository = jobDescriptionRepository;
        }

        public async Task HandleAsync(StartJobDescriptionReviewCommand command)
        {
            var jobDescription = await this._jobDescriptionRepository.GetByIdAsync(command.JobDescriptionId);
            if (jobDescription == null)
                throw new KeyNotFoundException("Job description not found.");

            // Entity enforces that only a Submitted job description can enter review
            jobDescription.StartReview();

            await this._jobDescriptionRepository.UpdateAsync(jobDescription);
        }
    }
}