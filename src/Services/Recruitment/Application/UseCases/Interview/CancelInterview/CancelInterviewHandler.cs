using SmartRecruitment.Application.Interfaces;

namespace SmartRecruitment.Application.UseCases.Interview.CancelInterview
{
    public class CancelInterviewHandler
    {
        private readonly IInterviewRepository _interviewRepository;
        private readonly ICandidateGateway _candidateGateway;

        public CancelInterviewHandler(
            IInterviewRepository interviewRepository,
            ICandidateGateway candidateGateway)
        {
            this._interviewRepository = interviewRepository;
            this._candidateGateway = candidateGateway;
        }

        public async Task HandleAsync(CancelInterviewCommand command)
        {
            var interview = await this._interviewRepository.GetByIdAsync(command.InterviewId);
            if (interview == null)
                throw new KeyNotFoundException("Interview not found.");

            // Entity enforces that only a scheduled interview can be cancelled,
            // and that a reason must be provided (by either the company or the candidate).
            // Nothing is saved yet, so a rejected cancellation leaves everything untouched.
            interview.Cancel(command.Reason);

            // Cancelling releases the candidate back to job seeking, same as a failed interview
            await this._candidateGateway.ReturnToUnemployedAsync(interview.CandidateId);

            await this._interviewRepository.UpdateAsync(interview);
        }
    }
}
