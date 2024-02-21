using MediatR;

namespace Basket.Application.Queries;

/// <summary>
/// Todo: 3.8.1 Delete action query / command
/// Không trả về kiểu dữ liệu nào
/// </summary>
public class DeleteBasketByUserNameQuery : IRequest
{
    public string UserName { get; set; }

    public DeleteBasketByUserNameQuery(string userName)
    {
        UserName = userName;
    }
}