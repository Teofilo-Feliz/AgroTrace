namespace AgroTrace.Aplication.Helpers
{
    public class StringNormalizer
    {
        public static string Normalize(string value)
        {
            return value?.Trim().ToLower() ?? string.Empty;
        }
    }
}
