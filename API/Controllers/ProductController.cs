using API.Middleware;
using API.RequestHelpers;
using Core.Entities;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController(ProductMiddleware _productServices): ControllerBase
    {
        [HttpGet]
        [Route("list")]
        public async Task<ActionResult<ApiBaseResponse<Pagination<Product>>>> GetProducts(
            [FromQuery] ProductSpecParams productSpec)
        {
            ApiBaseResponse<Pagination<Product>> products = await _productServices.GetProducts(productSpec);
            return products;
        }

        [HttpGet]
        [Route("get")]
        public async Task<ActionResult<ApiBaseResponse<Product>>> GetProduct(int id)
        { 
            ApiBaseResponse<Product> product = await _productServices.GetProduct(id);
            return product;
        }

        [HttpGet]
        [Route("brands")]
        public async Task<ActionResult<ApiBaseResponse<IReadOnlyList<string>>>> GetBrands()
        {
            ApiBaseResponse<IReadOnlyList<string>> brands = await _productServices.GetBrands();
            return brands; 
        }

        [HttpGet]
        [Route("types")]
        public async Task<ActionResult<ApiBaseResponse<IReadOnlyList<string>>>> GetTypes()
        {
            ApiBaseResponse<IReadOnlyList<string>> types = await _productServices.GetTypes();
            return types;
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<ApiBaseResponse<Product>>> CreateProduct(Product product)
        {
            ApiBaseResponse<Product> response = await _productServices.CreateProduct(product);
            return response;
        }

        [HttpPut]
        [Route("update")]
        public async Task<ActionResult<ApiBaseResponse<bool>>> UpdateProduct(int id, Product product)
        {
            ApiBaseResponse<bool> response = await _productServices.UpdateProduct(id, product);
            return response;
        }

        [HttpDelete]
        [Route("delete")]
        public async Task<ActionResult<ApiBaseResponse<bool>>> DeleteProduct(int id)
        {
            ApiBaseResponse<bool> response = await _productServices.DeleteProduct(id);
            return response;
        }
    }
}