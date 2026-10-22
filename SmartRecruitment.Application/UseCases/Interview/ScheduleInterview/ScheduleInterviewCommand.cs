namespace SmartRecruitment.Application.UseCases.Interview.ScheduleInterview
{
    public class ScheduleInterviewCommand
    {
        public Guid CandidateId { get; set; }
        public Guid JobDescriptionId { get; set; }
        public DateTime ScheduledAt { get; set; }

        public ScheduleInterviewCommand(Guid candidateId, Guid jobDescriptionId, DateTime scheduledAt)
        {
            this.CandidateId = candidateId;
            this.JobDescriptionId = jobDescriptionId;
            this.ScheduledAt = scheduledAt;
        }
    }
}