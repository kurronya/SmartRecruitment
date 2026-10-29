using System.Net.Http.Json;
using SmartRecruitment.Application.DTOs;
using SmartRecruitment.Application.Interfaces;

namespace SmartRecruitment.Infrastructure.Gateways
{
    public class CVHttpGateway : ICVGateway
    {
        private readonly HttpClient _http;

        public CVHttpGateway(HttpClient http)
        {
            this._http = http;
        }

        public async Task<List<CandidateCvEmbedding>> GetFinalizedEmbeddingsAsync()
        {
            var response = await this._http.GetAsync("internal/cvs/finalized-embeddings");
            await GatewayResponse.EnsureSuccessAsync(response, "CV data not found.");

            var body = await response.Content.ReadFromJsonAsync<List<CandidateCvEmbedding>>();
            return body ?? new List<CandidateCvEmbedding>();
        }
    }
}
