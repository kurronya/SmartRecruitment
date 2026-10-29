using Microsoft.EntityFrameworkCore;
using SmartRecruitment.Application.Interfaces;
using SmartRecruitment.Domain.Entities;
using SmartRecruitment.Domain.Enums;

namespace SmartRecruitment.Infrastructure.Persistence.Repositories
{
    public class CVRepository : ICVRepository
    {
        private readonly CVDbContext _context;

        public CVRepository(CVDbContext context)
        {
            this._context = context;
        }

        public async Task AddAsync(CV cv)
        {
            await this._context.CVs.AddAsync(cv);
            await this._context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CV cv)
        {
            this._context.CVs.Update(cv);
            await this._context.SaveChangesAsync();
        }

        public async Task<CV?> GetByIdAsync(Guid id)
        {
            return await this._context.CVs.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<CV>> GetAllFinalizedAsync()
        {
            // Only finalized CVs carry an embedding and are eligible for matching
            return await this._context.CVs
                .Where(c => c.Status == CVStatus.Finalized)
                .ToListAsync();
        }
    }
}