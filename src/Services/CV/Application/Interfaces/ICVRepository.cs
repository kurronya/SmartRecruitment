using SmartRecruitment.Domain.Entities;

namespace SmartRecruitment.Application.Interfaces
{
    public interface ICVRepository
    {
        Task AddAsync(CV cv);
        Task UpdateAsync(CV cv);
        Task<CV?> GetByIdAsync(Guid id);
        Task<List<CV>> GetAllFinalizedAsync();
    }
}