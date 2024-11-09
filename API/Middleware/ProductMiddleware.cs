using System.Net;
using API.RequestHelpers;
using API.Validators;
using Core.Entities;
using Core.Repository;
using Core.Specifications;
using FluentValidation.Results;

namespace API.Middleware
{
    public class ProductMiddleware(IGenericRepository<Product> _repo)
    {
        public async Task<ApiBaseResponse<Pagination<Product>>> GetProducts(ProductSpecParams productSpec)
        {
            ApiBaseResponse<Pagination<Product>> response = new();
            try
            {
                ProductSpecification spec = new (productSpec);

                IReadOnlyList<Product> products = await _repo.ListAsync(spec);
                int count = await _repo.CountAsync(spec);

                Pagination<Product> pagination = new (productSpec.PageSize,
                productSpec.PageIndex, count, products);

                response.StatusCode = (int)HttpStatusCode.OK;
                response.Message = "Success";
                response.Content = pagination;
                return response;
            }
            catch (Exception e)
            {
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Message = e.Message;
                return response;
            }
        }

        public async Task<ApiBaseResponse<Product>> GetProduct(int id)
        { 
            ApiBaseResponse<Product> response = new();
            try
            {
                Product? product = await _repo.GetByIdAsync(id);

                if (product == null)
                {
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    response.Message = "Product not found";
                    return response;
                }

                response.StatusCode = (int)HttpStatusCode.OK;
                response.Message = "Success";
                response.Content = product;
                return response;
            }
            catch (Exception e)
            {
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Message = e.Message;
                return response;
            }
        }

        public async Task<ApiBaseResponse<IReadOnlyList<string>>> GetBrands()
        {
            ApiBaseResponse<IReadOnlyList<string>> response = new();
            try
            {
                BrandListSpecification spec = new();

                IReadOnlyList<string> brands = await _repo.ListAsync(spec);

                if (brands.Count == 0)
                {
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    response.Message = "Brands not found";
                    return response;
                }

                response.StatusCode = (int)HttpStatusCode.OK;
                response.Message = "Success";
                response.Content = brands;
                return response;
            }
            catch (Exception e)
            {
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Message = e.Message;
                return response;
            }
        }

        public async Task<ApiBaseResponse<IReadOnlyList<string>>> GetTypes()
        {
            ApiBaseResponse<IReadOnlyList<string>> response = new();
            try
            {
                TypeListSpecification spec = new();

                IReadOnlyList<string> types = await _repo.ListAsync(spec);

                if (types.Count == 0)
                {
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    response.Message = "Types not found";
                    return response;
                }

                response.StatusCode = (int)HttpStatusCode.OK;
                response.Message = "Success";
                response.Content = types;
                return response;
            }
            catch (Exception e)
            {
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Message = e.Message;
                return response;
            }
        }

        public async Task<ApiBaseResponse<Product>> CreateProduct(Product product){

            ApiBaseResponse<Product> response = new();
            try
            {
                CreateProductValidator validationRules = new();
                ValidationResult validationResult = validationRules.Validate(product);

                if (!validationResult.IsValid)
                {
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Message = validationResult.Errors.FirstOrDefault()!.ToString();
                    return response;
                }

                _repo.Add(product);
                
                if (await _repo.SaveChangesAsync())
                {
                    response.StatusCode = (int)HttpStatusCode.Created;
                    response.Message = "Product created";
                    response.Content = product;
                    return response;
                }

                response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.Message = "Product not created";
                return response;
            }
            catch (Exception e)
            {
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Message = e.Message;
                return response;
            }
        }

        public async Task<ApiBaseResponse<bool>> UpdateProduct(int id, Product product)
        {
            ApiBaseResponse<bool> response = new();
            try
            {
                if (product.Id != id || !_repo.EntityExists(id))
                {
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Message = "Invalid product";
                    return response;
                }

                UpdateProductValidator validationRules = new();
                ValidationResult validationResult = validationRules.Validate(product);

                if (!validationResult.IsValid)
                {
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Message = validationResult.Errors.FirstOrDefault()!.ToString();
                    return response;
                }

                _repo.Update(product);

                if (await _repo.SaveChangesAsync())
                {
                    response.StatusCode = (int)HttpStatusCode.OK;
                    response.Message = "Product updated";
                    response.Content = true;
                    return response;
                }

                response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.Message = "Product not updated";
                return response;
            }
            catch (Exception e)
            {
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Message = e.Message;
                return response;
            }
        }

        public async Task<ApiBaseResponse<bool>> DeleteProduct(int id)
        {
            ApiBaseResponse<bool> response = new();
            try
            {
                Product? product = await _repo.GetByIdAsync(id);

                if (product == null)
                {
                    response.StatusCode = (int)HttpStatusCode.NotFound;;
                    response.Message = "Product not found";
                    return response;
                }

                _repo.Remove(product);

                if (await _repo.SaveChangesAsync())
                {
                    response.StatusCode = (int)HttpStatusCode.OK;;
                    response.Message = "Product deleted";
                    response.Content = true;
                    return response;
                }

                response.StatusCode = (int)HttpStatusCode.BadRequest;;
                response.Message = "Product not deleted";
                return response;
            }
            catch (Exception e)
            {
                response.StatusCode = (int)HttpStatusCode.InternalServerError;;
                response.Message = e.Message;
                return response;
            }
        }
    }
}