namespace SmartRecruitment.Application.UseCases.Company.ApproveCompany
{
    public class ApproveCompanyCommand
    {
        public Guid CompanyId { get; set; }

        public ApproveCompanyCommand(Guid companyId)
        {
            this.CompanyId = companyId;
        }
    }
}