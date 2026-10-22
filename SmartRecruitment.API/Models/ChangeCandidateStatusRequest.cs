using SmartRecruitment.Domain.Enums;

namespace SmartRecruitment.API.Models
{
    public class ChangeCandidateStatusRequest
    {
        public CandidateStatus NewStatus { get; set; }
    }
}