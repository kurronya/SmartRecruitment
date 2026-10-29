using SmartRecruitment.Application.Interfaces;

namespace SmartRecruitment.Application.UseCases.Candidate.CreateCandidate
{
    public class CreateCandidateHandler
    {
        private readonly ICandidateRepository _candidateRepository;

        public CreateCandidateHandler(ICandidateRepository candidateRepository)
        {
            this._candidateRepository = candidateRepository;
        }

        public async Task<Guid> HandleAsync(CreateCandidateCommand command)
        {
            // Business rule: Prevent registration with a duplicate email address.
            var existingCandidate = await this._candidateRepository.GetByEmailAsync(command.Email);
            if (existingCandidate != null)
                throw new InvalidOperationException("A candidate with this email already exists.");

            var candidate = new Domain.Entities.Candidate(
                command.FullName,
                command.Email,
                command.PhoneNumber
            );

            await this._candidateRepository.AddAsync(candidate);

            return candidate.Id;
        }
    }
}