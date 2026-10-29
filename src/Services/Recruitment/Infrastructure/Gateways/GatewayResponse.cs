using System.Net;
using System.Text.Json;

namespace SmartRecruitment.Infrastructure.Gateways
{
    // Translates HTTP status codes from another service back into the exceptions the controllers already handle
    internal static class GatewayResponse
    {
        public static async Task EnsureSuccessAsync(HttpResponseMessage response, string notFoundMessage)
        {
            if (response.IsSuccessStatusCode)
                return;

            if (response.StatusCode == HttpStatusCode.NotFound)
                throw new KeyNotFoundException(notFoundMessage);

            if (response.StatusCode == HttpStatusCode.Conflict)
                throw new InvalidOperationException(await ReadMessageAsync(response));

            // Anything else (500, 403, timeout...) is an infrastructure problem, not a business error
            response.EnsureSuccessStatusCode();
        }

        private static async Task<string> ReadMessageAsync(HttpResponseMessage response)
        {
            var body = await response.Content.ReadAsStringAsync();
            try
            {
                using var document = JsonDocument.Parse(body);
                if (document.RootElement.TryGetProperty("message", out var message))
                    return message.GetString() ?? "Operation not allowed.";
            }
            catch (JsonException)
            {
                // Body was not JSON - fall through to the default message
            }

            return "Operation not allowed.";
        }
    }
}
