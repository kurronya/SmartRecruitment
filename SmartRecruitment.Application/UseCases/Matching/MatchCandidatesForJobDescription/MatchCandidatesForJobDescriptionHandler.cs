using SmartRecruitment.Application.Common;
using SmartRecruitment.Application.DTOs;
using SmartRecruitment.Application.Interfaces;

namespace SmartRecruitment.Application.UseCases.Matching.MatchCandidatesForJobDescription
{
    public class MatchCandidatesForJobDescriptionHandler
    {
        private readonly IJobDescriptionRepository _jobDescriptionRepository;
        private readonly ICVRepository _cvRepository;

        public MatchCandidatesForJobDescriptionHandler(
            IJobDescriptionRepository jobDescriptionRepository,
            ICVRepository cvRepository)
        {
            this._jobDescriptionRepository = jobDescriptionRepository;
            this._cvRepository = cvRepository;
        }

        public async Task<List<CandidateMatchResult>> HandleAsync(MatchCandidatesForJobDescriptionQuery query)
        {
            var jobDescription = await this._jobDescriptionRepository.GetByIdAsync(query.JobDescriptionId);
            if (jobDescription == null)
                throw new KeyNotFoundException("Job description not found.");

            if (jobDescription.Embedding == null)
                throw new InvalidOperationException("Job description does not have an embedding yet.");

            // Only finalized CVs carry an embedding, so only those are eligible for matching
            var candidateCVs = await this._cvRepository.GetAllFinalizedAsync();

            var results = new List<CandidateMatchResult>();

            foreach (var cv in candidateCVs)
            {
                // Defensive check - a finalized CV should always have an embedding,
                // but skip gracefully instead of throwing if data is inconsistent
                if (cv.Embedding == null)
                    continue;

                var score = CosineSimilarityCalculator.Calculate(jobDescription.Embedding, cv.Embedding);
                results.Add(new CandidateMatchResult(cv.CandidateId, cv.Id, score));
            }

            // Rank candidates by descending similarity and return only the top N
            return results
                .OrderByDescending(r => r.SimilarityScore)
                .Take(query.TopN)
                .ToList();
        }
    }
}