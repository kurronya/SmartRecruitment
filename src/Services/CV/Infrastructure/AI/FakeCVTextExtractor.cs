using System.Text;
using SmartRecruitment.Application.Interfaces;

namespace SmartRecruitment.Infrastructure.AI
{
    // Temporary placeholder just decodes the raw bytes as UTF-8 text instead of
    // actually parsing a PDF/DOCX. Replace with a real extractor adapter later.
    public class FakeCVTextExtractor : ICVTextExtractor
    {
        public Task<string> ExtractTextAsync(byte[] fileContent, string fileName)
        {
            var text = Encoding.UTF8.GetString(fileContent);
            return Task.FromResult(text);
        }
    }
}