using Microsoft.EntityFrameworkCore;
using SmartRecruitment.Application.Interfaces;
using SmartRecruitment.Domain.Entities;
using SmartRecruitment.Domain.Enums;

namespace SmartRecruitment.Infrastructure.Persistence.Repositories
{
    public class InterviewRepository : IInterviewRepository
    {
        private readonly SmartRecruitmentDbContext _context;

        public InterviewRepository(SmartRecruitmentDbContext context)
        {
            this._context = context;
        }

        public async Task AddAsync(Interview interview)
        {
            await this._context.Interviews.AddAsync(interview);
            await this._context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Interview interview)
        {
            this._context.Interviews.Update(interview);
            await this._context.SaveChangesAsync();
        }

        public async Task<Interview?> GetByIdAsync(Guid id)
        {
            return await this._context.Interviews.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Interview?> GetActiveInterviewForCandidateAsync(Guid candidateId)
        {
            // "Active" means not yet resolved - still Scheduled or Completed
            // (Passed/Failed/Cancelled are terminal states and no longer block a new interview)
            return await this._context.Interviews.FirstOrDefaultAsync(i =>
                i.CandidateId == candidateId &&
                (i.Status == InterviewStatus.Scheduled || i.Status == InterviewStatus.Completed));
        }
    }
}