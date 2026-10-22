namespace SmartRecruitment.Application.Common
{
    // Pure computation helper - no external dependency, so it lives directly in Application
    // rather than behind an interface implemented by Infrastructure
    public static class CosineSimilarityCalculator
    {
        public static float Calculate(float[] vectorA, float[] vectorB)
        {
            if (vectorA.Length != vectorB.Length)
                throw new ArgumentException("Vectors must be of the same length to compare.");

            float dotProduct = 0f;
            float magnitudeA = 0f;
            float magnitudeB = 0f;

            for (int i = 0; i < vectorA.Length; i++)
            {
                dotProduct += vectorA[i] * vectorB[i];
                magnitudeA += vectorA[i] * vectorA[i];
                magnitudeB += vectorB[i] * vectorB[i];
            }

            if (magnitudeA == 0f || magnitudeB == 0f)
                return 0f;

            return dotProduct / (MathF.Sqrt(magnitudeA) * MathF.Sqrt(magnitudeB));
        }
    }
}