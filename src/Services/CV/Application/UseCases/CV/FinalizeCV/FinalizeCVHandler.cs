using SmartRecruitment.Application.Interfaces;

namespace SmartRecruitment.Application.UseCases.CV.FinalizeCV
{
    public class FinalizeCVHandler
    {
        private readonly ICVRepository _cvRepository;
        private readonly IEmbeddingService _embeddingService;

        public FinalizeCVHandler(ICVRepository cvRepository, IEmbeddingService embeddingService)
        {
            this._cvRepository = cvRepository;
            this._embeddingService = embeddingService;
        }

        public async Task HandleAsync(FinalizeCVCommand command)
        {
            var cv = await this._cvRepository.GetByIdAsync(command.CVId);
            if (cv == null)
                throw new KeyNotFoundException("CV not found.");

            // Entity enforces that only a draft CV can be finalized
            cv.Finalize();

            // Generate the embedding only after finalization, since the entity
            // only allows setting it on a finalized CV (avoids embedding a draft that may still change)
            var embedding = await this._embeddingService.GenerateEmbeddingAsync(cv.Content);
            cv.SetEmbedding(embedding);

            await this._cvRepository.UpdateAsync(cv);
        }
    }
}