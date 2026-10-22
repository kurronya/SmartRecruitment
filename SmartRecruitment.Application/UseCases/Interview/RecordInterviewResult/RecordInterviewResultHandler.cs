using SmartRecruitment.Application.Interfaces;

namespace SmartRecruitment.Application.UseCases.Interview.RecordInterviewResult
{
    public class RecordInterviewResultHandler
    {
        private readonly IInterviewRepository _interviewRepository;
        private readonly ICandidateRepository _candidateRepository;

        public RecordInterviewResultHandler(
            IInterviewRepository interviewRepository,
            ICandidateRepository candidateRepository)
        {
            this._interviewRepository = interviewRepository;
            this._candidateRepository = candidateRepository;
        }

        public async Task HandleAsync(RecordInterviewResultCommand command)
        {
            var interview = await this._interviewRepository.GetByIdAsync(command.InterviewId);
            if (interview == null)
                throw new KeyNotFoundException("Interview not found.");

            var candidate = await this._candidateRepository.GetByIdAsync(interview.CandidateId);
            if (candidate == null)
                throw new KeyNotFoundException("Candidate not found.");

            // This is the coordination point between two entities that the Domain layer
            // deliberately does not know about interview and Candidate stay independent,
            // and only the Application layer decides how a result on one affects the other
            if (command.Passed)
            {
                interview.MarkAsPassed();
                candidate.MarkAsEmployed();
            }
            else
            {
                interview.MarkAsFailed();
                candidate.ReturnToUnemployed();
            }

            await this._interviewRepository.UpdateAsync(interview);
            await this._candidateRepository.UpdateAsync(candidate);
        }
    }
}