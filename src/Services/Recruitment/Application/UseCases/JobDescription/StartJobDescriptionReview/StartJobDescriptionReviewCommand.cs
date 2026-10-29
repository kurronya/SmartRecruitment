namespace SmartRecruitment.Application.UseCases.JobDescription.StartJobDescriptionReview
{
    public class StartJobDescriptionReviewCommand
    {
        public Guid JobDescriptionId { get; set; }

        public StartJobDescriptionReviewCommand(Guid jobDescriptionId)
        {
            this.JobDescriptionId = jobDescriptionId;
        }
    }
}