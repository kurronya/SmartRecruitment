using SmartRecruitment.Application.Interfaces;

namespace SmartRecruitment.Infrastructure.AI
{
    // Temporary placeholder until the real Python/FastAPI embedding service is ready.
    // Returns a fixed size random vector so the matching pipeline is testable end-to-end
    // without a real AI model behind it.
    public class FakeEmbeddingService : IEmbeddingService
    {
        private const int EmbeddingDimensions = 384;

        public Task<float[]> GenerateEmbeddingAsync(string text)
        {
            var random = new Random(text.GetHashCode());
            var embedding = new float[EmbeddingDimensions];

            for (int i = 0; i < EmbeddingDimensions; i++)
            {
                embedding[i] = (float)random.NextDouble();
            }

            return Task.FromResult(embedding);
        }
    }
}