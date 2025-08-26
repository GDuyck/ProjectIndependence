using Microsoft.AspNetCore.Mvc;
using ProjectIndependence.API.Core.Dtos.Products;
using ProjectIndependence.API.Core.Interfaces.ServiceInterfaces.Products;
using ProjectIndependence.API.Core.Response;

namespace ProjectIndependence.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        public readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Get all products
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync()
        {
            var products = await _productService.GetAllAsync();

            return Ok(ApiResponse<IEnumerable<DtoProduct>>.FromSucces(products));
        }

        /// <summary>
        /// Adds a new product
        /// </summary>
        /// <param name="dtoCreateProduct">Values for a new product</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostAsync(DtoCreateProduct dtoCreateProduct)
        {
            if (dtoCreateProduct == null)
            {
                var problem = new ProblemDetails
                {
                    Title = "Invalid body request",
                    Detail = "The request body was empty or could not be serialized",
                    Status = StatusCodes.Status400BadRequest
                };

                return BadRequest(ApiResponse<object>.FromError(problem));
            }

            var newProduct = await _productService.AddAsync(dtoCreateProduct);

            return CreatedAtRoute(
                "GetProductById",
                new { id = newProduct.Id },
                ApiResponse<DtoProduct>.FromSucces(newProduct));
        }

        /// <summary>
        /// Updates a product
        /// </summary>
        /// <param name="id">The ID of the product to update.</param>
        /// <param name="dto">The updated product data.</param>
        /// <returns>
        /// returns a 200 OK with apiresponse succes is successfull
        /// 400 if problems with validation, 404 if not found,
        /// </returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<DtoProduct>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid id, DtoCreateProduct dto)
        {
            if (id != dto.Id)
            {
                var problemDetail = new ProblemDetails
                {
                    Title = "Id's don't match",
                    Detail = "The ID in the URL doesn't match the ID in the body",
                    Status = StatusCodes.Status404NotFound
                };

                return BadRequest(ApiResponse<object>.FromError(problemDetail));
            }

            var updatedProduct = await _productService.UpdateAsync(dto);

            if (updatedProduct is null)
            {
                var problemDetail = new ProblemDetails
                {
                    Title = "Product not found",
                    Detail = $"There was no product found with the ID {id}",
                    Status = StatusCodes.Status404NotFound
                };

                return NotFound(ApiResponse<object>.FromError(problemDetail));
            }

            return Ok(ApiResponse<DtoProduct>.FromSucces(updatedProduct));
        }

        /// <summary>
        /// Gets product by id number
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetProductById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DtoProduct>> GetByIdAsync(Guid id)
        {
            var product = await _productService.GetByIdAsync(id);

            if (product is null)
            {
                var notFound = new ProblemDetails
                {
                    Title = "Not found",
                    Detail = $"No product found with the id {id}",
                    Status = StatusCodes.Status404NotFound
                };

                return NotFound(ApiResponse<object>.FromError(notFound));
            }

            return Ok(ApiResponse<DtoProduct>.FromSucces(product));
        }

        /// <summary>
        /// Deletes an existing product with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the product to delete.</param>
        /// <returns>
        /// Returns a 200 OK response if the deletion is successful,
        /// 404 Not Found if the product does not exist.
        /// </returns>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _productService.DeleteAsync(id);

            if (!deleted)
            {
                var notFound = new ProblemDetails
                {
                    Title = "Not found",
                    Detail = $"No product found with the id {id}",
                    Status = StatusCodes.Status404NotFound
                };

                return NotFound(ApiResponse<object>.FromError(notFound));
            }

            return Ok(ApiResponse<object>.FromSuccesfullyDeleted("The product has been successfully deleted"));
        }
    }
}