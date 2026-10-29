namespace SmartRecruitment.Application.DTOs
{
    // A finalized CV embedding as returned by the CV Service
    public record CandidateCvEmbedding(Guid CandidateId, Guid CvId, float[]? Embedding);
}
