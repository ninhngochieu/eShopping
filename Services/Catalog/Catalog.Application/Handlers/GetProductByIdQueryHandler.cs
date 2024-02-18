using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers;

/// <summary>
/// Todo: 2.17.1 Handler for Query By Parameter
/// First Parameter: Query
/// Second Parameter: Response
/// </summary>
public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductResponse>
{
    private readonly IProductRepository _productRepository;

    public GetProductByIdQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    
    /// <summary>
    /// Parameter đã xuất hiện xong Query, chỉ cần móc ra để truy vấn và trả về đúng kiểu dữ liệu theo Query
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ProductResponse> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetProduct(request.Id);
        var productResponse = ProductMapper.Mapper.Map<ProductResponse>(product);
        return productResponse;
    }
}