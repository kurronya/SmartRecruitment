using System.Net.Http.Json;
using SmartRecruitment.Application.Interfaces;

namespace SmartRecruitment.Infrastructure.Gateways
{
    public class CompanyHttpGateway : ICompanyGateway
    {
        private readonly HttpClient _http;

        public CompanyHttpGateway(HttpClient http)
        {
            this._http = http;
        }

        public async Task<bool> IsVerifiedAsync(Guid companyId)
        {
            var response = await this._http.GetAsync($"internal/companies/{companyId}/verification-status");
            await GatewayResponse.EnsureSuccessAsync(response, "Company not found.");

            var body = await response.Content.ReadFromJsonAsync<VerificationStatusResponse>();
            return body?.Status == "Verified";
        }

        private record VerificationStatusResponse(string? Status);
    }
}
