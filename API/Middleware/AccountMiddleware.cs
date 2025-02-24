using System.Net;
using System.Security.Claims;
using API.DTOs;
using API.Extensions;
using API.RequestHelpers;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Middleware
{
    public class AccountMiddleware(SignInManager<AppUser> signInManager): ControllerBase
    {
        public async Task<ApiBaseResponse<AppUser>> Register(RegisterDTO registerDto)
        {
            ApiBaseResponse<AppUser> response = new();
            try
            {
                AppUser user = new();
                user.FirstName = registerDto.FirstName;
                user.LastName = registerDto.LastName;
                user.Email = registerDto.Email;
                user.UserName = registerDto.Email;

                IdentityResult result = await signInManager.UserManager.CreateAsync(user, registerDto.Password);

                if (!result.Succeeded)
                {
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Message = string.Join(", ", result.Errors.Select(e => e.Description));
                    return response;
                }

                response.StatusCode = (int)HttpStatusCode.OK;
                response.Message = "User registered";
                response.Content = user;
                return response;
            }
            catch
            {
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Message = "Something went wrong";
                return response;
            }
        }

        public async Task<ApiBaseResponse<bool>> Logout()
        {
            ApiBaseResponse<bool> response = new();
            try
            {
                await signInManager.SignOutAsync();
                response.StatusCode = (int)HttpStatusCode.NoContent;
                response.Message = "User logged out";
                response.Content = true;
                return response;
            }
            catch
            {
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Message = "Something went wrong";
                return response;
            }
        }
    
        public async Task<ApiBaseResponse<object>> GetUserInfo()
        {
            ApiBaseResponse<object> response = new();
            try
            {
                if (User.Identity?.IsAuthenticated == false)
                {
                    response.StatusCode = (int)HttpStatusCode.NoContent;
                    response.Message = "User not authenticated";
                    return response;
                }

                var user = await signInManager.UserManager.GetUserByEmailWithAddress(User);

                if (user == null)
                {
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    response.Message = "Unauthorized user";
                    response.Content = false;
                    return response;
                }

                else{

                    var userInfo = new {
                        user.FirstName,
                        user.LastName,
                        user.Email,
                        Address = user.Address?.ToDto()
                    };
                    response.Content = userInfo;
                    response.StatusCode = (int)HttpStatusCode.OK;
                    response.Message = "User found";
                    return response;
                }
            }
            catch
            {
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Message = "Something went wrong";
                return response;
            }
        }

        public ApiBaseResponse<bool> GetAuthState()
        {
            ApiBaseResponse<bool> response = new();
            try
            {
                bool IsAuthenticated = User.Identity?.IsAuthenticated ?? false;
                response.StatusCode = (int)HttpStatusCode.OK;
                response.Message = "Auth state found";
                response.Content = IsAuthenticated;
                return response;
            }
            catch
            {
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Message = "Something went wrong";
                return response;
            }
        }

        public async Task<ApiBaseResponse<AddressDto>> CreateOrUpdateAddress(AddressDto addressDto)
        {
            ApiBaseResponse<AddressDto> response = new();
            try
            {
                var user = await signInManager.UserManager.GetUserByEmailWithAddress(User);

                if (user.Address == null)
                {
                    user.Address = addressDto.ToEntity();
                }
                else
                {
                    user.Address.UpdateFromDto(addressDto);
                }

                var result = await signInManager.UserManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Message = string.Join(", ", result.Errors.Select(e => e.Description));
                    return response;
                }

                response.StatusCode = (int)HttpStatusCode.OK;
                response.Message = "Address updated";
                response.Content = user.Address.ToDto();
                return response;

            }
            catch
            {
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Message = "Something went wrong";
                return response;
            }
        }
    }
}