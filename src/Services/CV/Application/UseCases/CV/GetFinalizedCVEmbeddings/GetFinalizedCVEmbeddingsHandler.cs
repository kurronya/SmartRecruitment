using SmartRecruitment.Application.DTOs;
using SmartRecruitment.Application.Interfaces;

namespace SmartRecruitment.Application.UseCases.CV.GetFinalizedCVEmbeddings
{
    public class GetFinalizedCVEmbeddingsHandler
    {
        private readonly ICVRepository _cvRepository;

        public GetFinalizedCVEmbeddingsHandler(ICVRepository cvRepository)
        {
            this._cvRepository = cvRepository;
        }

        public async Task<List<FinalizedCVEmbedding>> HandleAsync()
        {
            // Only finalized CVs carry an embedding, so only those are eligible for matching
            var cvs = await this._cvRepository.GetAllFinalizedAsync();

            return cvs
                .Where(cv => cv.Embedding != null)
                .Select(cv => new FinalizedCVEmbedding(cv.CandidateId, cv.Id, cv.Embedding!))
                .ToList();
        }
    }
}
