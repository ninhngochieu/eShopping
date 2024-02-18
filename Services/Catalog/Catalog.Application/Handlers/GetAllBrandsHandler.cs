using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers;

//Todo: 2.13.3 Implement Handler for Query and get response
/// <summary>
/// First Parameter: Query
/// Second Parameter: Response
/// </summary>
public class GetAllBrandsHandler : IRequestHandler<GetAllBrandsQuery, IList<BrandResponse>>
{
    private readonly IBrandRepository _brandRepository;
    
    public GetAllBrandsHandler(IBrandRepository brandRepository)
    {
        _brandRepository = brandRepository;
    }
    /// <summary>
    /// Nhận request, xử lý xong bắt buộc phải trả về theo định nghĩa trước theo Query
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IList<BrandResponse>> Handle(GetAllBrandsQuery request, CancellationToken cancellationToken)
    {
        var brandList = await _brandRepository.GetAllBrands();
        var brandResponseList = ProductMapper.Mapper.Map<IList<ProductBrand>, IList<BrandResponse>>(brandList.ToList());
        return brandResponseList;
    }
}