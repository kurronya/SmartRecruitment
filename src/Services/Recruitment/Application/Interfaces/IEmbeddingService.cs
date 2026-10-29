namespace SmartRecruitment.Application.Interfaces
{
    // Implemented in Infrastructure by an adapter that calls the sentence-transformers embedding model
    public interface IEmbeddingService
    {
        Task<float[]> GenerateEmbeddingAsync(string text);
    }
}