using SmartRecruitment.Domain.Entities;

namespace SmartRecruitment.Application.Interfaces
{
    public interface IJobDescriptionRepository
    {
        Task AddAsync(JobDescription jobDescription);
        Task UpdateAsync(JobDescription jobDescription);
        Task<JobDescription?> GetByIdAsync(Guid id);
    }
}