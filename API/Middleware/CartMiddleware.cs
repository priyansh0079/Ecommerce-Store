using System.Net;
using API.RequestHelpers;
using Core.Entities;
using Core.Repository;

namespace API.Middleware
{
    public class CartMiddleware(ICartService cartService)
    {
        public async Task<ApiBaseResponse<ShoppingCart>> GetCartById(string id)
        {
            ApiBaseResponse<ShoppingCart> response = new();
            try
            {
                ShoppingCart? cart = await cartService.GetCartAsync(id);

                if (cart == null)
                {
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    response.Message = "Cart not found";
                    response.Content = new ShoppingCart { Id = id };
                    return response;
                }
                
                response.StatusCode = (int)HttpStatusCode.OK;
                response.Message = "Cart found";
                response.Content = cart;
                return response;
            }
            catch (Exception ex)
            {
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Message = ex.Message;
                return response;
            }
        }

        public async Task<ApiBaseResponse<ShoppingCart>> UpdateCart(ShoppingCart cart)
        {
            ApiBaseResponse<ShoppingCart> response = new();
            try
            {
                 ShoppingCart? updatedCart = await cartService.SetCartAsync(cart);

                if (updatedCart == null)
                {
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Message = "Problem updating the cart";
                    return response;
                }

                response.StatusCode = (int)HttpStatusCode.OK;
                response.Message = "Cart updated";
                response.Content = updatedCart;
                return response;
            }
            catch (Exception ex)
            {
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Message = ex.Message;
                return response;
            }
        }

        public async Task<ApiBaseResponse<bool>> DeleteCart(string id)
        {
            ApiBaseResponse<bool> response = new();
            try
            {
                bool results = await cartService.DeleteCartAsync(id);

                if (!results)
                {
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Message = "Problem deleting the cart";
                    return response;
                }

                response.StatusCode = (int)HttpStatusCode.OK;
                response.Message = "Cart deleted";
                response.Content = results;
                return response;
            }
            catch (Exception ex)
            {
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Message = ex.Message;
                return response;
            }
        }
    }
}