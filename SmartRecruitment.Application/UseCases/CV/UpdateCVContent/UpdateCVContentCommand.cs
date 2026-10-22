namespace SmartRecruitment.Application.UseCases.CV.UpdateCVContent
{
    public class UpdateCVContentCommand
    {
        public Guid CVId { get; set; }
        public string NewContent { get; set; }

        public UpdateCVContentCommand(Guid cvId, string newContent)
        {
            this.CVId = cvId;
            this.NewContent = newContent;
        }
    }
}