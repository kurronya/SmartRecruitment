using SmartRecruitment.Application.Interfaces;
using SmartRecruitment.Domain.Entities;

namespace SmartRecruitment.Infrastructure.AI
{
    // Temporary placeholder until the real Llama 2 adapter is ready.
    // Produces a simple templated CV from the candidate's own profile fields.
    public class FakeCVContentGenerator : ICVContentGenerator
    {
        public Task<string> GenerateContentAsync(Candidate candidate)
        {
            var content = $"CV for {candidate.FullName}\nEmail: {candidate.Email}\nPhone: {candidate.PhoneNumber}";
            return Task.FromResult(content);
        }
    }
}