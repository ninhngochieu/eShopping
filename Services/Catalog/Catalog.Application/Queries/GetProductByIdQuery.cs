using Catalog.Application.Responses;
using MediatR;

namespace Catalog.Application.Queries;

/// <summary>
/// 2.17.1 Get Item Query By Parameter
/// </summary>
public class GetProductByIdQuery: IRequest<ProductResponse>
{
    public string Id { get; set; }

    /// <summary>
    /// Assign Id using Setter for Query
    /// Truyền các tham số, các thông tin cần thiết để đem đi truy vấn
    /// </summary>
    /// <param name="id"></param>
    public GetProductByIdQuery(string id)
    {
        Id = id;
    }
}