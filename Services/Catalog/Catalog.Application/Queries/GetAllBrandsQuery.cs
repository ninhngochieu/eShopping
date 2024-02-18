using Catalog.Application.Responses;
using MediatR;

namespace Catalog.Application.Queries;

/// <summary>
/// Todo: 2.13.1 Implement Query for Handler
/// Định nghĩa hành vi và kiểu data sẽ trả về
/// IRequest<T>: Loại request cho Handler
/// T: Kiểu trả về dữ liệu Response sau khi Handler xử lý xong
/// </summary>
public class GetAllBrandsQuery : IRequest<IList<BrandResponse>>
{
    
}