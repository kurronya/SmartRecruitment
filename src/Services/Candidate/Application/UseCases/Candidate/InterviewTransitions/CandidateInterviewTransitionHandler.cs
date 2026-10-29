using SmartRecruitment.Application.Interfaces;

namespace SmartRecruitment.Application.UseCases.Candidate.InterviewTransitions
{
    // System-driven status transitions requested by the Recruitment Service.
    // They use the entity methods that bypass the 30-day lock, which is reserved for candidate self-service edits.
    public class CandidateInterviewTransitionHandler
    {
        private readonly ICandidateRepository _candidateRepository;

        public CandidateInterviewTransitionHandler(ICandidateRepository candidateRepository)
        {
            this._candidateRepository = candidateRepository;
        }

        public async Task EnterInterviewAsync(Guid candidateId)
        {
            var candidate = await this.LoadAsync(candidateId);

            // Entity enforces that only an unemployed candidate can enter an interview process
            candidate.EnterInterviewProcess();

            await this._candidateRepository.UpdateAsync(candidate);
        }

        public async Task MarkAsEmployedAsync(Guid candidateId)
        {
            var candidate = await this.LoadAsync(candidateId);

            // Starts the 30-day status lock
            candidate.MarkAsEmployed();

            await this._candidateRepository.UpdateAsync(candidate);
        }

        public async Task ReturnToUnemployedAsync(Guid candidateId)
        {
            var candidate = await this.LoadAsync(candidateId);

            candidate.ReturnToUnemployed();

            await this._candidateRepository.UpdateAsync(candidate);
        }

        private async Task<Domain.Entities.Candidate> LoadAsync(Guid candidateId)
        {
            var candidate = await this._candidateRepository.GetByIdAsync(candidateId);
            if (candidate == null)
                throw new KeyNotFoundException("Candidate not found.");

            return candidate;
        }
    }
}
