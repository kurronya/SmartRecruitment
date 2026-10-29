using SmartRecruitment.Application.DTOs;

namespace SmartRecruitment.Application.Interfaces
{
    // Port to the CV Service (read-only)
    public interface ICVGateway
    {
        Task<List<CandidateCvEmbedding>> GetFinalizedEmbeddingsAsync();
    }
}
