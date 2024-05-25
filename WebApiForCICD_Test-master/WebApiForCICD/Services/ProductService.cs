using WebApiForCICD.Entities;

namespace WebApiForCICD.Services
{
    public class ProductService : IProductService
    {
        private static List<Product> products = new List<Product>
        {
            new Product
            {
                Id = 1,
                 Name = "Acer",
                  Price = 1100,
            },
            new Product
            {
                Id=2,
                Name ="Apple",
                 Price=2200
            }
        };
        public void Add(Product product)
        {
            products.Add(product);
        }

        public void Delete(int id)
        {
            var item = products.FirstOrDefault(p => p.Id == id);
            if (item != null)
            {
                products.Remove(item);
            }
        }

        public Product? GetProductById(int id)
        {
            return products.FirstOrDefault(p => p.Id == id);
        }

        public List<Product> GetProducts()
        {
            return products;
        }

        public void Update(Product product)
        {
            var item = products.FirstOrDefault(p => p.Id == product.Id);
            if (item != null)
            {
                item.Name = product.Name;
                item.Price = product.Price;
            }
        }
    }
}
