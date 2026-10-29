using SmartRecruitment.Application.Interfaces;

namespace SmartRecruitment.Application.UseCases.Candidate.ChangeCandidateStatus
{
    public class ChangeCandidateStatusHandler
    {
        private readonly ICandidateRepository _candidateRepository;

        public ChangeCandidateStatusHandler(ICandidateRepository candidateRepository)
        {
            this._candidateRepository = candidateRepository;
        }

        public async Task HandleAsync(ChangeCandidateStatusCommand command)
        {
            var candidate = await this._candidateRepository.GetByIdAsync(command.CandidateId);
            if (candidate == null)
                throw new KeyNotFoundException("Candidate not found.");

            // The business rule (30 day lock) has already been implemented in the entity - Handler just calls it, does not repeat the logic
            candidate.ChangeStatus(command.NewStatus);

            // Note: UpdateAsync is not yet in the interface -> needs to be added in the next step
            await this._candidateRepository.UpdateAsync(candidate);
        }
    }
}