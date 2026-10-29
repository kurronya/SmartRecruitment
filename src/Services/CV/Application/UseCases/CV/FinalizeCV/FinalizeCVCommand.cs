namespace SmartRecruitment.Application.UseCases.CV.FinalizeCV
{
    public class FinalizeCVCommand
    {
        public Guid CVId { get; set; }

        public FinalizeCVCommand(Guid cvId)
        {
            this.CVId = cvId;
        }
    }
}