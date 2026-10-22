namespace SmartRecruitment.Application.DTOs
{
    // Represents one ranked candidate result from the matching engine
    public class CandidateMatchResult
    {
        public Guid CandidateId { get; set; }
        public Guid CVId { get; set; }
        public float SimilarityScore { get; set; }

        public CandidateMatchResult(Guid candidateId, Guid cvId, float similarityScore)
        {
            this.CandidateId = candidateId;
            this.CVId = cvId;
            this.SimilarityScore = similarityScore;
        }
    }
}