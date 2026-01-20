using Microsoft.AspNetCore.Mvc;
using ProjectIndependence.API.Application.Products.Commands.AdjustProductStock;
using ProjectIndependence.API.Application.Products.Commands.CreateProduct;
using ProjectIndependence.API.Application.Products.Commands.ToggleProductStatus;
using ProjectIndependence.API.Application.Products.Commands.UpdateProduct;
using ProjectIndependence.API.Application.Products.Commands.UpdateProductPrice;
using ProjectIndependence.API.Application.Products.Dtos;
using ProjectIndependence.API.Application.Products.Queries.GetProductById;
using ProjectIndependence.API.Application.Products.Queries.ProductList;
using ProjectIndependence.API.Controllers.Base;
using ProjectIndependence.API.Core.Entities.Products;
using ProjectIndependence.API.Core.Interfaces;
using ProjectIndependence.API.Core.Response;

namespace ProjectIndependence.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ApiBaseController
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
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
            var query = new ProductListQuery();
            var products = await _mediator.QueryAsync<ProductListQuery, List<ProductListDto>>(query);

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

            var product = await _mediator.QueryAsync<GetProductByIdQuery, ProductDto>(productQuery);

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

            var newProduct = await _mediator.SendAsync<CreateProductCommand, ProductDto>(createProductCommand);

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

            var updatedProduct = await _mediator.SendAsync<UpdateProductCommand, ProductDto>(updateProductCommand);

            if (updatedProduct is null)
            {
                return NotFoundResponse(nameof(Product), null);
            }

            return OkResponse<ProductDto>(updatedProduct);
        }

        #endregion PUT

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

            var updatedProductDto = await _mediator.SendAsync<UpdateProductPriceCommand, ProductDto>(command);

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

            var updatedProductDto = await _mediator.SendAsync<AdjustProductStockCommand, ProductDto>(command);

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

            var updatedProductDto = await _mediator.SendAsync<ProductStatusCommand, ProductDto>(command);

            if (updatedProductDto is null)
                return NotFoundResponse(nameof(Product), id);

            return OkResponse<ProductDto>(updatedProductDto);
        }

        #endregion PATCH
    }
}