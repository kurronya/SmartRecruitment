using SmartRecruitment.Domain.Enums;

namespace SmartRecruitment.Domain.Entities
{
    public class JobDescription
    {
        public Guid Id;
        public DateTime CreatedAt;

        //Foreign key to Company, JD always refers to a registered company.
        public Guid CompanyId;

        private string _title;
        private string _description;
        private string _requirements;
        private JobDescriptionStatus _status;
        private string? _rejectionReason;
        private DateTime? _publishedAt;
        private float[]? _embedding;

        // Vector embedding of Requirements, used to compare against CV embeddings during matching
        public float[]? Embedding
        {
            get { return this._embedding; }
            private set { this._embedding = value; }
        }

        // Business rule: embedding can only be set for an approved job description,
        // since Requirements should no longer change once approved
        public void SetEmbedding(float[] embedding)
        {
            if (this.Status != JobDescriptionStatus.Approved && this.Status != JobDescriptionStatus.Published)
                throw new InvalidOperationException("Embedding can only be set for an approved or published job description.");

            if (embedding == null || embedding.Length == 0)
                throw new ArgumentException("Embedding vector cannot be empty.");

            this._embedding = embedding;
        }

        public string Title
        {
            get { return this._title; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Job title cannot be empty.");
                this._title = value.Trim();
            }
        }

        public string Description
        {
            get { return this._description; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Job description cannot be empty.");
                this._description = value.Trim();
            }
        }

        // The requirement details—this is the text that will be converted into embeddings to match against the CV.
        public string Requirements
        {
            get { return this._requirements; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Job requirements cannot be empty.");
                this._requirements = value.Trim();
            }
        }

        public JobDescriptionStatus Status
        {
            get { return this._status; }
            private set { this._status = value; }
        }

        public string? RejectionReason
        {
            get { return this._rejectionReason; }
            private set { this._rejectionReason = value; }
        }

        public DateTime? PublishedAt
        {
            get { return this._publishedAt; }
            private set { this._publishedAt = value; }
        }

        // Constructor empty for EF Core
        public JobDescription()
        {
            this.Id = Guid.NewGuid();
            this.CreatedAt = DateTime.UtcNow;
            this.Status = JobDescriptionStatus.Submitted;
        }

        // Constructor complete, used when the company creates a new JD; it always starts at the "Submitted" status.
        public JobDescription(Guid companyId, string title, string description, string requirements) : this()
        {
            this.CompanyId = companyId;
            this.Title = title;
            this.Description = description;
            this.Requirements = requirements;
        }

        // Business rule: The admin has started reviewing the job description.
        public void StartReview()
        {
            if (this.Status != JobDescriptionStatus.Submitted)
                throw new InvalidOperationException("Only submitted job descriptions can enter review.");

            this.Status = JobDescriptionStatus.UnderReview;
        }

        // Business rule: Admin approves the JD after review.
        public void Approve()
        {
            if (this.Status != JobDescriptionStatus.UnderReview)
                throw new InvalidOperationException("Only job descriptions under review can be approved.");

            this.Status = JobDescriptionStatus.Approved;
            this.RejectionReason = null;
        }

        // Business rule: The admin rejected the JD, a reason is mandatory.
        public void Reject(string reason)
        {
            if (this.Status != JobDescriptionStatus.UnderReview)
                throw new InvalidOperationException("Only job descriptions under review can be rejected.");

            if (string.IsNullOrWhiteSpace(reason))
                throw new ArgumentException("Rejection reason must be provided.");

            this.Status = JobDescriptionStatus.Rejected;
            this.RejectionReason = reason.Trim();
        }

        // Business rule: Only approved JDs may be published publicly.
        public void Publish()
        {
            if (this.Status != JobDescriptionStatus.Approved)
                throw new InvalidOperationException("Only approved job descriptions can be published.");

            this.Status = JobDescriptionStatus.Published;
            this.PublishedAt = DateTime.UtcNow;
        }
    }
}