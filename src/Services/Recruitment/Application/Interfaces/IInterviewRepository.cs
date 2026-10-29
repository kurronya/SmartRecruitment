using SmartRecruitment.Domain.Entities;

namespace SmartRecruitment.Application.Interfaces
{
    public interface IInterviewRepository
    {
        Task AddAsync(Interview interview);
        Task UpdateAsync(Interview interview);
        Task<Interview?> GetByIdAsync(Guid id);

        // Returns a Scheduled or Completed interview for this candidate, if one exists -
        // used to enforce that a candidate can only interview with one company at a time
        Task<Interview?> GetActiveInterviewForCandidateAsync(Guid candidateId);
    }
}