using SmartRecruitment.Domain.Entities;

namespace SmartRecruitment.Application.Interfaces
{
    // Implemented in Infrastructure by an adapter that calls the Llama 2 model
    public interface ICVContentGenerator
    {
        Task<string> GenerateContentAsync(Candidate candidate);
    }
}