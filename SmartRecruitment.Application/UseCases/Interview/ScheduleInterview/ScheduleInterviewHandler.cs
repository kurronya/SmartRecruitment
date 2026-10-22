using SmartRecruitment.Application.Interfaces;

namespace SmartRecruitment.Application.UseCases.Interview.ScheduleInterview
{
    public class ScheduleInterviewHandler
    {
        private readonly IInterviewRepository _interviewRepository;
        private readonly ICandidateRepository _candidateRepository;
        private readonly IJobDescriptionRepository _jobDescriptionRepository;

        public ScheduleInterviewHandler(
            IInterviewRepository interviewRepository,
            ICandidateRepository candidateRepository,
            IJobDescriptionRepository jobDescriptionRepository)
        {
            this._interviewRepository = interviewRepository;
            this._candidateRepository = candidateRepository;
            this._jobDescriptionRepository = jobDescriptionRepository;
        }

        public async Task<Guid> HandleAsync(ScheduleInterviewCommand command)
        {
            var candidate = await this._candidateRepository.GetByIdAsync(command.CandidateId);
            if (candidate == null)
                throw new KeyNotFoundException("Candidate not found.");

            var jobDescription = await this._jobDescriptionRepository.GetByIdAsync(command.JobDescriptionId);
            if (jobDescription == null)
                throw new KeyNotFoundException("Job description not found.");

            // Cross entity business rule: a candidate can only interview with one company at a time
            var activeInterview = await this._interviewRepository.GetActiveInterviewForCandidateAsync(command.CandidateId);
            if (activeInterview != null)
                throw new InvalidOperationException("Candidate is currently interviewing elsewhere.");

            var interview = new Domain.Entities.Interview(command.CandidateId, command.JobDescriptionId, command.ScheduledAt);

            // Entity enforces that only an unemployed candidate can enter an interview process
            candidate.EnterInterviewProcess();

            await this._interviewRepository.AddAsync(interview);
            await this._candidateRepository.UpdateAsync(candidate);

            return interview.Id;
        }
    }
}