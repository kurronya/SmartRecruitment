namespace SmartRecruitment.Application.UseCases.JobDescription.SubmitJobDescription
{
    public class SubmitJobDescriptionCommand
    {
        public Guid CompanyId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Requirements { get; set; }

        public SubmitJobDescriptionCommand(Guid companyId, string title, string description, string requirements)
        {
            this.CompanyId = companyId;
            this.Title = title;
            this.Description = description;
            this.Requirements = requirements;
        }
    }
}