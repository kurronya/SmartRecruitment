using SmartRecruitment.Domain.Enums;

namespace SmartRecruitment.Domain.Entities
{
    public class Interview
    {
        public Guid Id;
        public DateTime CreatedAt;

        // An association entity linking Candidate and JobDescription.
        public Guid CandidateId;
        public Guid JobDescriptionId;

        private DateTime _scheduledAt;
        private InterviewStatus _status;
        private DateTime? _completedAt;
        private string? _cancellationReason;

        public DateTime ScheduledAt
        {
            get { return this._scheduledAt; }
            set
            {
                if (value < DateTime.UtcNow)
                    throw new ArgumentException("Scheduled time cannot be in the past.");
                this._scheduledAt = value;
            }
        }

        public InterviewStatus Status
        {
            get { return this._status; }
            private set { this._status = value; }
        }

        public DateTime? CompletedAt
        {
            get { return this._completedAt; }
            private set { this._completedAt = value; }
        }

        // Only valid if cancelled.
        public string? CancellationReason
        {
            get { return this._cancellationReason; }
            private set { this._cancellationReason = value; }
        }

        // Parameterless constructor for EF Core
        public Interview()
        {
            this.Id = Guid.NewGuid();
            this.CreatedAt = DateTime.UtcNow;
            this.Status = InterviewStatus.Scheduled;
        }

        // The full constructor is used when a company invites a candidate for an interview.
        public Interview(Guid candidateId, Guid jobDescriptionId, DateTime scheduledAt) : this()
        {
            this.CandidateId = candidateId;
            this.JobDescriptionId = jobDescriptionId;
            this.ScheduledAt = scheduledAt;
        }

        // Business rule: Mark the interview as completed; pending result entry.
        public void MarkAsCompleted()
        {
            if (this.Status != InterviewStatus.Scheduled)
                throw new InvalidOperationException("Only scheduled interviews can be marked as completed.");

            this.Status = InterviewStatus.Completed;
            this.CompletedAt = DateTime.UtcNow;
        }

        // Business rule: "Tick" the company—this action should only be performed after completion.
        public void MarkAsPassed()
        {
            if (this.Status != InterviewStatus.Completed)
                throw new InvalidOperationException("Only completed interviews can be marked as passed.");

            this.Status = InterviewStatus.Passed;
        }

        // Business rule: Company tick dropped – only called after completion.
        public void MarkAsFailed()
        {
            if (this.Status != InterviewStatus.Completed)
                throw new InvalidOperationException("Only completed interviews can be marked as failed.");

            this.Status = InterviewStatus.Failed;
        }

        // Business rule: Cancellations by the company or the candidate are only permitted if the event has not yet taken place.
        public void Cancel(string reason)
        {
            if (this.Status != InterviewStatus.Scheduled)
                throw new InvalidOperationException("Only scheduled interviews can be cancelled.");

            if (string.IsNullOrWhiteSpace(reason))
                throw new ArgumentException("Cancellation reason must be provided.");

            this.Status = InterviewStatus.Cancelled;
            this.CancellationReason = reason.Trim();
        }
    }
}