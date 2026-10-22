namespace SmartRecruitment.Application.UseCases.CV.GenerateCV
{
    public class GenerateCVCommand
    {
        public Guid CandidateId { get; set; }

        public GenerateCVCommand(Guid candidateId)
        {
            this.CandidateId = candidateId;
        }
    }
}