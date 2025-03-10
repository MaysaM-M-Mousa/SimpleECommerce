using Inventory.Domain.Products;
using MediatR;

namespace Inventory.Application.Products.GetAllProducts;

public record GetAllProductsQuery : IRequest<List<Product>>;
