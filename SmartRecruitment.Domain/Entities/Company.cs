using SmartRecruitment.Domain.Enums;

namespace SmartRecruitment.Domain.Entities
{
    public class Company
    {
        public Guid Id;
        public DateTime CreatedAt;

        private string _name;
        private string _email;
        private string _businessRegistrationNumber;
        private CompanyVerificationStatus _verificationStatus;
        private string? _rejectionReason;

        public string Name
        {
            get { return this._name; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Company name cannot be empty.");
                this._name = value.Trim();
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

        // The business registration number is used to verify the company (theo proposal: business document validation)
        public string BusinessRegistrationNumber
        {
            get { return this._businessRegistrationNumber; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Business registration number cannot be empty.");
                this._businessRegistrationNumber = value.Trim();
            }
        }

        public CompanyVerificationStatus VerificationStatus
        {
            get { return this._verificationStatus; }
            private set { this._verificationStatus = value; }
        }

        // Only has a value when rejected, explaining the reason to the company
        public string? RejectionReason
        {
            get { return this._rejectionReason; }
            private set { this._rejectionReason = value; }
        }

        // Parameterless constructor for EF Core
        public Company()
        {
            this.Id = Guid.NewGuid();
            this.CreatedAt = DateTime.UtcNow;
            this.VerificationStatus = CompanyVerificationStatus.Pending;
        }

        // The full constructor is used when a company registers a new account, it always starts in Pending status
        public Company(string name, string email, string businessRegistrationNumber) : this()
        {
            this.Name = name;
            this.Email = email;
            this.BusinessRegistrationNumber = businessRegistrationNumber;
        }

        // Business rule: admin approves the company -> change to Verified
        public void Approve()
        {
            if (this.VerificationStatus != CompanyVerificationStatus.Pending)
                throw new InvalidOperationException("Only pending companies can be approved.");

            this.VerificationStatus = CompanyVerificationStatus.Verified;
            this.RejectionReason = null;
        }

        // Business rule: admin rejects the company -> change to Rejected, a reason must be provided
        public void Reject(string reason)
        {
            if (this.VerificationStatus != CompanyVerificationStatus.Pending)
                throw new InvalidOperationException("Only pending companies can be rejected.");

            if (string.IsNullOrWhiteSpace(reason))
                throw new ArgumentException("Rejection reason must be provided.");

            this.VerificationStatus = CompanyVerificationStatus.Rejected;
            this.RejectionReason = reason.Trim();
        }
    }
}