namespace SmartRecruitment.API.Models
{
    public class SubmitJobDescriptionRequest
    {
        public Guid CompanyId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Requirements { get; set; } = string.Empty;
    }
}