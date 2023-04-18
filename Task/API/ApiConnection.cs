using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Task.DTO;

namespace Task
{
    public class ApiConnection
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        public ApiConnection(HttpClient httpClient, string apiUrl)
        {
            _httpClient = httpClient;
            _apiUrl = apiUrl;
        }

        public async Task<List<ProductDTO>> GetStockNameAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiUrl}/api/STI/GetStockName").ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var products = JsonConvert.DeserializeObject<List<ProductDTO>>(responseContent);
                return products;
            }
            catch (HttpRequestException ex)
            {
                // Log the exception and rethrow or handle it appropriately
                throw new Exception("An error occurred while calling the API", ex);
            }
        }

        public async Task<List<StockDataDTO>> GetStockStatementAsync(FilterDTO filter)
        {
            try
            {
                var requestData = new FilterDTO
                {
                    malKodu = filter.malKodu,
                    girisTarih = filter.girisTarih,
                    cikisTarih = filter.cikisTarih
                };

                var requestDataJson = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(requestDataJson, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{_apiUrl}/api/STI/GetStockStatement", content).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var stock = JsonConvert.DeserializeObject<StockDTO>(responseContent);
                return stock.Result;
            }
            catch (HttpRequestException ex)
            {
                // Log the exception and rethrow or handle it appropriately
                throw new Exception("An error occurred while calling the API", ex);
            }
        }
    }
}