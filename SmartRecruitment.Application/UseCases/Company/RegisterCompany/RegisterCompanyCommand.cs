namespace SmartRecruitment.Application.UseCases.Company.RegisterCompany
{
    public class RegisterCompanyCommand
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string BusinessRegistrationNumber { get; set; }

        public RegisterCompanyCommand(string name, string email, string businessRegistrationNumber)
        {
            this.Name = name;
            this.Email = email;
            this.BusinessRegistrationNumber = businessRegistrationNumber;
        }
    }
}