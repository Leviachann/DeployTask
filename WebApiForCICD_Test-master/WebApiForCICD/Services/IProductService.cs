using WebApiForCICD.Entities;

namespace WebApiForCICD.Services
{
    public interface IProductService
    {
        List<Product> GetProducts();
        Product GetProductById(int id);
        void Add(Product product);
        void Update(Product product);
        void Delete(int id);
    }
}
