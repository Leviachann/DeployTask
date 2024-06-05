using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using System.Net;
using System.Net.Http.Json;
using WebApiForCICD;
using WebApiForCICD.Entities;

namespace WebApi.Tests
{
    [TestFixture]
    public class ProductsControllerTests
    {
        private WebApplicationFactory<Program> _factory;
        private HttpClient _client;

        [SetUp]
        public void SetUp()
        {
            _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {

                });
            });

            _client = _factory.CreateClient();
        }

        [Test]
        public async Task GetProducts_ReturnsOkResponse()
        {
            var response = await _client.GetAsync("/api/products");
            response.EnsureSuccessStatusCode();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task CreateProduct_ReturnsCreatedResponse()
        {
            var product = new Product { Name = "Test Product", Price = 9.99 };
            var response = await _client.PostAsJsonAsync("/api/products", product);
            response.EnsureSuccessStatusCode();
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var createdProduct = await response.Content.ReadFromJsonAsync<Product>();
            Console.WriteLine("Added with this ID " + createdProduct.Id);
            Assert.IsNotNull(createdProduct);
            Assert.AreEqual("Test Product", createdProduct.Name);
        }

        [Test]
        public async Task GetProduct_ReturnsProduct()
        {
            var product = new Product { Name = "Test Product", Price = 9.99 };
            var createResponse = await _client.PostAsJsonAsync("/api/products", product);
            createResponse.EnsureSuccessStatusCode();

            var createdProduct = await createResponse.Content.ReadFromJsonAsync<Product>();

            var getResponse = await _client.GetAsync($"/api/products/{createdProduct.Id}");
            getResponse.EnsureSuccessStatusCode();
            Assert.AreEqual(HttpStatusCode.OK, getResponse.StatusCode);

            var fetchedProduct = await getResponse.Content.ReadFromJsonAsync<Product>();
            Assert.AreEqual(createdProduct.Id, fetchedProduct.Id);
            Assert.AreEqual(createdProduct.Name, fetchedProduct.Name);
        }

        [Test]
        public async Task UpdateProduct_ReturnsOkResponse()
        {
            var product = new Product { Name = "Test Product", Price = 9.99 };
            var createResponse = await _client.PostAsJsonAsync("/api/products", product);
            createResponse.EnsureSuccessStatusCode();

            var createdProduct = await createResponse.Content.ReadFromJsonAsync<Product>();
            createdProduct.Name = "Updated Test Product";

            var updateResponse = await _client.PutAsJsonAsync($"/api/products/{createdProduct.Id}", createdProduct);
            updateResponse.EnsureSuccessStatusCode();
            Assert.AreEqual(HttpStatusCode.OK, updateResponse.StatusCode);

            var updatedProduct = await updateResponse.Content.ReadFromJsonAsync<Product>();
            Assert.AreEqual("Updated Test Product", updatedProduct.Name);
        }
// hiiiiiii
        [Test]
        public async Task DeleteProduct_ReturnsNoContentResponse()
        {
            var product = new Product { Name = "Test Product", Price = 9.99 };
            var createResponse = await _client.PostAsJsonAsync("/api/products", product);
            createResponse.EnsureSuccessStatusCode();

            var createdProduct = await createResponse.Content.ReadFromJsonAsync<Product>();

            var deleteResponse = await _client.DeleteAsync($"/api/products/{createdProduct.Id}");
            Assert.AreEqual(HttpStatusCode.NoContent, deleteResponse.StatusCode);

            var getResponse = await _client.GetAsync($"/api/products/{createdProduct.Id}");
            Assert.AreEqual(HttpStatusCode.NotFound, getResponse.StatusCode);
        }
    }
}
