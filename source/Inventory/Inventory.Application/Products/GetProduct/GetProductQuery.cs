using Inventory.Domain.Products;
using MediatR;

namespace Inventory.Application.Products.GetProduct;

public record GetProductQuery(int ProductId) : IRequest<Product?>;
