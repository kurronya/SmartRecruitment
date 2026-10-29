namespace SmartRecruitment.Application.UseCases.Interview.RecordInterviewResult
{
    public class RecordInterviewResultCommand
    {
        public Guid InterviewId { get; set; }
        public bool Passed { get; set; }

        public RecordInterviewResultCommand(Guid interviewId, bool passed)
        {
            this.InterviewId = interviewId;
            this.Passed = passed;
        }
    }
}