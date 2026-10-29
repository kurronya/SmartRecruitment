using SmartRecruitment.Domain.Enums;

namespace SmartRecruitment.Domain.Entities
{
    public class CV
    {
        public Guid Id;
        public DateTime CreatedAt;

        // Foreign key: a candidate can have multiple CVs.
        public Guid CandidateId;

        private string _content;
        private CVStatus _status;
        private CVSource _source;
        private float[]? _embedding;
        private DateTime? _finalizedAt;

        // CV content: "Uploaded" refers to text extracted from the file, while "AiGenerated" refers to text written by AI.
        public string Content
        {
            get { return this._content; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("CV content cannot be empty.");
                this._content = value.Trim();
            }
        }

        public CVStatus Status
        {
            get { return this._status; }
            private set { this._status = value; }
        }

        public CVSource Source
        {
            get { return this._source; }
            private set { this._source = value; }
        }

        // Vector embedding used for semantic matching against JobDescription.Requirements
        // Nullable because it is generated only after the content is finalized and processed by the embedding model
        public float[]? Embedding
        {
            get { return this._embedding; }
            private set { this._embedding = value; }
        }

        public DateTime? FinalizedAt
        {
            get { return this._finalizedAt; }
            private set { this._finalizedAt = value; }
        }

        // Parameterless constructor for EF Core
        public CV()
        {
            this.Id = Guid.NewGuid();
            this.CreatedAt = DateTime.UtcNow;
            this.Status = CVStatus.Draft;
        }

        // The full constructor is used when creating a new CV; regardless of the source being Uploaded or AiGenerated, it always starts in Draft
        public CV(Guid candidateId, string content, CVSource source) : this()
        {
            this.CandidateId = candidateId;
            this.Content = content;
            this.Source = source;
        }

        // Business rule: candidate can edit the content while it is in Draft status
        public void UpdateContent(string newContent)
        {
            if (this.Status != CVStatus.Draft)
                throw new InvalidOperationException("Only draft CVs can be edited.");

            this.Content = newContent;
        }

        // Business rule: finalize the content, no further edits allowed this is the version used for applying/matching
        public void Finalize()
        {
            if (this.Status != CVStatus.Draft)
                throw new InvalidOperationException("Only draft CVs can be finalized.");

            this.Status = CVStatus.Finalized;
            this.FinalizedAt = DateTime.UtcNow;
        }

        // Business rule: set the embedding after calling the AI model only applicable for finalized CVs
        public void SetEmbedding(float[] embedding)
        {
            if (this.Status != CVStatus.Finalized)
                throw new InvalidOperationException("Embedding can only be set for finalized CVs.");

            if (embedding == null || embedding.Length == 0)
                throw new ArgumentException("Embedding vector cannot be empty.");

            this.Embedding = embedding;
        }
    }
}