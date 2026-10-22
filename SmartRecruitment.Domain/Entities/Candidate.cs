using SmartRecruitment.Domain.Enums;

namespace SmartRecruitment.Domain.Entities
{
    public class Candidate
    {
        // Public identity fields—no complex validation required.
        public Guid Id;
        public DateTime CreatedAt;

        // Backing fields for properties that require validation
        private string _fullName;
        private string _email;
        private CandidateStatus _status;
        private DateTime _statusChangedAt;

        public string FullName
        {
            get { return this._fullName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Full name cannot be empty.");
                this._fullName = value.Trim();
            }
        }

        public string Email
        {
            get { return this._email; }
            set
            {
                if (string.IsNullOrWhiteSpace(value) || !value.Contains("@"))
                    throw new ArgumentException("Email is invalid.");
                this._email = value.Trim().ToLowerInvariant();
            }
        }

        public string PhoneNumber { get; set; } // not strictly validated -> left flexible

        public CandidateStatus Status
        {
            get { return this._status; }
            private set { this._status = value; }
        }

        // Timestamp of the last status change -> used to enforce the 30-day lock rule
        public DateTime StatusChangedAt
        {
            get { return this._statusChangedAt; }
            private set { this._statusChangedAt = value; }
        }

        // Parameterless constructor for EF Core
        public Candidate()
        {
            this.Id = Guid.NewGuid();
            this.CreatedAt = DateTime.UtcNow;
            this.Status = CandidateStatus.Unemployed;
            this.StatusChangedAt = DateTime.UtcNow;
        }

        // The full constructor is used when creating a new candidate in the use case
        public Candidate(string fullName, string email, string phoneNumber) : this()
        {
            this.FullName = fullName;
            this.Email = email;
            this.PhoneNumber = phoneNumber;
        }

        // Business rule: change status with a 30 days lock rule according to the proposal
        public void ChangeStatus(CandidateStatus newStatus)
        {
            var daysSinceLastChange = (DateTime.UtcNow - this.StatusChangedAt).TotalDays;
            if (daysSinceLastChange < 30)
                throw new InvalidOperationException(
                    "Candidate cannot change status within 30 days of the last change.");

            this.Status = newStatus;
            this.StatusChangedAt = DateTime.UtcNow;
        }

        // Business rule: triggered when a company invites the candidate to an interview
        // this is a system-driven transition, not a self edit, so it bypasses the 30 days lock
        public void EnterInterviewProcess()
        {
            if (this.Status != CandidateStatus.Unemployed)
                throw new InvalidOperationException("Only unemployed candidates can enter an interview process.");

            this.Status = CandidateStatus.Interviewing;
            this.StatusChangedAt = DateTime.UtcNow;
        }

        // Business rule: candidate passed the interview this starts the 30-day lock
        // that prevents the candidate from self editing their status afterward
        public void MarkAsEmployed()
        {
            if (this.Status != CandidateStatus.Interviewing)
                throw new InvalidOperationException("Only interviewing candidates can be marked as employed.");

            this.Status = CandidateStatus.Employed;
            this.StatusChangedAt = DateTime.UtcNow;
        }

        // Business rule: interview failed or was cancelled candidate returns to job seeking,
        // no lock applies since they are not employed
        public void ReturnToUnemployed()
        {
            if (this.Status != CandidateStatus.Interviewing)
                throw new InvalidOperationException("Only interviewing candidates can return to unemployed.");

            this.Status = CandidateStatus.Unemployed;
            this.StatusChangedAt = DateTime.UtcNow;
        }
    }
}