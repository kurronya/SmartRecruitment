using SmartRecruitment.Domain.Entities;

namespace SmartRecruitment.Application.Interfaces
{
    public interface ICandidateRepository
    {
        Task AddAsync(Candidate candidate);
        Task UpdateAsync(Candidate candidate);
        Task<Candidate?> GetByIdAsync(Guid id);
        Task<Candidate?> GetByEmailAsync(string email);
    }
}