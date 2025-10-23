using Microsoft.AspNetCore.Mvc;
using ProjectIndependence.API.Core.Dtos.Products;
using ProjectIndependence.API.Core.Errors;
using ProjectIndependence.API.Core.Interfaces.ServiceInterfaces.Products;
using ProjectIndependence.API.Core.Products.Commands.CreateProduct;
using ProjectIndependence.API.Core.Products.Commands.UpdateProduct;
using ProjectIndependence.API.Core.Products.Dtos;
using ProjectIndependence.API.Core.Products.Queries.GetProductById;
using ProjectIndependence.API.Core.Products.Queries.ProductList;
using ProjectIndependence.API.Core.Response;

namespace ProjectIndependence.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly CreateProductCommandHandler _createProductCommandHandler;
        private readonly ProductListQueryHandler _productListQueryHandler;
        private readonly GetProductByIdQueryHandler _getProductByIdQueryHandler;
        private readonly UpdateProductCommandHandler _updateProductCommandHandler;

        public ProductsController(IProductService productService, CreateProductCommandHandler creatingProductHandler, ProductListQueryHandler productListQueryHandler, GetProductByIdQueryHandler getProductByIdQueryHandler, UpdateProductCommandHandler updateProductCommandHandler)
        {
            _productService = productService;
            _createProductCommandHandler = creatingProductHandler;
            _productListQueryHandler = productListQueryHandler;
            _getProductByIdQueryHandler = getProductByIdQueryHandler;
            _updateProductCommandHandler = updateProductCommandHandler;
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
            var products = await _productListQueryHandler.HandleAsync(null);

            return Ok(ApiResponse<IEnumerable<ProductListDto>>.FromSucces(products));
        }

        /// <summary>
        /// Adds a new product
        /// </summary>
        /// <param name="dtoCreateProduct">Values for a new product</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostAsync(CreateProductCommand createProductCommand)
        {
            if (createProductCommand == null)
            {
                var problem = new ProblemDetails
                {
                    Title = "Invalid body request",
                    Detail = "The request body was empty or could not be serialized",
                    Status = StatusCodes.Status400BadRequest
                };

                return BadRequest(ApiResponse<object>.FromError(problem));
            }

            var newProduct = await _createProductCommandHandler.HandleAsync(createProductCommand);

            return CreatedAtRoute(
                "GetProductById",
                new { id = newProduct.Id },
                ApiResponse<ProductDto>.FromSucces(newProduct));
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
        public async Task<IActionResult> Update(Guid id, UpdateProductCommand updateProductCommand)
        {
            if (id != updateProductCommand.Id)
            {
                var problemDetail = new ProblemDetails
                {
                    Title = ValidationErrors.IdsNotMatchingTitle,
                    Detail = ValidationErrors.IdsNotMatching,
                    Status = StatusCodes.Status404NotFound
                };

                return NotFound(ApiResponse<object>.FromError(problemDetail));
            }

            var updatedProduct = await _updateProductCommandHandler.HandleAsync(updateProductCommand);

            if (updatedProduct is null)
            {
                var problemDetail = new ProblemDetails
                {
                    Title = ValidationErrors.NotFoundTitle,
                    Detail = ValidationErrors.ProductNotFound + id,
                    Status = StatusCodes.Status404NotFound
                };

                return NotFound(ApiResponse<object>.FromError(problemDetail));
            }

            return Ok(ApiResponse<ProductDto>.FromSucces(updatedProduct));
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
            var productQuery = new GetProductByIdQuery(id);

            var product = await _getProductByIdQueryHandler.HandleAsync(productQuery);

            if (product is null)
            {
                var notFound = new ProblemDetails
                {
                    Title = ValidationErrors.NotFoundTitle,
                    Detail = ValidationErrors.ProductNotFound + id,
                    Status = StatusCodes.Status404NotFound
                };

                return NotFound(ApiResponse<object>.FromError(notFound));
            }

            return Ok(ApiResponse<ProductDto>.FromSucces(product));
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
                    Title = ValidationErrors.NotFoundTitle,
                    Detail = ValidationErrors.ProductNotFound + id,
                    Status = StatusCodes.Status404NotFound
                };

                return NotFound(ApiResponse<object>.FromError(notFound));
            }

            return Ok(ApiResponse<object>.FromSuccesfullyDeleted("The product has been successfully deleted"));
        }
    }
}