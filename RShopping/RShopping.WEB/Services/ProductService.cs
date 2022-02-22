using RShopping.WEB.Models;
using RShopping.WEB.Services.IServices;
using RShopping.WEB.Utils;

namespace RShopping.WEB.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        public const string BaseUrl = "api/v1/product";

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<ProductModel> Create(ProductModel model)
        {
            HttpResponseMessage response = await _httpClient.PostAsJson(BaseUrl, model);
            if (response.IsSuccessStatusCode) return await response.ReadContentAs<ProductModel>();
            else throw new Exception("Error in post product via API");
        }

        public async Task<bool> Delete(long id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
            if (response.IsSuccessStatusCode) return await response.ReadContentAs<bool>();
            else throw new Exception("Error in delete product via API");
        }

        public async Task<IEnumerable<ProductModel>> FindAll()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(BaseUrl);
            return await response.ReadContentAs<List<ProductModel>>();
        }

        public async Task<ProductModel> FindById(long id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{BaseUrl}/{id}");
            return await response.ReadContentAs<ProductModel>();
        }

        public async Task<ProductModel> Update(ProductModel model)
        {
            HttpResponseMessage response = await _httpClient.PutAsJson(BaseUrl, model);
            if (response.IsSuccessStatusCode) return await response.ReadContentAs<ProductModel>();
            else throw new Exception("Error in put product via API");
        }
    }
}
