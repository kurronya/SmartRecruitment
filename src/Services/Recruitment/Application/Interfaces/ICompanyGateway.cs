namespace SmartRecruitment.Application.Interfaces
{
    // Port to the Company Service (read-only). Throws KeyNotFoundException when the company does not exist.
    public interface ICompanyGateway
    {
        Task<bool> IsVerifiedAsync(Guid companyId);
    }
}
