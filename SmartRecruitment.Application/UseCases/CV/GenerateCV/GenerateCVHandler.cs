using SmartRecruitment.Application.Interfaces;
using SmartRecruitment.Domain.Enums;

namespace SmartRecruitment.Application.UseCases.CV.GenerateCV
{
    public class GenerateCVHandler
    {
        private readonly ICVRepository _cvRepository;
        private readonly ICandidateRepository _candidateRepository;
        private readonly ICVContentGenerator _contentGenerator;

        public GenerateCVHandler(
            ICVRepository cvRepository,
            ICandidateRepository candidateRepository,
            ICVContentGenerator contentGenerator)
        {
            this._cvRepository = cvRepository;
            this._candidateRepository = candidateRepository;
            this._contentGenerator = contentGenerator;
        }

        public async Task<Guid> HandleAsync(GenerateCVCommand command)
        {
            var candidate = await this._candidateRepository.GetByIdAsync(command.CandidateId);
            if (candidate == null)
                throw new KeyNotFoundException("Candidate not found.");

            // Delegate content generation to the Infrastructure adapter calling the Llama 2 model
            var generatedContent = await this._contentGenerator.GenerateContentAsync(candidate);

            var cv = new Domain.Entities.CV(command.CandidateId, generatedContent, CVSource.AiGenerated);

            await this._cvRepository.AddAsync(cv);

            return cv.Id;
        }
    }
}