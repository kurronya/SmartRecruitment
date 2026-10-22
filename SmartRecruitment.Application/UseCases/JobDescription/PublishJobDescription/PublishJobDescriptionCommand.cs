namespace SmartRecruitment.Application.UseCases.JobDescription.PublishJobDescription
{
    public class PublishJobDescriptionCommand
    {
        public Guid JobDescriptionId { get; set; }

        public PublishJobDescriptionCommand(Guid jobDescriptionId)
        {
            this.JobDescriptionId = jobDescriptionId;
        }
    }
}