using SmartRecruitment.Application.Interfaces;

namespace SmartRecruitment.Application.UseCases.Interview.CancelInterview
{
    public class CancelInterviewHandler
    {
        private readonly IInterviewRepository _interviewRepository;
        private readonly ICandidateRepository _candidateRepository;

        public CancelInterviewHandler(
            IInterviewRepository interviewRepository,
            ICandidateRepository candidateRepository)
        {
            this._interviewRepository = interviewRepository;
            this._candidateRepository = candidateRepository;
        }

        public async Task HandleAsync(CancelInterviewCommand command)
        {
            var interview = await this._interviewRepository.GetByIdAsync(command.InterviewId);
            if (interview == null)
                throw new KeyNotFoundException("Interview not found.");

            var candidate = await this._candidateRepository.GetByIdAsync(interview.CandidateId);
            if (candidate == null)
                throw new KeyNotFoundException("Candidate not found.");

            // Entity enforces that only a scheduled interview can be cancelled,
            // and that a reason must be provided (by either the company or the candidate)
            interview.Cancel(command.Reason);

            // Cancelling releases the candidate back to job seeking, same as a failed interview
            candidate.ReturnToUnemployed();

            await this._interviewRepository.UpdateAsync(interview);
            await this._candidateRepository.UpdateAsync(candidate);
        }
    }
}