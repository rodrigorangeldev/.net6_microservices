using System.Net.Http.Headers;
using System.Text.Json;

namespace RShopping.WEB.Utils
{
    public static class HttpClientUtil
    {
        private static MediaTypeHeaderValue ContentType = new MediaTypeHeaderValue("application/json");

        public static async Task<T> ReadContentAs<T>(this HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException($"Error in http call: {response.ReasonPhrase}");
            }

            var responseString =  await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return JsonSerializer.Deserialize<T>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true});
        }

        public static Task<HttpResponseMessage> PostAsJson<T>(this HttpClient httpClient, string url, T data)
        {
            var serialized = JsonSerializer.Serialize(data);
            var content = new StringContent(serialized);
            content.Headers.ContentType = ContentType;
            return httpClient.PostAsync(url, content);
        }

        public static Task<HttpResponseMessage> PutAsJson<T>(this HttpClient httpClient, string url, T data)
        {
            var serialized = JsonSerializer.Serialize(data);
            var content = new StringContent(serialized);
            content.Headers.ContentType = ContentType;
            return httpClient.PutAsync(url, content);
        }
    }
}
