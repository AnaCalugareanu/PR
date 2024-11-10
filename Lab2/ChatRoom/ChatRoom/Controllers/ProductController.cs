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

        [HttpPost("uploadJsonFile")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateFile(IFormFile file)
        {
            if (file.Length == 0 || file.Length > 20971520 || file.ContentType != "application/json")
            {
                return BadRequest("No file or an invalid one has been inputted. Only JSON files are accepted.");
            }

            var resultMessage = await productRepository.ProcessAndInsertProductsFromFile(file);

            if (resultMessage.StartsWith("Invalid"))
                return BadRequest(resultMessage);

            return Ok(resultMessage);
        }
    }
}