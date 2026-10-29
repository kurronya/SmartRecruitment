using SmartRecruitment.Application.Interfaces;

namespace SmartRecruitment.Application.UseCases.Company.GetCompanyVerificationStatus
{
    // Read-only query used by the Recruitment Service to check whether a company may post job descriptions
    public class GetCompanyVerificationStatusHandler
    {
        private readonly ICompanyRepository _companyRepository;

        public GetCompanyVerificationStatusHandler(ICompanyRepository companyRepository)
        {
            this._companyRepository = companyRepository;
        }

        public async Task<string> HandleAsync(Guid companyId)
        {
            var company = await this._companyRepository.GetByIdAsync(companyId);
            if (company == null)
                throw new KeyNotFoundException("Company not found.");

            return company.VerificationStatus.ToString();
        }
    }
}
