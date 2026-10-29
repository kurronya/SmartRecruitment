using SmartRecruitment.Application.Interfaces;
using SmartRecruitment.Domain.Enums;

namespace SmartRecruitment.Application.UseCases.CV.UploadCV
{
    public class UploadCVHandler
    {
        private readonly ICVRepository _cvRepository;
        private readonly ICVTextExtractor _textExtractor;

        public UploadCVHandler(ICVRepository cvRepository, ICVTextExtractor textExtractor)
        {
            this._cvRepository = cvRepository;
            this._textExtractor = textExtractor;
        }

        public async Task<Guid> HandleAsync(UploadCVCommand command)
        {
            // Delegate the actual file parsing to the Infrastructure adapter -
            // Application only knows it needs "text", not how to read a PDF/DOCX
            var extractedText = await this._textExtractor.ExtractTextAsync(command.FileContent, command.FileName);

            var cv = new Domain.Entities.CV(command.CandidateId, extractedText, CVSource.Uploaded);

            await this._cvRepository.AddAsync(cv);

            return cv.Id;
        }
    }
}