namespace Geta.SocialChannels
{
    public static class Base64Utils
    {
        public static string Base64Encode(string plainText) {
            if (string.IsNullOrEmpty(plainText))
            {
                return null;
            }
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}