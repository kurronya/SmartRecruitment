using SmartRecruitment.Domain.Entities;

namespace SmartRecruitment.Application.Interfaces
{
    public interface ICompanyRepository
    {
        Task AddAsync(Company company);
        Task UpdateAsync(Company company);
        Task<Company?> GetByIdAsync(Guid id);
        Task<Company?> GetByEmailAsync(string email);
    }
}