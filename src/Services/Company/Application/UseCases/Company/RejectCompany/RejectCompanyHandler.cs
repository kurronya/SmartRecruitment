using SmartRecruitment.Application.Interfaces;

namespace SmartRecruitment.Application.UseCases.Company.RejectCompany
{
    public class RejectCompanyHandler
    {
        private readonly ICompanyRepository _companyRepository;

        public RejectCompanyHandler(ICompanyRepository companyRepository)
        {
            this._companyRepository = companyRepository;
        }

        public async Task HandleAsync(RejectCompanyCommand command)
        {
            var company = await this._companyRepository.GetByIdAsync(command.CompanyId);
            if (company == null)
                throw new KeyNotFoundException("Company not found.");

            // The business rule (only 'Pending' items can be rejected; a reason is mandatory) has already been implemented in the entity.
            company.Reject(command.Reason);

            await this._companyRepository.UpdateAsync(company);
        }
    }
}