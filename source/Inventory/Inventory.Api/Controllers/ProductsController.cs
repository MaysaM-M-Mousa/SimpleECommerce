using Inventory.Api.DTOs;
using Inventory.Api.DTOs.Extensions;
using Inventory.Application.Products.GetAllProducts;
using Inventory.Application.Products.GetProduct;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<List<ProductDto>> GetAllProducts()
    {
        var products = await _mediator.Send(new GetAllProductsQuery());
        return products.ToDto();
    }

    [HttpGet("productId")]
    public async Task<ProductDto?> GetProductById(int productId)
    {
        var product = await _mediator.Send(new GetProductQuery(productId));

        return product is null ? null : product.ToDto();
    }
}
