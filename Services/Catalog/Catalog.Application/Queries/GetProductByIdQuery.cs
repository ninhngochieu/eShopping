using Catalog.Application.Responses;
using MediatR;

namespace Catalog.Application.Queries;

//Todo: 2.17.1 Get Item Query By Parameter
public class GetProductByIdQuery: IRequest<ProductResponse>
{
    public string Id { get; set; }

    /// <summary>
    /// Assign Id using Setter for Query 
    /// </summary>
    /// <param name="id"></param>
    public GetProductByIdQuery(string id)
    {
        Id = id;
    }
}