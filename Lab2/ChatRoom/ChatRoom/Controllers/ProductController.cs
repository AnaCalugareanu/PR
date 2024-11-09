using System.Text.Json;
using ChatRoom.Models;
using ChatRoom.Models.Dtos;
using ChatRoom.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ChatRoom.Controllers
{
    [ApiController]
    [Route("product")]
    public class ProductController : ControllerBase
    {
        private readonly ProductRepository productRepository;

        public ProductController(ProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        [HttpGet]
        public IActionResult Get(int offset = 0, int limit = 5)
        {
            return Ok(productRepository.GetProducts(offset, limit));
        }

        [HttpPost]
        public IActionResult PostProduct([FromBody] InsertProductDto productDto)
        {
            productRepository.InsertProduct(productDto);
            return Ok();
        }

        [HttpPut]
        public IActionResult Put([FromBody] UpdateProductDto product, [FromQuery] int Id)
        {
            productRepository.UpdateProduct(Id, product);

            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete([FromQuery] int Id)
        {
            productRepository.DeleteProduct(Id);

            return Ok();
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Please upload a valid file.");

            if (Path.GetExtension(file.FileName).ToLower() != ".json")
                return BadRequest("Only JSON files are allowed.");

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
                    return BadRequest("Invalid JSON format.");
                }
            }

            foreach (var product in products)
            {
                if (string.IsNullOrEmpty(product.ProductName) || product.Price <= 0)
                {
                    return BadRequest("Invalid product data: ProductName is required and Price must be positive.");
                }
            }

            productRepository.InsertProductsByFile(products);

            return Ok("Products successfully uploaded and saved.");
        }
    }
}