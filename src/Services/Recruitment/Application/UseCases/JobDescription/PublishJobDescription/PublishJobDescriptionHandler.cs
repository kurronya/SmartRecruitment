using SmartRecruitment.Application.Interfaces;

namespace SmartRecruitment.Application.UseCases.JobDescription.PublishJobDescription
{
    public class PublishJobDescriptionHandler
    {
        private readonly IJobDescriptionRepository _jobDescriptionRepository;
        private readonly IEmbeddingService _embeddingService;

        public PublishJobDescriptionHandler(
            IJobDescriptionRepository jobDescriptionRepository,
            IEmbeddingService embeddingService)
        {
            this._jobDescriptionRepository = jobDescriptionRepository;
            this._embeddingService = embeddingService;
        }

        public async Task HandleAsync(PublishJobDescriptionCommand command)
        {
            var jobDescription = await this._jobDescriptionRepository.GetByIdAsync(command.JobDescriptionId);
            if (jobDescription == null)
                throw new KeyNotFoundException("Job description not found.");

            var embedding = await this._embeddingService.GenerateEmbeddingAsync(jobDescription.Requirements);
            jobDescription.SetEmbedding(embedding);

            jobDescription.Publish();

            await this._jobDescriptionRepository.UpdateAsync(jobDescription);
        }
    }
}