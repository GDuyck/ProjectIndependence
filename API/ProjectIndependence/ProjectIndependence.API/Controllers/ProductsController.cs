using Microsoft.AspNetCore.Mvc;
using ProjectIndependence.API.Controllers.Base;
using ProjectIndependence.API.Core.Entities.Products;
using ProjectIndependence.API.Core.Errors;
using ProjectIndependence.API.Core.Interfaces.ServiceInterfaces.Products;
using ProjectIndependence.API.Core.Products.Commands.AdjustProductStock;
using ProjectIndependence.API.Core.Products.Commands.CreateProduct;
using ProjectIndependence.API.Core.Products.Commands.ToggleProductStatus;
using ProjectIndependence.API.Core.Products.Commands.UpdateProduct;
using ProjectIndependence.API.Core.Products.Commands.UpdateProductPrice;
using ProjectIndependence.API.Core.Products.Dtos;
using ProjectIndependence.API.Core.Products.Queries.GetProductById;
using ProjectIndependence.API.Core.Products.Queries.ProductList;
using ProjectIndependence.API.Core.Response;

namespace ProjectIndependence.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ApiBaseController
    {
        private readonly IProductService _productService;
        private readonly CreateProductCommandHandler _createProductCommandHandler;
        private readonly ProductListQueryHandler _productListQueryHandler;
        private readonly GetProductByIdQueryHandler _getProductByIdQueryHandler;
        private readonly UpdateProductCommandHandler _updateProductCommandHandler;
        private readonly UpdateProductPriceCommandHandler _updateProductPriceCommandHandler;
        private readonly AdjustProductStockCommandHandler _adjustProductStockCommandHandler;
        private readonly ProductStatusCommandHandler _updateProductStatusCommandHandler;

        public ProductsController(IProductService productService, CreateProductCommandHandler creatingProductHandler, ProductListQueryHandler productListQueryHandler, GetProductByIdQueryHandler getProductByIdQueryHandler, UpdateProductCommandHandler updateProductCommandHandler, UpdateProductPriceCommandHandler updateProductPriceCommandHandler, AdjustProductStockCommandHandler adjustProductStockCommandHandler, ProductStatusCommandHandler updateProductStatusCommandHandler)
        {
            _productService = productService;
            _createProductCommandHandler = creatingProductHandler;
            _productListQueryHandler = productListQueryHandler;
            _getProductByIdQueryHandler = getProductByIdQueryHandler;
            _updateProductCommandHandler = updateProductCommandHandler;
            _updateProductPriceCommandHandler = updateProductPriceCommandHandler;
            _adjustProductStockCommandHandler = adjustProductStockCommandHandler;
            _updateProductStatusCommandHandler = updateProductStatusCommandHandler;
        }

        #region GET

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

            return OkResponse<IEnumerable<ProductListDto>>(products);
        }

        /// <summary>
        /// Gets product by id number
        /// </summary>
        /// <param name="id">product id</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetProductById")]
        [ProducesResponseType(typeof(ApiResponse<ProductDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetByIdAsync(Guid id)
        {
            var productQuery = new GetProductByIdQuery(id);

            var product = await _getProductByIdQueryHandler.HandleAsync(productQuery);

            if (product is null)
            {
                return NotFoundResponse(nameof(Product), id);
            }

            return OkResponse<ProductDto>(product);
        }

        #endregion GET

        #region POST

        /// <summary>
        /// Adds a new product
        /// </summary>
        /// <param name="createProductCommand">Values for a new product</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApiResponse<ProductDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
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

            return CreatedAtResponse(newProduct, "GetProductById", new { id = newProduct.Id });
        }

        #endregion POST

        #region PUT

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
        [ProducesResponseType(typeof(ApiResponse<ProductDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid id, UpdateProductCommand updateProductCommand)
        {
            if (id != updateProductCommand.Id)
            {
                return MismatchResponse();
            }

            var updatedProduct = await _updateProductCommandHandler.HandleAsync(updateProductCommand);

            if (updatedProduct is null)
            {
                return NotFoundResponse(nameof(Product), null);
            }

            return OkResponse<ProductDto>(updatedProduct);
        }

        #endregion PUT

        #region DELETE

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

        #endregion DELETE

        #region PATCH

        /// <summary>
        /// Updates a productprice
        /// </summary>
        /// <param name="id">The ID of the product to update.</param>
        /// <param name="command">The updated product data.</param>
        /// <returns>
        /// returns a 200 OK with apiresponse succes is successfull
        /// 400 if problems with validation, 404 if not found,
        /// </returns>
        [ProducesResponseType(typeof(ApiResponse<ProductDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [HttpPatch("{id}/price")]
        public async Task<IActionResult> UpdatePrice(Guid id, [FromBody] UpdateProductPriceCommand command)
        {
            if (id != command.Id)
            {
                return MismatchResponse();
            }

            var updatedProductDto = await _updateProductPriceCommandHandler.HandleAsync(command);

            if (updatedProductDto is null)
            {
                return NotFoundResponse(nameof(Product), id);
            }

            return OkResponse<ProductDto>(updatedProductDto);
        }

        /// <summary>
        /// Updates product stock
        /// </summary>
        /// <param name="id">The ID of the product to update.</param>
        /// <param name="command">The updated product data.</param>
        /// <returns>
        /// returns a 200 OK with apiresponse succes is successfull
        /// 400 if problems with validation, 404 if not found,
        /// </returns>
        [ProducesResponseType(typeof(ApiResponse<ProductDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [HttpPatch("{id}/stock")]
        public async Task<IActionResult> AdjustProductStock(Guid id, AdjustProductStockCommand command)
        {
            if (id != command.Id)
                return MismatchResponse();

            var updatedProductDto = await _adjustProductStockCommandHandler.HandleAsync(command);

            if (updatedProductDto is null)
                return NotFoundResponse(nameof(Product), id);

            return OkResponse<ProductDto>(updatedProductDto);
        }

        /// <summary>
        /// Updates product status
        /// </summary>
        /// <param name="id">The ID of the product to update.</param>
        /// <param name="command">The updated product data.</param>
        /// <returns>
        /// returns a 200 OK with apiresponse succes is successfull
        /// 400 if problems with validation, 404 if not found,
        /// </returns>
        [ProducesResponseType(typeof(ApiResponse<ProductDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> ChangeProductStatus(Guid id, ProductStatusCommand command)
        {
            if (id != command.Id)
                return MismatchResponse();

            var updatedProductDto = await _updateProductStatusCommandHandler.HandleAsync(command);

            if (updatedProductDto is null)
                return NotFoundResponse(nameof(Product), id);

            return OkResponse<ProductDto>(updatedProductDto);
        }

        #endregion PATCH
    }
}