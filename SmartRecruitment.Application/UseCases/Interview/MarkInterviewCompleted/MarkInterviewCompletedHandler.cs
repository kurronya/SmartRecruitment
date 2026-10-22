using SmartRecruitment.Application.Interfaces;

namespace SmartRecruitment.Application.UseCases.Interview.MarkInterviewCompleted
{
    public class MarkInterviewCompletedHandler
    {
        private readonly IInterviewRepository _interviewRepository;

        public MarkInterviewCompletedHandler(IInterviewRepository interviewRepository)
        {
            this._interviewRepository = interviewRepository;
        }

        public async Task HandleAsync(MarkInterviewCompletedCommand command)
        {
            var interview = await this._interviewRepository.GetByIdAsync(command.InterviewId);
            if (interview == null)
                throw new KeyNotFoundException("Interview not found.");

            // Entity enforces that only a scheduled interview can be marked as completed
            interview.MarkAsCompleted();

            await this._interviewRepository.UpdateAsync(interview);
        }
    }
}