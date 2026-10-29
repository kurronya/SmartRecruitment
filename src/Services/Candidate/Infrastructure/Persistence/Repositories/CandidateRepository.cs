using Microsoft.EntityFrameworkCore;
using SmartRecruitment.Application.Interfaces;
using SmartRecruitment.Domain.Entities;

namespace SmartRecruitment.Infrastructure.Persistence.Repositories
{
    public class CandidateRepository : ICandidateRepository
    {
        private readonly CandidateDbContext _context;

        public CandidateRepository(CandidateDbContext context)
        {
            this._context = context;
        }

        public async Task AddAsync(Candidate candidate)
        {
            await this._context.Candidates.AddAsync(candidate);
            await this._context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Candidate candidate)
        {
            this._context.Candidates.Update(candidate);
            await this._context.SaveChangesAsync();
        }

        public async Task<Candidate?> GetByIdAsync(Guid id)
        {
            return await this._context.Candidates.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Candidate?> GetByEmailAsync(string email)
        {
            return await this._context.Candidates.FirstOrDefaultAsync(c => c.Email == email);
        }
    }
}