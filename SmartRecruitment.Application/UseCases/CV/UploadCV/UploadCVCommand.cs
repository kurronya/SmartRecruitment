using SmartRecruitment.Domain.Enums;

namespace SmartRecruitment.Application.UseCases.CV.UploadCV
{
    public class UploadCVCommand
    {
        public Guid CandidateId { get; set; }
        public byte[] FileContent { get; set; }
        public string FileName { get; set; }

        public UploadCVCommand(Guid candidateId, byte[] fileContent, string fileName)
        {
            this.CandidateId = candidateId;
            this.FileContent = fileContent;
            this.FileName = fileName;
        }
    }
}