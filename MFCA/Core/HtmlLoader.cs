using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MFCA.Core
{
    class HtmlLoader
    {
        readonly HttpClient client;
        readonly string url;

        public HtmlLoader(IParserSettings settings)
        {
            client = new HttpClient();
            url = $"{settings.BaseURL}";
        }

        public async Task<string> GetSourceByPage()
        {
            var currentUrl = url;

            Console.WriteLine($"Connected to {currentUrl}");
            var response = await client.GetAsync(currentUrl);
            string source = null;

            if (response != null && response.StatusCode == HttpStatusCode.OK)
            {
                source = await response.Content.ReadAsStringAsync();
            }
            
            return source;
        }
    }
}
