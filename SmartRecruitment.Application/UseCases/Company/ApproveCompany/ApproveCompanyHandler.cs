using SmartRecruitment.Application.Interfaces;

namespace SmartRecruitment.Application.UseCases.Company.ApproveCompany
{
    public class ApproveCompanyHandler
    {
        private readonly ICompanyRepository _companyRepository;

        public ApproveCompanyHandler(ICompanyRepository companyRepository)
        {
            this._companyRepository = companyRepository;
        }

        public async Task HandleAsync(ApproveCompanyCommand command)
        {
            var company = await this._companyRepository.GetByIdAsync(command.CompanyId);
            if (company == null)
                throw new KeyNotFoundException("Company not found.");

            // The business rule (only 'Pending' items can be approved) is already implemented in the entity.
            company.Approve();

            await this._companyRepository.UpdateAsync(company);
        }
    }
}