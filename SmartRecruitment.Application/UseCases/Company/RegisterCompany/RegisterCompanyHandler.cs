using SmartRecruitment.Application.Interfaces;

namespace SmartRecruitment.Application.UseCases.Company.RegisterCompany
{
    public class RegisterCompanyHandler
    {
        private readonly ICompanyRepository _companyRepository;

        public RegisterCompanyHandler(ICompanyRepository companyRepository)
        {
            this._companyRepository = companyRepository;
        }

        public async Task<Guid> HandleAsync(RegisterCompanyCommand command)
        {
            // Business rule: không cho đăng ký trùng email
            var existingCompany = await this._companyRepository.GetByEmailAsync(command.Email);
            if (existingCompany != null)
                throw new InvalidOperationException("A company with this email already exists.");

            var company = new Domain.Entities.Company(
                command.Name,
                command.Email,
                command.BusinessRegistrationNumber
            );

            await this._companyRepository.AddAsync(company);

            return company.Id;
        }
    }
}