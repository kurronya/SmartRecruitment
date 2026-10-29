namespace SmartRecruitment.Application.UseCases.JobDescription.ApproveJobDescription
{
    public class ApproveJobDescriptionCommand
    {
        public Guid JobDescriptionId { get; set; }

        public ApproveJobDescriptionCommand(Guid jobDescriptionId)
        {
            this.JobDescriptionId = jobDescriptionId;
        }
    }
}