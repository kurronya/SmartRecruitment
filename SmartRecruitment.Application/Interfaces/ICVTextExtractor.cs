namespace SmartRecruitment.Application.Interfaces
{
    // Implemented in Infrastructure by an adapter that reads an uploaded file (PDF/DOCX) and extracts raw text
    public interface ICVTextExtractor
    {
        Task<string> ExtractTextAsync(byte[] fileContent, string fileName);
    }
}