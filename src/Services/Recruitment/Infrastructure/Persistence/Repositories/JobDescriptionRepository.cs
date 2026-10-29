using Microsoft.EntityFrameworkCore;
using SmartRecruitment.Application.Interfaces;

namespace SmartRecruitment.Infrastructure.Persistence.Repositories
{
    public class JobDescriptionRepository : IJobDescriptionRepository
    {
        private readonly RecruitmentDbContext _context;

        public JobDescriptionRepository(RecruitmentDbContext context)
        {
            this._context = context;
        }

        public async Task AddAsync(Domain.Entities.JobDescription jobDescription)
        {
            await this._context.JobDescriptions.AddAsync(jobDescription);
            await this._context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Domain.Entities.JobDescription jobDescription)
        {
            this._context.JobDescriptions.Update(jobDescription);
            await this._context.SaveChangesAsync();
        }

        public async Task<Domain.Entities.JobDescription?> GetByIdAsync(Guid id)
        {
            return await this._context.JobDescriptions.FirstOrDefaultAsync(j => j.Id == id);
        }
    }
}