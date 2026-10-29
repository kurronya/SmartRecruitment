using Microsoft.EntityFrameworkCore;
using SmartRecruitment.Application.Interfaces;
using SmartRecruitment.Domain.Entities;

namespace SmartRecruitment.Infrastructure.Persistence.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly CompanyDbContext _context;

        public CompanyRepository(CompanyDbContext context)
        {
            this._context = context;
        }

        public async Task AddAsync(Company company)
        {
            await this._context.Companies.AddAsync(company);
            await this._context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Company company)
        {
            this._context.Companies.Update(company);
            await this._context.SaveChangesAsync();
        }

        public async Task<Company?> GetByIdAsync(Guid id)
        {
            return await this._context.Companies.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Company?> GetByEmailAsync(string email)
        {
            return await this._context.Companies.FirstOrDefaultAsync(c => c.Email == email);
        }
    }
}