namespace SmartRecruitment.Application.UseCases.JobDescription.RejectJobDescription
{
    public class RejectJobDescriptionCommand
    {
        public Guid JobDescriptionId { get; set; }
        public string Reason { get; set; }

        public RejectJobDescriptionCommand(Guid jobDescriptionId, string reason)
        {
            this.JobDescriptionId = jobDescriptionId;
            this.Reason = reason;
        }
    }
}