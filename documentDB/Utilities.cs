using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace documentDB
{
    public static partial class CRUD
    {
        private static StringContent GetStringContent(string body)
        {
            StringContent stringContent = new StringContent(body);
            stringContent.Headers.ContentLength = Encoding.UTF8.GetByteCount(body);
            return stringContent;
        }

        private async static Task<HttpClient> GetClient(string verb, string accountID, string mainID)
        {
            HttpClient client = new HttpClient();
            string date = DateTime.UtcNow.ToString("R", CultureInfo.InvariantCulture);
            string authHeader = await Authenticate.GetAuthorizationHeader(verb, date, mainID);
            client.DefaultRequestHeaders.Add("authorization", authHeader);
            client.DefaultRequestHeaders.Add("x-ms-date", date);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("Host", String.Format("{0}.documents.azure.com", accountID));
            client.DefaultRequestHeaders.Add("x-ms-consistency-level", "Session");
            client.DefaultRequestHeaders.Add("x-ms-version", "2015-04-08");
            return client;
        }
    }
}
