namespace SmartRecruitment.Application.UseCases.Matching.MatchCandidatesForJobDescription
{
    public class MatchCandidatesForJobDescriptionQuery
    {
        public Guid JobDescriptionId { get; set; }
        public int TopN { get; set; }

        public MatchCandidatesForJobDescriptionQuery(Guid jobDescriptionId, int topN)
        {
            this.JobDescriptionId = jobDescriptionId;
            this.TopN = topN;
        }
    }
}