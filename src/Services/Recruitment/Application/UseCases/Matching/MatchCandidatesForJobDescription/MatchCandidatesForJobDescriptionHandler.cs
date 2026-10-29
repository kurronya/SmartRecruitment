using SmartRecruitment.Application.Common;
using SmartRecruitment.Application.DTOs;
using SmartRecruitment.Application.Interfaces;

namespace SmartRecruitment.Application.UseCases.Matching.MatchCandidatesForJobDescription
{
    public class MatchCandidatesForJobDescriptionHandler
    {
        private readonly IJobDescriptionRepository _jobDescriptionRepository;
        private readonly ICVGateway _cvGateway;

        public MatchCandidatesForJobDescriptionHandler(
            IJobDescriptionRepository jobDescriptionRepository,
            ICVGateway cvGateway)
        {
            this._jobDescriptionRepository = jobDescriptionRepository;
            this._cvGateway = cvGateway;
        }

        public async Task<List<CandidateMatchResult>> HandleAsync(MatchCandidatesForJobDescriptionQuery query)
        {
            var jobDescription = await this._jobDescriptionRepository.GetByIdAsync(query.JobDescriptionId);
            if (jobDescription == null)
                throw new KeyNotFoundException("Job description not found.");

            if (jobDescription.Embedding == null)
                throw new InvalidOperationException("Job description does not have an embedding yet.");

            // The CV Service only returns finalized CVs, the only ones that carry an embedding
            var candidateCVs = await this._cvGateway.GetFinalizedEmbeddingsAsync();

            var results = new List<CandidateMatchResult>();

            foreach (var cv in candidateCVs)
            {
                // Defensive check - skip gracefully instead of throwing if data is inconsistent
                if (cv.Embedding == null)
                    continue;

                var score = CosineSimilarityCalculator.Calculate(jobDescription.Embedding, cv.Embedding);
                results.Add(new CandidateMatchResult(cv.CandidateId, cv.CvId, score));
            }

            // Rank candidates by descending similarity and return only the top N
            return results
                .OrderByDescending(r => r.SimilarityScore)
                .Take(query.TopN)
                .ToList();
        }
    }
}
