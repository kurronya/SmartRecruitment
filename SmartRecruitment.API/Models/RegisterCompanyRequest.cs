namespace SmartRecruitment.API.Models
{
    public class RegisterCompanyRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string BusinessRegistrationNumber { get; set; } = string.Empty;
    }
}