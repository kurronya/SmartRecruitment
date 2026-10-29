namespace SmartRecruitment.Application.UseCases.Interview.CancelInterview
{
    public class CancelInterviewCommand
    {
        public Guid InterviewId { get; set; }
        public string Reason { get; set; }

        public CancelInterviewCommand(Guid interviewId, string reason)
        {
            this.InterviewId = interviewId;
            this.Reason = reason;
        }
    }
}