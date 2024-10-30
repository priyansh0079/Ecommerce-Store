using Core.Entities;
using Core.Repository;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController(IGenericRepository<Product> _repo): ControllerBase
    {
        [HttpGet]
        [Route("list")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(
            string? brand, string? type, string? sort)
        {
            ProductSpecification spec = new (brand, type, sort);

            IReadOnlyList<Product> products = await _repo.ListAsync(spec);

            return Ok(products);
        }

        [HttpGet]
        [Route("get")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        { 
            Product? product = await _repo.GetByIdAsync(id);
            if (product == null) return NotFound();
            return product;
        }

        [HttpGet]
        [Route("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
        {
            BrandListSpecification spec = new(); 

            return Ok(await _repo.ListAsync(spec));
        }

        [HttpGet]
        [Route("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
        {
            TypeListSpecification spec = new();

            return Ok(await _repo.ListAsync(spec));
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<Product>> CreateProduct(Product product){
            _repo.Add(product);

            if (await _repo.SaveChangesAsync()){
                return CreatedAtAction("GetProduct", new { id = product.Id }, product);
            }

            return BadRequest("Product not created");
        }

        [HttpPut]
        [Route("update")]
        public async Task<ActionResult> UpdateProduct(int id, Product product){
            if (product.Id != id || !_repo.EntityExists(id))
             return BadRequest("Can't update this product");

            _repo.Update(product);

            if (await _repo.SaveChangesAsync()){
                return NoContent();
            }

            return BadRequest("Product not updated");
        }

        [HttpDelete]
        [Route("delete")]
        public async Task<ActionResult> DeleteProduct(int id){
            Product? product = await _repo.GetByIdAsync(id);
            if (product == null) return NotFound();

            _repo.Remove(product);
            if (await _repo.SaveChangesAsync()){
                return NoContent();
            }

            return BadRequest("Product not deleted");
        }

        private bool ProductExists(int id){
            return _repo.EntityExists(id);
        }
    }
}