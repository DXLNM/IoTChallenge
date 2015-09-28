using System.Net.Http;
using System.Threading.Tasks;

namespace documentDB
{
    internal static class Authenticate
    {
        static HttpClient client = new HttpClient();

        internal static async Task<string> GetAuthorizationHeader(string httpRequest, string date, string docID)
        {
            string nodejsAuth = "http://nodejsauth.azurewebsites.net/authDocPermission/{0}/{1}/{2}";
            switch (httpRequest)
            {
                case "get":
                    nodejsAuth = string.Format(nodejsAuth,"get", date, docID);
                    return await client.GetStringAsync(nodejsAuth);
                case "post":
                    nodejsAuth = string.Format(nodejsAuth,"post", date,docID);
                    return await client.GetStringAsync(nodejsAuth);
                case "put":
                    nodejsAuth = string.Format(nodejsAuth, "put", date, docID);
                    return await client.GetStringAsync(nodejsAuth);
                case "delete":
                    nodejsAuth = string.Format(nodejsAuth, "delete", date, docID);
                    return await client.GetStringAsync(nodejsAuth);
            }

            return null;
        }

        //public static string GetHeaderLocal()
        //{
        //    string formatedTime = DateTime.UtcNow.ToString("R", CultureInfo.InvariantCulture);
        //    String Key = "1fn+QHMexpQT1vzH9S3u8b8frjS97qQVcYUo2/iw46EkviIzjuuyaf4PVRmaW+sNa45y8kRIScQh+WRO2NOazg==";
        //    String text = "get" + "\n" +
        //"docs" + "\n" +
        //"123456" + "\n" +
        //formatedTime + "\n" +
        // "\n";

        //    string auth = HmacSha256(Key, text);
        //    auth = "type=master&ver=1.0&sig=" + auth;
        //    auth = auth.Replace("=", "%3d");
        //    auth = auth.Replace("&", "%26");
        //    auth = auth.Replace("/", "%2f");
        //    return (auth);
        //}

        //public static string HmacSha256(string secretKey, string value)
        //{
        // Move strings to buffers.
        //var key = CryptographicBuffer.ConvertStringToBinary(secretKey, BinaryStringEncoding.Utf8);
        //var msg = CryptographicBuffer.ConvertStringToBinary(value, BinaryStringEncoding.Utf8);

        //// Create HMAC.
        //var objMacProv = MacAlgorithmProvider.OpenAlgorithm(MacAlgorithmNames.HmacSha256);
        //var hash = objMacProv.CreateHash(key);
        //hash.Append(msg);
        //return CryptographicBuffer.EncodeToBase64String(hash.GetValueAndReset());
        //}
    }
}
