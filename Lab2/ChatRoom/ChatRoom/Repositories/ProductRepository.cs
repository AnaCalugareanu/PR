using System.Text.Json;
using ChatRoom.Data;
using ChatRoom.Models;
using ChatRoom.Models.Dtos;

namespace ChatRoom.Repositories
{
    public class ProductRepository
    {
        private readonly AppDbContext context;

        public ProductRepository(AppDbContext context)
        {
            this.context = context;
        }

        public List<Product> GetProducts(int offset = 0, int limit = 5)
        {
            var products = context.Products.Skip(offset).Take(limit).ToList();
            return products;
        }

        public void InsertProduct(InsertProductDto productDto)
        {
            var product = new Product
            {
                ProductName = productDto.ProductName,
                Price = productDto.Price,
                Specifications = productDto.Specifications,
            };

            context.Products.Add(product);
            context.SaveChanges();
        }

        public void UpdateProduct(int id, UpdateProductDto updateProductDto)
        {
            var productToBeUpdated = context.Products.FirstOrDefault(x => x.ProductId == id);

            if (productToBeUpdated == null)
                return;

            productToBeUpdated.ProductName = updateProductDto.ProductName ?? productToBeUpdated.ProductName;
            productToBeUpdated.Price = updateProductDto.Price ?? productToBeUpdated.Price;
            productToBeUpdated.Specifications = updateProductDto.Specifications ?? productToBeUpdated.Specifications;

            context.Products.Update(productToBeUpdated);
            context.SaveChanges();
        }

        public void DeleteProduct(int id)
        {
            var productToBeDeleted = context.Products.FirstOrDefault(x => x.ProductId == id);

            if (productToBeDeleted == null)
                return;

            context.Products.Remove(productToBeDeleted);
            context.SaveChanges();
        }

        public async Task<string> ProcessAndInsertProductsFromFile(IFormFile file)
        {
            List<Product> products;
            using (var streamReader = new StreamReader(file.OpenReadStream()))
            {
                var fileContent = await streamReader.ReadToEndAsync();
                try
                {
                    products = JsonSerializer.Deserialize<List<Product>>(fileContent);
                }
                catch (JsonException)
                {
                    return "Invalid JSON format.";
                }
            }

            foreach (var product in products)
            {
                if (string.IsNullOrEmpty(product.ProductName) || product.Price <= 0)
                {
                    return "Invalid product data: ProductName is required and Price must be positive.";
                }
            }

            context.Products.AddRange(products);
            await context.SaveChangesAsync();

            return "Products successfully uploaded and saved.";
        }
    }
}