using SmartRecruitment.Application.Interfaces;

namespace SmartRecruitment.Application.UseCases.Interview.RecordInterviewResult
{
    public class RecordInterviewResultHandler
    {
        private readonly IInterviewRepository _interviewRepository;
        private readonly ICandidateGateway _candidateGateway;

        public RecordInterviewResultHandler(
            IInterviewRepository interviewRepository,
            ICandidateGateway candidateGateway)
        {
            this._interviewRepository = interviewRepository;
            this._candidateGateway = candidateGateway;
        }

        public async Task HandleAsync(RecordInterviewResultCommand command)
        {
            var interview = await this._interviewRepository.GetByIdAsync(command.InterviewId);
            if (interview == null)
                throw new KeyNotFoundException("Interview not found.");

            // This is the coordination point between two services: Interview and Candidate stay independent,
            // and only the Application layer decides how a result on one affects the other.
            // The entity change happens in memory first (it validates the transition), then the
            // Candidate Service is called, and only then is the interview saved.
            if (command.Passed)
            {
                interview.MarkAsPassed();
                await this._candidateGateway.MarkAsEmployedAsync(interview.CandidateId);
            }
            else
            {
                interview.MarkAsFailed();
                await this._candidateGateway.ReturnToUnemployedAsync(interview.CandidateId);
            }

            await this._interviewRepository.UpdateAsync(interview);
        }
    }
}
