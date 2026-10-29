using SmartRecruitment.Application.Interfaces;

namespace SmartRecruitment.Application.UseCases.Interview.ScheduleInterview
{
    public class ScheduleInterviewHandler
    {
        private readonly IInterviewRepository _interviewRepository;
        private readonly ICandidateGateway _candidateGateway;
        private readonly IJobDescriptionRepository _jobDescriptionRepository;

        public ScheduleInterviewHandler(
            IInterviewRepository interviewRepository,
            ICandidateGateway candidateGateway,
            IJobDescriptionRepository jobDescriptionRepository)
        {
            this._interviewRepository = interviewRepository;
            this._candidateGateway = candidateGateway;
            this._jobDescriptionRepository = jobDescriptionRepository;
        }

        public async Task<Guid> HandleAsync(ScheduleInterviewCommand command)
        {
            var jobDescription = await this._jobDescriptionRepository.GetByIdAsync(command.JobDescriptionId);
            if (jobDescription == null)
                throw new KeyNotFoundException("Job description not found.");

            // Cross entity business rule: a candidate can only interview with one company at a time
            var activeInterview = await this._interviewRepository.GetActiveInterviewForCandidateAsync(command.CandidateId);
            if (activeInterview != null)
                throw new InvalidOperationException("Candidate is currently interviewing elsewhere.");

            var interview = new Domain.Entities.Interview(command.CandidateId, command.JobDescriptionId, command.ScheduledAt);

            // The Candidate Service owns candidate status: it throws KeyNotFoundException if the candidate
            // does not exist and InvalidOperationException if the candidate is not unemployed
            await this._candidateGateway.EnterInterviewAsync(command.CandidateId);

            try
            {
                await this._interviewRepository.AddAsync(interview);
            }
            catch
            {
                // Compensation: there is no shared transaction across services, so undo the status change manually
                await this._candidateGateway.ReturnToUnemployedAsync(command.CandidateId);
                throw;
            }

            return interview.Id;
        }
    }
}
