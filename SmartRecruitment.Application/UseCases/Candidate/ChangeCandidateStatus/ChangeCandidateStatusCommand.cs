using SmartRecruitment.Domain.Enums;

namespace SmartRecruitment.Application.UseCases.Candidate.ChangeCandidateStatus
{
    public class ChangeCandidateStatusCommand
    {
        public Guid CandidateId { get; set; }
        public CandidateStatus NewStatus { get; set; }

        public ChangeCandidateStatusCommand(Guid candidateId, CandidateStatus newStatus)
        {
            this.CandidateId = candidateId;
            this.NewStatus = newStatus;
        }
    }
}