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

        public void InsertProductsByFile(List<Product> products)
        {
            context.Products.AddRange(products);
            context.SaveChanges();
        }
    }
}