using API.DTOs;
using API.Middleware;
using API.RequestHelpers;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("[controller]")]
    public class AccountController(AccountMiddleware accountMiddleware) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<ApiBaseResponse<AppUser>>> Register(RegisterDTO registerDto)
        {
            var response = await accountMiddleware.Register(registerDto);
            return response;
        }
        
        [Authorize] 
        [HttpPost("logout")]
        public async Task<ActionResult<ApiBaseResponse<bool>>> Logout()
        {
            ApiBaseResponse<bool> response = await accountMiddleware.Logout();
            return response;
        }

        [HttpGet("user-info")]
        public async Task<ActionResult<ApiBaseResponse<object>>> GetUserInfo()
        {
            var userInfo =  await accountMiddleware.GetUserInfo();
            return userInfo;
        }

        [HttpGet]
        public ActionResult<ApiBaseResponse<bool>> GetAuthStatus()
        {
            var response = accountMiddleware.GetAuthState();
            return response;
        }

        [Authorize]
        [HttpPost("address")]
        public async Task<ActionResult<ApiBaseResponse<AddressDto>>> CreateOrUpdateAddress(AddressDto address)
        {
            var response = await accountMiddleware.CreateOrUpdateAddress(address);
            return response;
        }
    }
}