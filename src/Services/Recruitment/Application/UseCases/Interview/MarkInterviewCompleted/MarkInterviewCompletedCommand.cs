namespace SmartRecruitment.Application.UseCases.Interview.MarkInterviewCompleted
{
    public class MarkInterviewCompletedCommand
    {
        public Guid InterviewId { get; set; }

        public MarkInterviewCompletedCommand(Guid interviewId)
        {
            this.InterviewId = interviewId;
        }
    }
}