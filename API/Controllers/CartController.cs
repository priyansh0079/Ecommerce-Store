using API.Middleware;
using API.RequestHelpers;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController(CartMiddleware cartMiddleware) : ControllerBase
    {
        [HttpGet("get")]
        public async Task<ActionResult<ApiBaseResponse<ShoppingCart>>> GetCartById(string id)
        {
            ApiBaseResponse<ShoppingCart> response = await cartMiddleware.GetCartById(id);
            return response;
        }

        [HttpPost("set")]
        public async Task<ActionResult<ApiBaseResponse<ShoppingCart>>> UpdateCart(ShoppingCart cart)
        {
            ApiBaseResponse<ShoppingCart> response = await cartMiddleware.UpdateCart(cart);
            return response;
        }

        [HttpDelete("delete")]
        public async Task<ActionResult<ApiBaseResponse<bool>>> DeleteCart(string id)
        {
            ApiBaseResponse<bool> response = await cartMiddleware.DeleteCart(id);
            return response;
        }
    }
}