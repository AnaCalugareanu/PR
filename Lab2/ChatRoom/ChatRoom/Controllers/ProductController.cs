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
        public IActionResult Get()
        {
            return Ok(productRepository.GetProducts());
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
    }
}