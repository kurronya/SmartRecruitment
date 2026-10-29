namespace SmartRecruitment.Application.DTOs
{
    // Shape returned by GET /internal/cvs/finalized-embeddings (consumed by the Recruitment Service)
    public record FinalizedCVEmbedding(Guid CandidateId, Guid CvId, float[] Embedding);
}
