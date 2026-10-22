namespace SmartRecruitment.Application.UseCases.Company.RejectCompany
{
    public class RejectCompanyCommand
    {
        public Guid CompanyId { get; set; }
        public string Reason { get; set; }

        public RejectCompanyCommand(Guid companyId, string reason)
        {
            this.CompanyId = companyId;
            this.Reason = reason;
        }
    }
}