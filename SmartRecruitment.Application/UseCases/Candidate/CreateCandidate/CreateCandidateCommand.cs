namespace SmartRecruitment.Application.UseCases.Candidate.CreateCandidate
{
    public class CreateCandidateCommand
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public CreateCandidateCommand(string fullName, string email, string phoneNumber)
        {
            this.FullName = fullName;
            this.Email = email;
            this.PhoneNumber = phoneNumber;
        }
    }
}