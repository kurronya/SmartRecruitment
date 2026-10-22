namespace SmartRecruitment.API.Models
{
    public class ScheduleInterviewRequest
    {
        public Guid CandidateId { get; set; }
        public Guid JobDescriptionId { get; set; }
        public DateTime ScheduledAt { get; set; }
    }
}