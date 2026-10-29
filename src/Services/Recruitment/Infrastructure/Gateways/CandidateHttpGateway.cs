using SmartRecruitment.Application.Interfaces;

namespace SmartRecruitment.Infrastructure.Gateways
{
    public class CandidateHttpGateway : ICandidateGateway
    {
        private readonly HttpClient _http;

        public CandidateHttpGateway(HttpClient http)
        {
            this._http = http;
        }

        public Task EnterInterviewAsync(Guid candidateId)
            => this.PostAsync($"internal/candidates/{candidateId}/enter-interview");

        public Task MarkAsEmployedAsync(Guid candidateId)
            => this.PostAsync($"internal/candidates/{candidateId}/mark-employed");

        public Task ReturnToUnemployedAsync(Guid candidateId)
            => this.PostAsync($"internal/candidates/{candidateId}/return-to-unemployed");

        private async Task PostAsync(string path)
        {
            var response = await this._http.PostAsync(path, null);
            await GatewayResponse.EnsureSuccessAsync(response, "Candidate not found.");
        }
    }
}
