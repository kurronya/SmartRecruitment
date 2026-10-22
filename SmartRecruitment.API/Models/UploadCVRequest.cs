using Microsoft.AspNetCore.Http;

namespace SmartRecruitment.API.Models
{
    public class UploadCVRequest
    {
        public Guid CandidateId { get; set; }
        public IFormFile File { get; set; } = null!;
    }
}