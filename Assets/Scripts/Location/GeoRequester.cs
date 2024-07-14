using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Location
{
    internal class GeoRequester : MonoBehaviour
    {
        private HttpClient _httpClient;

        private void Awake()
        {
            _httpClient = new HttpClient();
            GetAsync();
        }

        public async Task<GeoJSON> GetAsync()
        {
            HttpRequestMessage request = new(HttpMethod.Get, "http://ip-api.com/json");
            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var data = JsonConvert.DeserializeObject<GeoJSON>(await response.Content.ReadAsStringAsync());
                Debug.Log($"Location: {data.country}");
                return data;
            }
            else
            {
                throw new HttpRequestException($"Request failed with code {response.StatusCode}");
            }
        }
    }
}
